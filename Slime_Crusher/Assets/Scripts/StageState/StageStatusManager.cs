using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Burst.Intrinsics;

public class StageStatusManager : MonoBehaviour
{
    public StageTimeLimit stageTimeLimit;
    public PlayerController playerController;
    private StageManager stageManager;
    public ItemSkill itemSkill;

    public int buff; // 버프, 디버프 구분
    public int status; // 버프 종류 선택
    public Image stageStatus; 
    public GameObject statusPos;

    // 버프
    public bool isDamageUp; // 기본데미지 증가
    public bool isMonsterHealthDown; // 몬스터 체력 감소
    public bool isTimeUp; // 제한시간 증가
    public bool isPercentUp; // 확률 증가
    public GameObject damageUp; // 기본데미지 증가
    public GameObject monsterHealthDown; // 몬스터 체력 감소
    public GameObject timeUp; // 제한시간 증가
    public GameObject percentUp; // 확률 증가
    public GameObject monsterDie; // 일정시간 마다 몬스터 제거
    private float timer = 0f;
    private float delay = 10f;
    private List<GameObject> buffList = new List<GameObject>(); // 버프 리스트

    // 디버프
    public bool isDamageDown; // 기본데미지 감소
    public bool isMonsterHealthUP; // 몬스터 체력 증가
    public bool isTimeDown; // 제한시간 감소
    public bool isPercentDown; // 확률감소
    public bool isMonsterAttackSpdUp; // 투사체 속도 증가
    public bool isMonsterSizeDown;// 몬스터 크기 감소
    public GameObject damageDown; // 기본데미지 감소
    public GameObject monsterHealthUP; // 몬스터 체력 증가
    public GameObject timeDown; // 제한시간 감소
    public GameObject percentDown; // 확률감소
    public GameObject monsterAttackSpdUp; // 투사체 속도 증가
    public GameObject monsterSizeDown;// 몬스터 크기 감소
    private List<GameObject> deBuffList = new List<GameObject>(); // 디버프 리스트

    public GameObject selectedEffect; // 선택된 버프
    public TMP_Text buffText;

    private StageStatus currentStatus;

    private void Awake()
    {
        if (!stageTimeLimit)
            stageTimeLimit = FindObjectOfType<StageTimeLimit>();
        if (!playerController)
            playerController = FindObjectOfType<PlayerController>();
        if (!itemSkill)
            itemSkill = FindObjectOfType<ItemSkill>();
        if (!stageManager)
            stageManager = FindObjectOfType<StageManager>();
    }

    void Start()
    {
        // 버프 리스트 추가
        buffList.Add(damageUp);
        buffList.Add(monsterHealthDown);
        buffList.Add(timeUp);
        buffList.Add(percentUp);
        buffList.Add(monsterDie);

        // 디버프 리스트 추가
        deBuffList.Add(damageDown);
        deBuffList.Add(monsterHealthUP);
        deBuffList.Add(timeDown);
        deBuffList.Add(percentDown);
        deBuffList.Add(monsterAttackSpdUp);
        deBuffList.Add(monsterSizeDown);

        // 버프 적용상태 초기화
        ResetState();
    }

    void Update()
    {
        if (buff == 1) // 버프 적용
        {
            ApplyBuff();
        }
        else if (buff == 2) // 디버프 적용
        {
            ApplyDeBuff();
        }
    }

    void ApplyBuff() // 버프 적용
    {
        switch (status)
        {
            case 1:
                if (!isDamageUp)
                {
                    isDamageUp = true;
                    currentStatus = new DamageUpStatus();
                }
                break;
            case 2:
                if (!isMonsterHealthDown)
                {
                    isMonsterHealthDown = true;
                    currentStatus = new MonsterHealthDownStatus();
                }
                break;
            case 3:
                if (!isTimeUp)
                {
                    isTimeUp = true;
                    currentStatus = new TimeUpStatus();
                }
                    break;
            case 4:
                if (!isPercentUp)
                {
                    isPercentUp = true;
                    currentStatus = new PercentUpStatus();
                }
                break;
            case 5:
                timer += Time.deltaTime;

                if (timer >= delay)
                {
                    currentStatus = new MonsterDieStatus();
                    timer = 0f;
                }
                break;
        }
        if (currentStatus != null)
        {
            currentStatus.Apply(this);
        }
    }

    void ApplyDeBuff() // 디버프 적용
    {
        if (currentStatus != null) return;

        switch (status)
        {
            case 1:
                if (!isDamageDown)
                {
                    isDamageDown = true;
                    currentStatus = new DamageDownStatus();
                }
                break;
            case 2:
                if (!isMonsterHealthUP)
                {
                    isMonsterHealthUP = true;
                    currentStatus = new MonsterHealthUPStatus();
                }
                break;
            case 3:
                if (!isTimeDown)
                {
                    isTimeDown = true;
                    currentStatus = new TimeDownStatus();
                }
                break;
            case 4:
                if (!isPercentDown)
                {
                    isPercentDown = true;
                    currentStatus = new PercentDownStatus();
                }
                break;
            case 5:
                if (!isMonsterAttackSpdUp)
                {
                    isMonsterAttackSpdUp = true;
                    currentStatus = new MonsterAttackSpdUpStatus();
                }
                break;
            case 6:
                if (!isMonsterSizeDown)
                {
                    isMonsterSizeDown = true;
                    currentStatus = new MonsterSizeDownStatus();
                }
                break;
        }
        if (currentStatus != null)
        {
            currentStatus.Apply(this);
        }
    }

    // 버프 선택
    public void BuffStatus(bool execution) 
    {
        if (currentStatus != null) return;

        if (execution)
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
        if (currentStatus != null)
        {
            currentStatus.Reset(this);
            currentStatus = null;
            buffText.text = "";
            if (selectedEffect != null)
            {
                selectedEffect.transform.position = new Vector3(-300, -300, 1);
            }
        }
    }
    
    void ResetState() // 버프 초기화
    {
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
