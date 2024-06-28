using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StageStatus : MonoBehaviour
{
    private StageTimeLimit stageTimeLimit;
    private PlayerController playerController;
    private ItemSkill itemSkill;
    private StageManager stageManager;

    public int buff; // 버프, 디버프 구분
    public int status; // 버프 종류 선택
    public Image stageStatus; 
    public GameObject statusPos;
    public int saveDamage; // 공격력 관련 버프 데미지

    // 버프
    public GameObject damageUp; // 기본데미지 증가
    public bool isDamageUp; // 기본데미지 증가
    public GameObject monsterHealthDown; // 몬스터 체력 감소
    public bool isMonsterHealthDown; // 몬스터 체력 감소
    public GameObject timeUp; // 제한시간 증가
    public bool isTimeUp; // 제한시간 증가
    public GameObject percentUp; // 확률 증가
    public bool isPercentUp; // 확률 증가
    public GameObject monsterDie; // 일정시간 마다 몬스터 제거
    private float timer = 0f;
    private float delay = 10f;
    private List<GameObject> buffList = new List<GameObject>(); // 버프 리스트

    // 디버프
    public GameObject damageDown; // 기본데미지 감소
    public bool isDamageDown; // 기본데미지 감소
    public GameObject monsterHealthUP; // 몬스터 체력 증가
    public bool isMonsterHealthUP; // 몬스터 체력 증가
    public GameObject timeDown; // 제한시간 감소
    public bool isTimeDown; // 제한시간 감소
    public GameObject percentDown; // 확률감소
    public bool isPercentDown; // 확률감소
    public GameObject monsterAttackSpdUp; // 투사체 속도 증가
    public bool isMonsterAttackSpdUp; // 투사체 속도 증가
    public GameObject monsterSizeDown;// 몬스터 크기 감소
    public bool isMonsterSizeDown;// 몬스터 크기 감소
    private List<GameObject> deBuffList = new List<GameObject>(); // 디버프 리스트

    private GameObject selectedEffect; // 선택된 버프
    public TMP_Text buffText;

    private void Awake()
    {
        stageTimeLimit = GameObject.Find("Manager").GetComponent<StageTimeLimit>();
        playerController = GameObject.Find("Manager").GetComponent<PlayerController>();
        itemSkill = GameObject.Find("Manager").GetComponent<ItemSkill>();
        stageManager = GameObject.Find("Manager").GetComponent<StageManager>();
    }

    void Start()
    {
        // 리스트에 추가
        // 버프
        buffList.Add(damageUp);
        buffList.Add(monsterHealthDown);
        buffList.Add(timeUp);
        buffList.Add(percentUp);
        buffList.Add(monsterDie);

        // 디버프
        deBuffList.Add(damageDown);
        deBuffList.Add(monsterHealthUP);
        deBuffList.Add(timeDown);
        deBuffList.Add(percentDown);
        deBuffList.Add(monsterAttackSpdUp);
        deBuffList.Add(monsterSizeDown);

        // 버프 적용않함
        isDamageUp = false;
        isMonsterHealthDown = false;
        isTimeUp = false;
        isPercentUp = false;
        isDamageDown = false;
        isMonsterHealthUP = false;
        isTimeDown = false;
        isPercentDown = false;
        isMonsterAttackSpdUp = false;
        isMonsterSizeDown = false;
    }

    void Update()
    {
        // 버프 적용
        if (buff == 1)
        {
            if (status == 1)
            {
                buffText.text = "플레이어의 기본 공격력이 증가합니다.";
                if (!isDamageUp)
                {
                    DamageUP();
                }
            }
            else if (status == 2)
            {
                buffText.text = "몬스터의 기본 체력이 감소합니다.";
                if (!isMonsterHealthDown)
                {
                    MonsterHealthDown();
                }
            }
            else if (status == 3)
            {
                 buffText.text = "스테이지 제한 시간이 증가합니다.";
                if (!isTimeUp)
                {
                    TimeUp();
                }
            }
            else if (status == 4)
            {
                buffText.text = "아이템 발동 확률이 증가합니다.";
                if (!isPercentUp)
                {
                    PercentUp();
                }
            }
            else if (status == 5)
            {
                buffText.text = "일정 시간마다 몬스터가 사망합니다.";
                timer += Time.deltaTime;

                if (timer >= delay)
                {
                    MonsterDie();

                    timer = 0f;
                }
            }
        }
        else if (buff == 2) // 디버프 적용
        {
            if (status == 1)
            {
                buffText.text = "플레이어의 기본 공격력이 감소합니다.";
                if (!isDamageDown)
                {
                    DamageDown();
                }
            }
            else if (status == 2)
            {
                buffText.text = "몬스터의 기본 체력이 증가합니다.";
                if (!isMonsterHealthUP)
                {
                    MonsterHealthUP();
                }
            }
            else if (status == 3)
            {
                buffText.text = "스테이지 제한 시간이 감소합니다.";
                if (!isTimeDown)
                {
                    TimeDown();
                }
            }
            else if (status == 4)
            {
                buffText.text = "아이템 발동 확률이 감소합니다.";
                if (!isPercentDown)
                {
                    PercentDown();
                }
            }
            else if (status == 5)
            {
                buffText.text = "투사체 속도가 증가합니다.";
                if (!isMonsterAttackSpdUp)
                {
                    MonsterAttackSpdUp();
                }
            }
            else if (status == 6)
            {
                buffText.text = "몬스터의 사이즈가 감소합니다.";
                if (!isMonsterSizeDown)
                {
                    MonsterSizeDown();
                }
            }
        }
    }

    // 버프
    // 기본데미지증가
    public void DamageUP()
    {
        isDamageUp = true;
        saveDamage = playerController.damage;
        playerController.damage += (int)(playerController.damage * 0.5f);
    }
    // 기본데미지증가 리셋
    public void ResetDamageUP()
    {
        isDamageUp = true;
        playerController.damage = saveDamage;
    }

    // 몬스터 체력 감소
    public void MonsterHealthDown()
    {
        isMonsterHealthDown = true;
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");

        foreach(GameObject monster in monsters)
        {
            MonsterController monsterController = monster.GetComponent<MonsterController>();
            monsterController.currentHealth -= (monsterController.currentHealth * 0.3f);
        }
    }

    // 제한시간 증가
    public void TimeUp()
    {
        isTimeUp = true;
        stageTimeLimit.stageTime += 10;
    }
    // 제한시간 증가 리셋
    public void ResetTimeUp()
    {
        isTimeUp = true;
        stageTimeLimit.stageTime -= 10;
    }

    // 확률 증가
    public void PercentUp()
    {
        isPercentUp = true;

        itemSkill.firePercent += 5f;
        itemSkill.fireShotPercent += 5f;
        itemSkill.holyShotPercent += 5f;
        itemSkill.holyWavePercent += 5f;
        itemSkill.rockPercent += 5f;
        itemSkill.posionPercent += 5f;
        itemSkill.meleePercent += 5f;
        itemSkill.sturnPercent += 5f;
    }
    // 확률 증가 리셋
    public void ResetPercentUp()
    {
        isPercentUp = true;

        itemSkill.firePercent -= 5f;
        itemSkill.fireShotPercent -= 5f;
        itemSkill.holyShotPercent -= 5f;
        itemSkill.holyWavePercent -= 5f;
        itemSkill.rockPercent -= 5f;
        itemSkill.posionPercent -= 5f;
        itemSkill.meleePercent -= 5f;
        itemSkill.sturnPercent -= 5f;
    }

    // 일정시간 마다 몬스터 제거
    public void MonsterDie()
    {
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");

        foreach (GameObject monster in monsters)
        {
            MonsterController monsterController = monster.GetComponent<MonsterController>();

            if (monsterController != null && monsterController.currentHealth > 0)
            {
                monsterController.currentHealth = 0;
                break;
            }
        }
    }

    // 디버프
    // 기본데미지 감소
    public void DamageDown()
    {
        isDamageDown = true;
        saveDamage = playerController.damage;
        playerController.damage -= (int)(playerController.damage * 0.5f);
    }
    // 기본데미지 감소 리셋
    public void ResetDamageDown()
    {
        isDamageDown = true;
        playerController.damage = saveDamage;
    }

    // 몬스터 체력 증가
    public void MonsterHealthUP()
    {
        isMonsterHealthUP = true;
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");

        foreach (GameObject monster in monsters)
        {
            MonsterController monsterController = monster.GetComponent<MonsterController>();
            monsterController.currentHealth = (monsterController.currentHealth * 1.5f);
        }
    }

    // 제한시간 감소
    public void TimeDown()
    {
        isTimeDown = true;
        stageTimeLimit.stageTime -= 10;
    }
    // 제한시간 감소 리셋
    public void ResetTimeDown()
    {
        isTimeDown = true;
        stageTimeLimit.stageTime += 10;
    }

    // 확률감소
    public void PercentDown()
    {
        isPercentDown = true;

        itemSkill.firePercent -= 5f;
        itemSkill.fireShotPercent -= 5f;
        itemSkill.holyShotPercent -= 5f;
        itemSkill.holyWavePercent -= 5f;
        itemSkill.rockPercent -= 5f;
        itemSkill.posionPercent -= 5f;
        itemSkill.meleePercent -= 5f;
        itemSkill.sturnPercent -= 5f;
    }
    // 확률감소 리셋
    public void ResetPercentDown()
    {
        isPercentDown = true;

        itemSkill.firePercent += 5f;
        itemSkill.fireShotPercent += 5f;
        itemSkill.holyShotPercent += 5f;
        itemSkill.holyWavePercent += 5f;
        itemSkill.rockPercent += 5f;
        itemSkill.posionPercent += 5f;
        itemSkill.meleePercent += 5f;
        itemSkill.sturnPercent += 5f;
    }

    // 투사체 속도 증가
    public void MonsterAttackSpdUp()
    {
        isMonsterAttackSpdUp = true;
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");

        foreach (GameObject monster in monsters)
        {
            MonoBehaviour[] scripts = monster.GetComponents<MonoBehaviour>();

            foreach (MonoBehaviour script in scripts)
            {
                if (script.GetType().Name.Contains("Stage"))
                {
                    System.Reflection.FieldInfo field = script.GetType().GetField("bulletSpd");

                    if (field != null)
                    {
                        field.SetValue(script, (float)field.GetValue(script) + 1);
                    }
                }
            }
        }
    }

    // 몬스터 크기 증가
    public void MonsterSizeDown()
    {
        isMonsterSizeDown = true;
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");

        foreach (GameObject monster in monsters)
        {
            Transform monsterTransform = monster.transform;
            Vector3 newScale = monsterTransform.localScale - new Vector3(0.05f, 0.05f, 0f);
            monsterTransform.localScale = newScale;
        }
    }

    // 버프 선택
    public void BuffStatus(bool execution) 
    {
        if(execution)
        {
            List<GameObject> selectedList = (Random.Range(0, 2) == 0) ? buffList : deBuffList;

            if(selectedList == buffList)
            {
                buff = 1;
                Buff();
            }
            else if(selectedList == deBuffList)
            {
                buff = 2;
                DeBuff();
            }

            if (selectedList.Count > 0)
            {
                int randomIndex = Random.Range(0, selectedList.Count);

                status = randomIndex + 1;

                selectedEffect = selectedList[randomIndex];

                selectedEffect.transform.position = statusPos.transform.position;
            }
        }
    }

    // 버프 리셋
    public void ResetStatus()
    {
        if (selectedEffect != null)
        {
            selectedEffect.transform.position = new Vector3(100, 1500, 0);

            if (buff == 1)
            {
                if (status == 1)
                {
                    if (isDamageUp)
                    {
                        ResetDamageUP();
                    }
                }
                else if (status == 3)
                {
                    if (isTimeUp)
                    {
                        ResetTimeUp();
                    }
                }
                else if (status == 4)
                {
                    if (isPercentUp)
                    {
                        ResetPercentUp();
                    }
                }
            }
            else if (buff == 2)
            {
                if (status == 1)
                {
                    if (isDamageDown)
                    {
                        ResetDamageDown();
                    }
                }
                else if (status == 3)
                {
                    if (isTimeDown)
                    {
                        ResetTimeDown();
                    }
                }
                else if (status == 4)
                {
                    if (isPercentDown)
                    {
                        ResetPercentDown();
                    }
                }
            }

            isDamageUp = false;
            isMonsterHealthDown = false;
            isTimeUp = false;
            isPercentUp = false;
            isDamageDown = false;
            isMonsterHealthUP = false;
            isTimeDown = false;
            isPercentDown = false;
            isMonsterAttackSpdUp = false;
            isMonsterSizeDown = false;

            buff = 0;
            status = 0;

            selectedEffect = null;
        }
    }

    // 버프, 디버프 시각적 표시
    void Buff()
    {
        stageStatus.color = new Color(0f, 0.49f, 1f);
    }

    void DeBuff()
    {
        stageStatus.color = Color.red;
    }
}
