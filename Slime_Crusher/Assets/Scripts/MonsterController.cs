using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    private MonsterSpwan monsterSpawn;
    private PlayerController playerController;
    private ItemSkill itemSkill;
    private AudioManager audioManager;
    private StageManager stageManager;
    private Combo combo;

    public int damage;
    public float maxHealth;
    public float currentHealth;

    // 피격 간격
    public bool monsterTakeDamage = true;
    public bool playerTakeDamage = true; // 기본공격

    // 아이템 피격
    public bool fireTakeDamage = true; // 파이어
    public bool fireShotTakeDamage = true; // 파이어샷
    public bool fireShotSubTakeDamage = true; // 파이어샷 서브
    public bool holyWaveTakeDamage = true; // 홀리웨이브
    public bool holyShotTakeDamage = true; // 홀리샷
    public bool rockTakeDamage = true; // 돌
    public bool poisonTakeDamage = true; // 독

    // 캐릭터 스킬 피격
    public bool pRockTakeDamage = true; // 돌
    public bool pWaterTakeDamage = true; // 물

    private int bossAttackNum;
    public float attackTime;
    public float selectedAttackTime;

    // 몬스터 상태 관련
    public GameObject danager;
    public GameObject dieEffect;
    public GameObject sturn;
    private bool isMonsterAttacking = false;
    private bool isBossAttacking = false;
    private bool isDie = false;

    // 플레이어 기술 관련
    public bool fired; // 불
    public bool stop; // 기절중인지
    public bool poisoned; // 중독   

    private int monsterLayer;
    private int bossLayer;

    // 보스 스킬 관련
    public bool boss1Defending = false;

    // 몬스터 스킬 관련
    public bool defense = false;
    public bool moved = false;

    // 몬스터 사망시 아이템 생성
    public GameObject healthUpItem;
    private bool itemSpawn = false;

    SpriteRenderer spriteRenderer;
    private Animator anim;

    private void Awake()
    {
        monsterSpawn = GameObject.Find("Manager").GetComponent<MonsterSpwan>();
        playerController = GameObject.Find("Manager").GetComponent<PlayerController>();
        itemSkill = GameObject.Find("Manager").GetComponent<ItemSkill>();
        audioManager = GameObject.Find("Manager").GetComponent<AudioManager>();
        stageManager = GameObject.Find("Manager").GetComponent<StageManager>();
        combo = GameObject.Find("Manager").GetComponent<Combo>();
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        danager.SetActive(false);
        stop = false;

        bossAttackNum = 1; // 보스 연속 공격 방지용 (1 = 평소, 0 = 공격)

        monsterLayer = LayerMask.NameToLayer("Monster");
        bossLayer = LayerMask.NameToLayer("Boss");

        // 스테이지 몬스터 체력관리
        if (stageManager.mainStage > 8)
        {
            maxHealth = maxHealth + ((stageManager.mainStage - 8) * 15);

            if (gameObject.name == "6(Clone)")
            {
                maxHealth = maxHealth + ((stageManager.mainStage - 8) * 25);
            }
            else if (gameObject.name == "6-2(Clone)")
            {
                GameObject monster = GameObject.Find("6(Clone)");

                if(monster != null)
                {
                    MonsterController monsterController = monster.GetComponent<MonsterController>();
                    maxHealth = monsterController.currentHealth / 2;
                }
            }
        }
        currentHealth = maxHealth;
    }

    void Update()
    {
        // 몬스터 사망시 상태 및 오브젝트 제거
        if (stageManager.mainStage == 1 && stageManager.subStage == 5)
        {
            GameObject boss = GameObject.Find("4(Clone)");
            if (boss == null)
            {
                boss1Defending = false;
            }
        }
        else if (stageManager.mainStage >= 8)
        {
            GameObject boss = GameObject.Find("6(Clone)");
            if (boss == null)
            {
                boss1Defending = false;
            }
        }
        if (stageManager.mainStage == 7 && stageManager.subStage == 5)
        {
            GameObject boss = GameObject.Find("4(Clone)");
            GameObject[] bossSkill = GameObject.FindObjectsOfType<GameObject>();
            if (boss == null)
            {
                if (bossSkill != null)
                {
                    foreach (GameObject skill in bossSkill)
                    {
                        if (skill.name == "BossSkill")
                        {
                            Destroy(skill);
                        }
                    }
                }
            }
        }
        else if (stageManager.mainStage >= 8)
        {
            GameObject boss = GameObject.Find("6(Clone)");
            GameObject[] bossSkill = GameObject.FindObjectsOfType<GameObject>();
            if (boss == null)
            {
                if (bossSkill != null)
                {
                    foreach (GameObject skill in bossSkill)
                    {
                        if (skill.name == "BossSkill")
                        {
                            Destroy(skill);
                        }
                    }
                }
            }
        }
        
        // 사망
        if (currentHealth <= 0)
        {
            Die();
        }

        // 기절시 멈추도록
        if (stop)
        {
            anim.enabled = false;
        }
        else
        {
            anim.enabled = true;
        }
        
        // 몬스터 공격
        if (attackTime <= 0)
        {
            if (gameObject.tag == "Monster")
            {             
                StartCoroutine(MonsterAttackReady());
            }
            else if(gameObject.tag == "Boss" && bossAttackNum == 1)
            {
                StartCoroutine(BossAttackReady());                           
            }
        }
        else
        {
            attackTime -= Time.deltaTime;
        }

        // 플레이어 아이템 발동시 데미지
        if (itemSkill.holyWave && playerTakeDamage)
        {
            if (holyWaveTakeDamage)
            {
                if (boss1Defending)
                {
                    playerController.HolyWaveDamageText(this);

                    currentHealth -= itemSkill.holyWaveDamage * 0.5f;
                    StartCoroutine(HolyWaveDamageCooldown(0.7f, 0.2f));
                }
                else
                {
                    playerController.HolyWaveDamageText(this);

                    currentHealth -= itemSkill.holyWaveDamage;
                    StartCoroutine(HolyWaveDamageCooldown(0.7f, 0.2f));
                }           
            }
        }

        if (poisoned)
        {
            if (poisonTakeDamage)
            {
                if (itemSkill.posionDuration >= 0)
                {
                    if (boss1Defending)
                    {
                        playerController.PoisonDamageText(this);

                        currentHealth -= itemSkill.poisonDamage * 0.5f;
                        StartCoroutine(PoisonDamageCooldown(0.5f, 0.2f));
                    }
                    else
                    {
                        playerController.PoisonDamageText(this);

                        currentHealth -= itemSkill.poisonDamage;
                        StartCoroutine(PoisonDamageCooldown(0.5f, 0.2f));
                    }
                    
                }
                else
                {
                    poisoned = false;
                }
            }
        }

        if (fired)
        {
            if (fireTakeDamage)
            {
                if (boss1Defending)
                {
                    playerController.FireDamageText(this);

                    currentHealth -= itemSkill.fireDamage * 0.5f;
                    StartCoroutine(FireDamageCooldown(0.3f, 0.2f));
                    StartCoroutine(DeleteFire());
                }
                else
                {
                    playerController.FireDamageText(this);

                    currentHealth -= itemSkill.fireDamage;
                    StartCoroutine(FireDamageCooldown(0.3f, 0.2f));
                    StartCoroutine(DeleteFire());
                }
            }
        }
    }

    // 기절종료
    IEnumerator Removestun()
    {
        yield return new WaitForSeconds(3f);

        stop = false;
    }

    IEnumerator DeleteFire()
    {
        yield return new WaitForSeconds(3f);

        fired = false;
    }

    // 보스 공격 준비
    IEnumerator BossAttackReady()
    {
        if (!isBossAttacking)
        {
            isBossAttacking = true;
            danager.SetActive(true);
            yield return new WaitForSeconds(1.0f);
            danager.SetActive(false);

            yield return StartCoroutine(BossAttack());

            isBossAttacking = false;
        }
    }

    // 보스 공격
    IEnumerator BossAttack()
    {
        danager.SetActive(true);
        yield return new WaitForSeconds(0.5f);

        anim.SetBool("Attack", true);

        if (bossAttackNum != 0)
        {
            if (playerController.defending)
            {

            }
            else
            {
                bossAttackNum = 0;
                playerController.playerHealth -= damage;
                combo.comboNum = 0;
            }         
        }

        yield return new WaitForSeconds(1f);
       // attack = false;
        anim.SetBool("Attack", false);
        danager.SetActive(false);

        attackTime = Random.Range(8.0f, 10.0f);
        StartCoroutine(ReadyBossAttack());
    }

    // 보스 상태 초기화
    IEnumerator ReadyBossAttack()
    {
        yield return new WaitForSeconds(attackTime);
        bossAttackNum = 1;
    }

    // 몬스터 공격 준비
    IEnumerator MonsterAttackReady()
    {
        if (!isMonsterAttacking)
        {
            isMonsterAttacking = true;
            danager.SetActive(true);
            yield return new WaitForSeconds(1.0f);

            yield return StartCoroutine(MonsterAttack());

            isMonsterAttacking = false;
        }
    }

    // 몬스터 공격
    IEnumerator MonsterAttack()
    {
        anim.SetBool("Attack", true);
        danager.SetActive(false);

        yield return new WaitForSeconds(0.2f);

        // 각 몬스터당 공격 사용
        MonoBehaviour[] scripts = GetComponents<MonoBehaviour>();

        MonoBehaviour stageScript = null;
        foreach (MonoBehaviour script in scripts)
        {
            if (script.GetType().Name.Contains("Stage"))
            {
                stageScript = script;
                break;
            }
        }
        if (stageScript != null)
        {
            System.Reflection.MethodInfo method = stageScript.GetType().GetMethod("Attack");
            if (method != null)
            {
                method.Invoke(stageScript, null);
            }
        }

        yield return new WaitForSeconds(1f);

        anim.SetBool("Attack", false);
        danager.SetActive(false);

        attackTime = selectedAttackTime;
    }

    // 공격 받을시 색 변경 (피격 표시)
    IEnumerator BackColor(float time)
    {
        yield return new WaitForSeconds(time);
        spriteRenderer.color = Color.white;
    }

    // 피격 간격 관리
    public void PlayerDamegeCoolDown(float damageCooldown, float colorChangeTime) // 다른 스크립트용
    { 
        StartCoroutine(PlayerDamageCooldown(damageCooldown, colorChangeTime));
    }
    public void FireDamegeCoolDown(float damageCooldown, float colorChangeTime)
    {
        StartCoroutine(FireDamageCooldown(damageCooldown, colorChangeTime));
    }
    public void FireShotDamegeCoolDown(float damageCooldown, float colorChangeTime)
    {
        StartCoroutine(FireShotDamageCooldown(damageCooldown, colorChangeTime));
    }
    public void FireShotSubDamegeCoolDown(float damageCooldown, float colorChangeTime)
    {
        StartCoroutine(FireShotSubDamageCooldown(damageCooldown, colorChangeTime));
    }
    public void HolyWaveDamegeCoolDown(float damageCooldown, float colorChangeTime)
    {
        StartCoroutine(HolyWaveDamageCooldown(damageCooldown, colorChangeTime));
    }
    public void HolyShotDamegeCoolDown(float damageCooldown, float colorChangeTime)
    {
        StartCoroutine(HolyShotDamageCooldown(damageCooldown, colorChangeTime));
    }
    public void RockDamegeCoolDown(float damageCooldown, float colorChangeTime)
    {
        StartCoroutine(RockDamageCooldown(damageCooldown, colorChangeTime));
    }
    public void PoisonDamegeCoolDown(float damageCooldown, float colorChangeTime)
    {
        StartCoroutine(PoisonDamageCooldown(damageCooldown, colorChangeTime));
    }
    
    public void PlayerRockDamegeCoolDown(float damageCooldown, float colorChangeTime)
    {
        StartCoroutine(PlayerRockDamageCooldown(damageCooldown, colorChangeTime));
    }
    public void PlayerWaterDamegeCoolDown(float damageCooldown, float colorChangeTime)
    {
        StartCoroutine(PlayerWaterDamageCooldown(damageCooldown, colorChangeTime));
    }

    IEnumerator MonsterDamageCooldown(float damageCooldown)
    {
        monsterTakeDamage = false; 
        yield return new WaitForSeconds(damageCooldown);
        monsterTakeDamage = true;
    }

    IEnumerator PlayerDamageCooldown(float damageCooldown, float colorChangeTime)
    {
        spriteRenderer.color = Color.red;
        StartCoroutine(BackColor(colorChangeTime));

        playerTakeDamage = false;
        yield return new WaitForSeconds(damageCooldown);
        playerTakeDamage = true;
    }
    IEnumerator FireDamageCooldown(float damageCooldown, float colorChangeTime)
    {
        spriteRenderer.color = new Color(1f, 0.5f, 0, 1);
        StartCoroutine(BackColor(colorChangeTime));

        fireTakeDamage = false;
        yield return new WaitForSeconds(damageCooldown);
        fireTakeDamage = true;
    }
    IEnumerator FireShotDamageCooldown(float damageCooldown, float colorChangeTime)
    {
        spriteRenderer.color = Color.red;
        StartCoroutine(BackColor(colorChangeTime));

        fireShotTakeDamage = false;
        yield return new WaitForSeconds(damageCooldown);
        fireShotTakeDamage = true;
    }
    IEnumerator FireShotSubDamageCooldown(float damageCooldown, float colorChangeTime)
    {
        spriteRenderer.color = Color.red;
        StartCoroutine(BackColor(colorChangeTime));

        fireShotSubTakeDamage = false;
        yield return new WaitForSeconds(damageCooldown);
        fireShotSubTakeDamage = true;
    }
    IEnumerator HolyWaveDamageCooldown(float damageCooldown, float colorChangeTime)
    {
        spriteRenderer.color = Color.red;
        StartCoroutine(BackColor(colorChangeTime));

        holyWaveTakeDamage = false;
        yield return new WaitForSeconds(damageCooldown);
        holyWaveTakeDamage = true;
    }
    IEnumerator HolyShotDamageCooldown(float damageCooldown, float colorChangeTime)
    {
        spriteRenderer.color = Color.red;
        StartCoroutine(BackColor(colorChangeTime));

        holyShotTakeDamage = false;
        yield return new WaitForSeconds(damageCooldown);
        holyShotTakeDamage = true;
    }
    IEnumerator RockDamageCooldown(float damageCooldown, float colorChangeTime)
    {
        spriteRenderer.color = Color.red;
        StartCoroutine(BackColor(colorChangeTime));

        rockTakeDamage = false;
        yield return new WaitForSeconds(damageCooldown);
        rockTakeDamage = true;
    }
    IEnumerator PoisonDamageCooldown(float damageCooldown, float colorChangeTime)
    {
        spriteRenderer.color = new Color(0.7f, 0, 0.7f, 1);
        StartCoroutine(BackColor(colorChangeTime));

        poisonTakeDamage = false;
        yield return new WaitForSeconds(damageCooldown);
        poisonTakeDamage = true;
    }
    IEnumerator PlayerRockDamageCooldown(float damageCooldown, float colorChangeTime)
    {
        spriteRenderer.color = Color.red;
        StartCoroutine(BackColor(colorChangeTime));

        pRockTakeDamage = false;
        yield return new WaitForSeconds(damageCooldown);
        pRockTakeDamage = true;
    }
    IEnumerator PlayerWaterDamageCooldown(float damageCooldown, float colorChangeTime)
    {
        spriteRenderer.color = Color.red;
        StartCoroutine(BackColor(colorChangeTime));

        pWaterTakeDamage = false;
        yield return new WaitForSeconds(damageCooldown);
        pWaterTakeDamage = true;
    }

    // 사망
    public void Die()
    {
        StartCoroutine(MonsterDie());
    }

    IEnumerator MonsterDie()
    {
        // 사망시 확률적으로 회복 아이템 생성
        if (gameObject.CompareTag("Monster"))
        {
            if (!itemSpawn)
            {
                itemSpawn = true;

                if (Random.Range(0f, 100f) <= 5)
                {
                    GameObject item = Instantiate(healthUpItem, gameObject.transform.position, Quaternion.identity);
                    item.name = "HealthUpItem";
                }               
            }
        }

        // 사망시 콤보수 증가
        if (!isDie)
        {
            isDie = true;
            
            combo.comboNum++;
            combo.ComboUp();

            if(combo.comboNum % 5 == 0)
            {
                playerController.comboDamageUP = true;
            }
        }

        // 사망시 사망 이펙트 생성
        if (dieEffect != null)
        {
            SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
            renderer.enabled = false;

            dieEffect.SetActive(true);

            yield return new WaitForSeconds(1f);
        }

        // 사망시 총 소환 몬스터 리스트에서 제거
        monsterSpawn.RemoveMonsterFromList(gameObject);

        // 기절 관련
        if (stop)
        {
            itemSkill.DestroyMonster(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // 플레이어 아이템에 피격
        if (collision.gameObject.tag == "HolyShot")
        {
            if (holyShotTakeDamage)
            {
                if (boss1Defending)
                {
                    playerController.HolyShotDamageText(this);
                    currentHealth -= itemSkill.holyShotDamage * 0.5f;
                    StartCoroutine(HolyShotDamageCooldown(0.5f, 0.2f));
                }
                else
                {
                    playerController.HolyShotDamageText(this);
                    currentHealth -= itemSkill.holyShotDamage;
                    StartCoroutine(HolyShotDamageCooldown(0.5f, 0.2f));
                }
            }
        }
        else if (collision.gameObject.tag == "Fire")
        {
            fired = true;
        }
        else if (collision.gameObject.tag == "Poison")
        {
            poisoned = true;
        }
        else if (collision.gameObject.tag == "FireShotSub")
        {
            if (fireShotSubTakeDamage)
            {
                if (boss1Defending)
                {
                    playerController.FireShotSubDamageText(this);
                    currentHealth -= itemSkill.fireShotSubDamage * 0.5f;
                    StartCoroutine(FireShotSubDamageCooldown(0.5f, 0.2f));
                }
                else
                {
                    playerController.FireShotSubDamageText(this);
                    currentHealth -= itemSkill.fireShotSubDamage;
                    StartCoroutine(FireShotSubDamageCooldown(0.5f, 0.2f));
                }
            }
        }
        // 몬스터 위치 관리
        else if ((collision.gameObject.tag == "Monster" || collision.gameObject.tag == "Boss") && !isColliding)
        {
            StartCoroutine(MoveWithRandomDirection()); // 유닛 겹치지 않도록
        }
        else if (collision.gameObject.tag == "Wall")
        {
            transform.position = Vector3.zero;
        }
    }
    private bool isColliding = false;

    // 몬스터가 겹칠시 랜덤한 위치로 이동
    private IEnumerator MoveWithRandomDirection()
    {
        isColliding = true;

        Rigidbody2D playerRb = GetComponent<Rigidbody2D>();

        if (playerRb != null)
        {
            float randomAngle = Random.Range(0f, 360f);
            float randomAngleInRadians = Mathf.Deg2Rad * randomAngle;
            Vector2 randomDirection = new Vector2(Mathf.Cos(randomAngleInRadians), Mathf.Sin(randomAngleInRadians));

            playerRb.velocity = randomDirection * 3f;

            yield return new WaitForSeconds(1f);

            if (isColliding)
            {
                StartCoroutine(MoveWithRandomDirection());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // 겹치지 않을시 멈춤
        if (collision.gameObject.tag == "Monster")
        {
            isColliding = false;
            Rigidbody2D playerRb = GetComponent<Rigidbody2D>();

            if (playerRb != null)
            {
                playerRb.velocity = Vector2.zero;
            }
        }
    }
}
