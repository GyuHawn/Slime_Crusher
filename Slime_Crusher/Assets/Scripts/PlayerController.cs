using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Burst.CompilerServices;

public class PlayerController : MonoBehaviour
{
    private StageManager stageManager;
    private ItemSkill itemSkill;
    private CharacterSkill characterSkill;
    private Combo combo;

    // 플레이어 체력 관련
    public GameObject[] playerHealthUI; // 체력 표시
    public GameObject hitEffect; // 피격 표시 이펙트
    public int playerHealth; // 현재 체력
    public GameObject gameover; // 게임 종료 표시
    public bool die; // 사망 하였는지

    // 플레이어 공격 관련
    public int damage; // 데미지
    public int comboDamage; // 콤보로 증가한 데미지
    public bool comboDamageUP; // 데미지 무한 증가 방지 변수
    public bool isAttacking = false; // 공격 속도 (다음 공격까지 시간을 주기위함)
    public bool playerHit = true; // 몬스터에게 공격 받았는지
    public GameObject hubDamageText; // 데미지 시각적 표시
    public GameObject attckEffect; // 공격 이펙트
    public GameObject dragEffect;

    public Vector3 lastClickPosition; // 마지막 클릭위치
    public bool isDragging = false; // 드래그 중인지

    // 플레이어 방어 관련
    public GameObject defenseEffect; // 방어 이펙트
    public bool defending; // 방어 중인지
    public float defenseTime; // 방어 쿨타임
    public GameObject defenseCoolTime; // 쿨타임 표시
    public TMP_Text defenseCoolTimeText;

    // 돈
    public int money;

    // 스테이지 관련
    public bool stage5Debuff = false; //보스 스킬 관련
    public bool isStageHit = true; 

    private void Awake()
    {
        if (!stageManager)
            stageManager = FindObjectOfType<StageManager>();
        if (!itemSkill)
            itemSkill = FindObjectOfType<ItemSkill>();
        if (!characterSkill)
            characterSkill = FindObjectOfType<CharacterSkill>();
        if (!combo)
            combo = FindObjectOfType<Combo>();
    }

    void Start()
    {
        playerHealth = 8; // 체력 설정
        damage = 10; // 데미지 설정  

        die = false; // 사망 여부 초기화
        comboDamageUP = false; // 데미지 증가 여부 초기화
        defending = false; // 방어 중 여부 초기화    

        lastClickPosition = Vector3.zero; // 클릭 포지션 초기화

        UpdateHealth(); // 플레이어 체력관리
    }

    public void UpdateHealth() // 플레이어 체력관리
    {
        for (int i = 0; i < playerHealthUI.Length; i++)
        {
            playerHealthUI[i].SetActive(i == playerHealth);
        }
    }

    void Update()
    {
        // 사망
        if (playerHealth <= 0 && !die)
        {
            Die();
        }

        DefenseCoolTime(); // 방어 쿨타임 관련        
        PlayerAttack(); // 플레이어 공격관련
        ComboDamageUp(); // 콤보 증가시 데미지 증가
    }

    void Die() // 사망
    {
        die = true;
        Handheld.Vibrate(); // 사망시 진동

        // 모든 몬스터 삭제
        foreach(GameObject monster in GameObject.FindGameObjectsWithTag("Monster"))
        {
            Destroy(monster);
        }
        Destroy(GameObject.FindWithTag("Boss"));

        playerHealthUI[0].SetActive(true);
        Time.timeScale = 0f; // 게임 멈추기
        stageManager.Reward(); // 종료 결과
        isDragging = false;
        dragEffect.SetActive(false);
        stageManager.gameStart = false; // 게임종료 상태로 변경
        gameover.SetActive(true); // 결과창 표시
    }

    void DefenseCoolTime() // 방어 쿨타임 관련
    {      
        if (defenseTime > 0)
        {
            defenseCoolTimeText.text = ((int)defenseTime).ToString();
            defenseTime -= Time.deltaTime;
        }
        else
        {
            defenseCoolTime.SetActive(false);
        }
    }

