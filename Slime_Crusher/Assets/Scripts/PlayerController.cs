using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Burst.CompilerServices;

public class PlayerController : MonoBehaviour
{
    private StageManager stageManager;
    private ItemSkill itemSkill;
    private AudioManager audioManager;
    private CharacterSkill charaterSkill;
    private Combo combo;

    public GameObject[] playerHealthUI; // 체력 표시
    public GameObject healthEffect; // 피격 표시 이펙트
    public int playerHealth; // 현재 체력
    public GameObject gameover; // 게임 종료 표시
    public bool die; // 사망 하였는지

    public int damage; // 데미지
    public int comboDamage; // 콤보로 증가한 데미지
    public bool comboDamageUP; // 데미지 무한 증가 방지 변수
    public bool isAttacking = false; // 공격 속도 (다음 공격까지 시간을 주기위함)
    public bool hitBullet = true; // 몬스터에게 공격 받았는지
    public GameObject hubDamageText; // 데미지 시각적 표시

    public GameObject defenseEffect; // 방어 이펙트
    public bool defending; // 방어 중인지
    public float defenseTime; // 방어 쿨타임
    public GameObject defenseCoolTime; // 쿨타임 표시
    public TMP_Text defenseCoolTimeText;

    public int money;

    public float gameTime; // 총시간 표시
    public TMP_Text gameTimeText;

    public bool isDragging = false; // 드래그 중인지

    public GameObject attckEffect; // 공격 이펙트
    public GameObject dragEffect;

    public bool stage5Debuff = false; //보스 스킬 관련
    public bool isStageHit = true; 

    public Vector3 lastClickPosition; // 마지막 클릭위치

    private void Awake()
    {
        stageManager = GameObject.Find("Manager").GetComponent<StageManager>();
        itemSkill = GameObject.Find("Manager").GetComponent<ItemSkill>();
        audioManager = GameObject.Find("Manager").GetComponent<AudioManager>();
        charaterSkill = GameObject.Find("Manager").GetComponent<CharacterSkill>();
        combo = GameObject.Find("Manager").GetComponent<Combo>();
    }

    void Start()
    {
        die = false;

        defending = false;

        damage = 10;
        playerHealth = 8;
        UpdateHealthUI();
        comboDamageUP = false;

        gameTime = 0f;
        lastClickPosition = Vector3.zero;
    }

    void Update()
    {
        UpdateHealthUI();

        // 방어 쿨타임 관련
        if (defenseTime >= 0)
        {
            defenseCoolTimeText.text = ((int)defenseTime).ToString();
            defenseTime -= Time.deltaTime;
        }
        else
        {
            defenseCoolTime.SetActive(false);
        }

        // 총 시간 표시
        gameTime += Time.deltaTime;
        gameTimeText.text = string.Format("{0:00}:{1:00}", Mathf.Floor(gameTime / 60), gameTime % 60);
        
        // 드래그 공격 관련
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            dragEffect.SetActive(true);
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            dragEffect.SetActive(false);
        }