    void OnDrag() // 공격 시작 여부
    {
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
    }
    void AttackEffect()
    {
        if (isDragging)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            dragEffect.transform.position = new Vector3(mousePosition.x, mousePosition.y, dragEffect.transform.position.z);
        }
    }

    void PlayerAttack() // 플레이어 공격관련
    {
         OnDrag(); // 공격 시작 여부
         AttackEffect(); // 공격 이펙트


        // 공격 관련
        if (isDragging && !isAttacking)
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

            lastClickPosition = worldPoint;

            // 공격 시
            if (hit.collider != null)
            {
                var monsterController = hit.collider.GetComponent<MonsterController>();

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
                    if (playerHealth < 8)
                    {
                        playerHealth++;
                    }
                    else
                    {
                        Destroy(hit.collider.gameObject);
                    }
                }

                // 몬스터 공격에 닿을시
                if (hit.collider.CompareTag("Bullet") && playerHit)
                {
                    PlayerHit(); // (몬스터 총알) 플레이어 피격
                }

                // 몬스터 공격시
                if (monsterController != null)
                {
                    PlayerEtcHit(monsterController); // (몬스터 총알 제외) 플레이어 피격
                }
            }
        }
    }

    void PlayerHit() // 플레이어 피격
    {
        AudioManager.Instance.PlayHitAudio();

        playerHealth --; // 체력 감소
        stageManager.comboNum = 0; // 콤보 초기화
        UpdateHealth(); // 플레이어 피격

        Instantiate(hitEffect, transform.position, Quaternion.identity);
        StartCoroutine(BulletHitCooldown(0.2f)); // 재 피격가능까지 시간
    }
    void PlayerEtcHit(MonsterController monsterController) // (몬스터 총알 제외) 플레이어 피격
    {
        if (monsterController.boss1Defending) { } // 1스테이지 보스 스킬중 몬스터 공격 불가
        else if (monsterController.moved && playerHit) // 이동관련 공격 몬스터
        {
            PlayerHit();
        }
        else
        {
            StartCoroutine(AttackMonster(monsterController)); // 몬스터 공격 성공
        }
    }


    void ComboDamageUp()// 콤보 증가시 데미지 증가
    {      
        if (comboDamageUP)
        {
            if (stageManager.comboNum >= 5)
            {
                comboDamage += 5;
                comboDamageUP = false;
            }
            else if (stageManager.comboNum < 5)
            {
                comboDamage = 0;
            }
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

        AudioManager.Instance.PlayDefenseAudio();

        defenseEffect.SetActive(true);
        defenseCoolTime.SetActive(true);
        defenseTime = 6;

        yield return new WaitForSeconds(3f);

        defending = false;
        defenseEffect.SetActive(false);
    }


    // 플레이어 재 피격가능까지 시간
    IEnumerator BulletHitCooldown(float damageCooldown)
    {
        playerHit = false;
        yield return new WaitForSeconds(damageCooldown);
        playerHit = true;
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
        stageManager.comboNum = 0;
        UpdateHealth(); // 플레이어 피격

        Vector3 effectPos = new Vector3(transform.position.x, transform.position.y, transform.position.z - 6f);
        Instantiate(hitEffect, effectPos, Quaternion.identity);
        yield return new WaitForSeconds(1f);
        isStageHit = true;
    }

    // 몬스터 공격시 몬스터 주위에 공격 이펙트 생성
    void AttackEffect(Vector3 targetPosition)
    {
        var spawnPosition = targetPosition + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.2f, 0.2f), -5);
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
    }

    // 몬스터 공격
    IEnumerator AttackMonster(MonsterController monsterController)
    {
        isAttacking = true;
        AudioManager.Instance.PlayAttackAudio(); // 공격 오디오
        itemSkill.SetCurrentAttackedMonster(monsterController.gameObject); // 현재 공격받는 몬스터 설정

        if (monsterController.defense) // 몬스터가 방어중일때 공격 불가
        {
            isAttacking = false;
            yield break;
        }

        // 몬스터 공격 가능시 공격성공
        if (monsterController.playerTakeDamage)
        {
            AttackEffect(monsterController.gameObject.transform.position); // 공격 이펙트 생성
            monsterController.currentHealth -= damage + comboDamage; // 몬스터 체력 감소
            PlayerDamageText(monsterController); // 데미지 표시
        }

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

        
        StartCoroutine(AttackDelayTime()); // 공격 지연시간
    }

    IEnumerator AttackDelayTime() // 공격지연시간 (마우스를 때거나 0.2초뒤 공격가능)
    {
        float startTime = Time.time;
        yield return new WaitUntil(() => !Input.GetMouseButton(0) || Time.time - startTime > 0.2f);

        isAttacking = false; // 공격이 끝남
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
        damegeText.GetComponent<DamageText>().damege = (int)characterSkill.waterDamage;
        }
    }
    public void CRockDamageText(MonsterController monsterController)
    {
        if (monsterController != null)
        {
        GameObject damegeText = Instantiate(hubDamageText, monsterController.transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
        damegeText.GetComponent<DamageText>().damege = (int)characterSkill.rockDamage;
        }
    }
}