        // 드래그 공격 이펙트
        if (isDragging)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            dragEffect.transform.position = new Vector3(mousePosition.x, mousePosition.y, dragEffect.transform.position.z);
        }

        // 공격 관련
        if (isDragging && !isAttacking)
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

            lastClickPosition = worldPoint;

            // 공격 시
            if (hit.collider != null)
            {
                MonsterController monsterController = hit.collider.GetComponent<MonsterController>();

                // 보스 스킬에 공격이 닿았을시
                if (hit.collider.CompareTag("Stage6") || hit.collider.CompareTag("Stage4") || hit.collider.CompareTag("Stage7"))
                {
                    if (isStageHit)
                    {                    
                        StartCoroutine(StageBossHit());
                    }
                }

                // 회복 아이템 획득
                if (hit.collider.CompareTag("HealthUp"))
                {
                    if(playerHealth >= 8)
                    {
                        Destroy(hit.collider.gameObject);
                    }
                    else
                    {
                        playerHealth++;
                        Destroy(hit.collider.gameObject);
                    }
                }

                // 몬스터 공격에 닿을시
                if (hit.collider.CompareTag("Bullet") && hitBullet)
                {
                    audioManager.HitAudio();

                    playerHealth -= 1; // 체력 감소
                    combo.comboNum = 0; // 콤보 초기화
                    Vector3 effectPos = new Vector3(transform.position.x, transform.position.y, transform.position.z - 6f); // 피격 이펙트
                    GameObject effect = Instantiate(healthEffect, transform.position, Quaternion.identity);
                    StartCoroutine(BulletHitCooldown(0.2f)); // 재 피격가능까지 시간
                }

                // 몬스터 공격시
                if (monsterController != null)
                {
                    if (monsterController.boss1Defending)
                    {
                        // 1스테이지 보스 스킬중 몬스터 공격 불가
                    }
                    else if (monsterController.moved && hitBullet) // 이동관련 공격 몬스터
                    {
                        playerHealth -= 1;
                        combo.comboNum = 0;
                        Vector3 effectPos = new Vector3(transform.position.x, transform.position.y, transform.position.z - 6f);
                        GameObject effect = Instantiate(healthEffect, transform.position, Quaternion.identity);
                        StartCoroutine(BulletHitCooldown(0.2f));
                    }
                    else
                    {
                        StartCoroutine(AttackMonster(monsterController)); // 몬스터 공격 성공
                    }
                }
            }
        }

        // 콤보 증가시 데미지 증가
        if (comboDamageUP)
        {
            if(combo.comboNum >= 5) 
            {
                comboDamage += 5;
                comboDamageUP = false;
            }
            else if(combo.comboNum < 5)
            {
                comboDamage = 0;
            }
        }
    }

    // 플레이어 재 피격가능까지 시간
    IEnumerator BulletHitCooldown(float damageCooldown)
    {
        hitBullet = false;
        yield return new WaitForSeconds(damageCooldown);
        hitBullet = true;
    }

    // 플레이어가 클릭중인 위치 (플레이어 드래그를 따라가는 몬스터용)
    public Vector3 GetLastClickPosition()
    {
        return lastClickPosition;
    }

    // 보스 스킬에 피격시
    IEnumerator StageBossHit()
    {
        isStageHit = false;      
        playerHealth -= 1;
        combo.comboNum = 0;
        Vector3 effectPos = new Vector3(transform.position.x, transform.position.y, transform.position.z - 6f);
        GameObject effect = Instantiate(healthEffect, effectPos, Quaternion.identity);
        yield return new WaitForSeconds(1f);
        isStageHit = true;
    }

    // 몬스터 공격시 몬스터 주위에 공격 이펙트 생성
    void AttackEffect(Vector3 targetPosition)
    {
        float xOffset = Random.Range(-0.5f, 0.5f);
        float yOffset = Random.Range(-0.2f, 0.2f);
        Vector3 spawnPosition = targetPosition + new Vector3(xOffset, yOffset, -5);

        GameObject monsterHit = Instantiate(attckEffect, spawnPosition, Quaternion.identity);

        StartCoroutine(RotateAndShrinkEffect(monsterHit.transform, 0.3f)); // 0.3초 후에 몬스터 히트 이펙트 제거
    }

    IEnumerator RotateAndShrinkEffect(Transform effectTransform, float destroyDelay)
    {
        float duration = 0.3f;
        float elapsedTime = 0f;

        Quaternion startRotation = effectTransform.rotation;
        Quaternion targetRotation = startRotation * Quaternion.Euler(0, 0, 180);

        Vector3 startScale = effectTransform.localScale;
        Vector3 targetScale = Vector3.zero;

        while (elapsedTime < duration)
        {
            effectTransform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime / duration);
            effectTransform.localScale = Vector3.Slerp(startScale, targetScale, elapsedTime / duration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        effectTransform.rotation = targetRotation;
        effectTransform.localScale = targetScale;

        yield return new WaitForSeconds(destroyDelay);
        Destroy(effectTransform.gameObject);
    }

    // 몬스터 공격
    IEnumerator AttackMonster(MonsterController monsterController)
    {
        isAttacking = true;

        if (monsterController.defense) // 몬스터가 방어중일때 공격 불가
        {
            isAttacking = false;
            yield break;
        }

        audioManager.AttackAudio();

        // 몬스터 공격 가능시 공격성공
        if (monsterController.playerTakeDamage)
        {
            AttackEffect(monsterController.gameObject.transform.position); // 공격 이펙트 생성
            monsterController.currentHealth -= damage + comboDamage; // 몬스터 체력 감소
            PlayerDamageText(monsterController); // 데미지 표시
        }

        itemSkill.SetCurrentAttackedMonster(monsterController.gameObject);

        // 플레이어 공격 속도 감소 (스테이지 5보스 스킬 - 공격 속도 감소)
        if (stage5Debuff)
        {
            monsterController.PlayerDamegeCoolDown(3f, 0.2f);
        }
        else
        {
            monsterController.PlayerDamegeCoolDown(0.2f, 0.2f);
        }

        // 공격시 확률적으로 아이템 발동
        if (itemSkill.isFire && Random.Range(0f, 100f) <= itemSkill.firePercent)
        {
            itemSkill.Fire(monsterController.gameObject.transform.position);
        }
        if (itemSkill.isFireShot && Random.Range(0f, 100f) <= itemSkill.fireShotPercent)
        {
            itemSkill.FireShot(monsterController.gameObject.transform.position);
            if (monsterController.fireShotTakeDamage)
            {
                FireDamageText(monsterController);
                monsterController.currentHealth -= itemSkill.fireShotDamage;
                monsterController.FireShotDamegeCoolDown(0.5f, 0.2f);
            }
        }
        if (itemSkill.isHolyWave && Random.Range(0f, 100f) <= itemSkill.holyWavePercent)
        {
            itemSkill.HolyWave();
        }
        if (itemSkill.isHolyShot && Random.Range(0f, 100f) <= itemSkill.holyShotPercent)
        {
            itemSkill.HolyShot(monsterController.gameObject.transform.position);
        }
        if (itemSkill.isMelee && Random.Range(0f, 100f) <= itemSkill.meleePercent)
        {
            itemSkill.Melee(monsterController.gameObject.transform.position, itemSkill.meleeNum);

            StartCoroutine(MeleeAttack());
        }
        IEnumerator MeleeAttack()
        {
            for (int i = 0; i < itemSkill.meleeNum; i++)
            {
                monsterController.currentHealth -= damage + comboDamage;

                PlayerDamageText(monsterController);

                yield return new WaitForSeconds(0.15f);
            }
        }

        if (itemSkill.isPosion && Random.Range(0f, 100f) <= itemSkill.posionPercent)
        {
            itemSkill.Posion(monsterController.gameObject.transform.position);
        }
        if (itemSkill.isRock && Random.Range(0f, 100f) <= itemSkill.rockPercent)
        {
            itemSkill.Rock(monsterController.gameObject.transform.position);
            if (monsterController.rockTakeDamage)
            {
                RockDamageText(monsterController);
                monsterController.currentHealth -= itemSkill.rockDamage;
                monsterController.RockDamegeCoolDown(0.5f, 0.2f);
            }
        }
        if (itemSkill.isSturn && Random.Range(0f, 100f) <= itemSkill.sturnPercent && monsterController.CompareTag("Monster"))
        {
            itemSkill.Sturn();
        }

        // 마우스를 때거나 0.2초뒤 공격가능
        float startTime = Time.time;
        yield return new WaitUntil(() => !Input.GetMouseButton(0) || Time.time - startTime > 0.2f);

        isAttacking = false; // 공격이 끝남
    }

    // 체력 관련
    void UpdateHealthUI()
    {
        for (int i = 0; i < playerHealthUI.Length; i++) // 나머지 체력 비활성화
        {
            playerHealthUI[i].SetActive(false);
        }

        if (playerHealth >= 0) // 체력 표시
        {
            playerHealthUI[playerHealth].SetActive(true);
        }

        // 사망
        if (playerHealth <= 0 && !die)
        {
            die = true;
            Handheld.Vibrate(); // 사망시 진동

            GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster"); // 맵의 몬스터 전체 삭제
            GameObject boss = GameObject.FindWithTag("Boss");
            foreach(GameObject monster in monsters)
            {
                Destroy(monster);
            }
            Destroy(boss);

            playerHealthUI[0].SetActive(true);
            Time.timeScale = 0f; // 게임 멈추기
            stageManager.Reward(); // 종료 결과
            isDragging = false; 
            dragEffect.SetActive(false);
            stageManager.gameStart = false; // 게임종료 상태로 변경
            gameover.SetActive(true); // 결과창 표시
        }
    }

    // 방어
    public void Defense()
    {
        if (defenseTime <= 0)
        {
            StartCoroutine(StartDefense());
        }
    }

    IEnumerator StartDefense()
    {
        defending = true;

        audioManager.DefenseAudio();

        defenseEffect.SetActive(true);
        defenseCoolTime.SetActive(true);
        defenseTime = 6;

        yield return new WaitForSeconds(3f);

        defending = false;
        defenseEffect.SetActive(false);
    }

    // 플레이어 공격 데미지 텍스트
    public void PlayerDamageText(MonsterController monsterController)
    {
        if(monsterController != null)
        {
            GameObject damegeText = Instantiate(hubDamageText, monsterController.transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
            damegeText.GetComponent<DamageText>().damege = (damage + comboDamage);
        }
    }

    public void FireDamageText(MonsterController monsterController)
    {
        if (monsterController != null)
        {
        GameObject damegeText = Instantiate(hubDamageText, monsterController.transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
        damegeText.GetComponent<DamageText>().damege = (int)itemSkill.fireDamage;
        }
    }

    public void FireShotDamageText(MonsterController monsterController)
    {
        if (monsterController != null)
        {
        GameObject damegeText = Instantiate(hubDamageText, monsterController.transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
        damegeText.GetComponent<DamageText>().damege = (int)itemSkill.fireShotDamage;
        }
    }
    public void FireShotSubDamageText(MonsterController monsterController)
    {
        if (monsterController != null)
        {
        GameObject damegeText = Instantiate(hubDamageText, monsterController.transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
        damegeText.GetComponent<DamageText>().damege = (int)itemSkill.fireShotSubDamage;
        }
    }

    public void HolyShotDamageText(MonsterController monsterController)
    {
        if (monsterController != null)
        {
        GameObject damegeText = Instantiate(hubDamageText, monsterController.transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
        damegeText.GetComponent<DamageText>().damege = (int)itemSkill.holyShotDamage;
        }
    }

    public void HolyWaveDamageText(MonsterController monsterController)
    {
        if (monsterController != null)
        {
        GameObject damegeText = Instantiate(hubDamageText, monsterController.transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
        damegeText.GetComponent<DamageText>().damege = (int)itemSkill.holyWaveDamage;
        }
    }
    public void RockDamageText(MonsterController monsterController)
    {
        if (monsterController != null)
        {
        GameObject damegeText = Instantiate(hubDamageText, monsterController.transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
        damegeText.GetComponent<DamageText>().damege = (int)itemSkill.rockDamage;
        }
    }
    public void PoisonDamageText(MonsterController monsterController)
    {
        if (monsterController != null)
        {
        GameObject damegeText = Instantiate(hubDamageText, monsterController.transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
        damegeText.GetComponent<DamageText>().damege = (int)itemSkill.poisonDamage;
        }
    }

    public void CWaterDamageText(MonsterController monsterController)
    {
        if (monsterController != null)
        {
        GameObject damegeText = Instantiate(hubDamageText, monsterController.transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
        damegeText.GetComponent<DamageText>().damege = (int)charaterSkill.waterDamage;
        }
    }
    public void CRockDamageText(MonsterController monsterController)
    {
        if (monsterController != null)
        {
        GameObject damegeText = Instantiate(hubDamageText, monsterController.transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
        damegeText.GetComponent<DamageText>().damege = (int)charaterSkill.rockDamage;
        }
    }
}
