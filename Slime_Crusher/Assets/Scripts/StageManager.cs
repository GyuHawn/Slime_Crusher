using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class StageManager : MonoBehaviour, Observer
{
    private MonsterSpawn monsterSpawn;
    private SelectItem selectItem;
    private SelectPass selectPass;
    private StageTimeLimit stageTimeLimit;
    private PlayerController playerController;
    private CharacterSkill characterSkill;
    private Character character;
    private ItemSkill itemSkill;
    private StageStatusManager stageStatusManager;
    private StageChange stageChange;
    private Combo combo;
    private GameTimeUI gameTimeUI;

    public bool gameStart = false; // 게임시작 여부

    public GameObject[] map;
    public int mainStage; // 메인스테이지 (1, 2, 3...)
    public int subStage; // 서브스테이지 (1-1, 1-2...)
    private bool isNextStageAvailable = true;
    public TMP_Text stageText;

    public int base0Monster; // 스테이지 몬스터 [0]의 수
    public int base1Monster; // 스테이지 몬스터 [1]의 수
    public int base2Monster; // 스테이지 몬스터 [2]의 수
    public int base3Monster; // 스테이지 몬스터 [3]의 수
    public int bossMonster; // 보스몬스터 수
    public bool allMonstersSpawned = false; // 모든 몬스터 소환 확인

    public float timeLimit; // 스테이지당 제한시간

    public bool selectingPass; // 패시브 선택중인지

    public float totalTime; // 총 시간
    public int rewardMoney; // 획득 머니
    public TMP_Text totalTimeText;
    public TMP_Text rewardMoneyText;
    public TMP_Text finalWaveText;
    public TMP_Text maxComboText;
    public TMP_Text clearText;

    public bool passing; // 스테이지 전환 표시 중

    public int comboNum;
    public int maxComboNum;

    public Canvas canvas;

    private void Awake()
    {
        if (!monsterSpawn)
            monsterSpawn = FindObjectOfType<MonsterSpawn>();
        if (!selectItem)
            selectItem = FindObjectOfType<SelectItem>();
        if (!selectPass)
            selectPass = FindObjectOfType<SelectPass>();
        if (!stageTimeLimit)
            stageTimeLimit = FindObjectOfType<StageTimeLimit>();
        if (!playerController)
            playerController = FindObjectOfType<PlayerController>();
        if (!characterSkill)
            characterSkill = FindObjectOfType<CharacterSkill>();
        if (!character)
            character = FindObjectOfType<Character>();
        if (!itemSkill)
            itemSkill = FindObjectOfType<ItemSkill>();
        if (!stageStatusManager)
            stageStatusManager = FindObjectOfType<StageStatusManager>();
        if (!stageChange)
            stageChange = FindObjectOfType<StageChange>();
        if (!combo)
            combo = FindObjectOfType<Combo>();

        if (gameTimeUI != null)
        {
            gameTimeUI.RegisterObserver(FindObjectOfType<GameUIManager>());
        }
    }

    void Start()
    {
        // 1-1 시작 설정 후 게임 시작
        if (!gameStart)
        {
            InitialStageSetting(); // 스테이지 초반세팅
            StageMonsterSetting();
            SpawnMonsters();
        }       
    }
    void InitialStageSetting() // 스테이지 초반 세팅
    {
        mainStage = 1;
        subStage = 1;
        selectingPass = false;
        gameStart = true;

        totalTime = 0;
        rewardMoney = 0;
    }

    void ClearSetting() // 클리어 후 세팅
    {
        clearText.text = "게임 클리어!!";
        clearText.fontSize = 40;
        Time.timeScale = 0f;
        Reward();
        gameStart = false;
        playerController.gameover.SetActive(true);
    }
    void Update()
    {
        // 게임 클리어
        if(mainStage > 20)
        {
            ClearSetting(); // 클리어 후 세팅
        }

        UpdateMap(); // 스테이지 맵 변경
        UpdateStageText(); // 스테이지 표시 텍스트 변경
        TimeOver(); // 제한시간 초과시 게임종료
        GameOver();  // 게임종료 결과 표시, 집계
        StageAudio(); // 스테이지오디오   
    }
    void UpdateMap() // 스테이지 맵 변경
    {
        for (int i = 0; i < Mathf.Min(8, mainStage); ++i)
        {
            map[i].SetActive(i == mainStage - 1);
        }
    }

    void UpdateStageText() // 스테이지 표시 텍스트 변경
    {
        if (mainStage <= 7)
        {
            stageText.text = mainStage <= 7 ? $"스테이지 {mainStage}-{subStage}" : $"스테이지 {mainStage}";
        }
    }

    void TimeOver() // 제한시간 초과시 게임종료
    {     
        if (stageTimeLimit.stageFail >= stageTimeLimit.stageTime)
        {
            playerController.gameover.SetActive(true);
            gameStart = false;
        }
    }


    public void UpdateTime(float gameTime)
    {
        totalTime = gameTime; // 게임 시간을 누적하여 totalTime으로 설정
    }
    public void UpdateCombo(int combo, int maxCombo)
    {
        maxComboNum = maxCombo;
    }
    void GameOver()  // 게임종료 결과 표시, 집계
    {
        totalTimeText.text = string.Format("{0:00}:{1:00}", Mathf.Floor(totalTime / 60), totalTime % 60);
        finalWaveText.text = mainStage < 8 ? $"{mainStage} - {subStage} Wave" : $"{mainStage} Wave";
        maxComboText.text = maxComboNum.ToString();
        rewardMoneyText.text = rewardMoney.ToString();
    }

    // 게임중 머니 획득 (결과 머니 게임중 획득)
    public void Reward()
    {
        if (gameStart)
        {
            rewardMoney += (int)totalTime + maxComboNum;
            int playerMoney = PlayerPrefs.GetInt("GameMoney", 0) + rewardMoney;
            PlayerPrefs.SetInt("GameMoney", playerMoney);
            PlayerPrefs.Save();
        }       
    }

    // 스테이지 몬스터 수 설정
    public void StageMonsterSetting()
    {
        if (mainStage <= 7) // 1~7 스테이지 설정
        {          
            switch (subStage)
            {
                case 1:
                    base0Monster = 2;
                    break;
                case 2:
                    base0Monster = 2;
                    base1Monster = 2;
                    break;
                case 3:
                    base0Monster = 2;
                    base1Monster = 2;
                    base2Monster = 3;
                    break;
                case 4:
                    base0Monster = 2;
                    base1Monster = 2;
                    base2Monster = 3;
                    base3Monster = 3;
                    break;
                case 5:
                    base0Monster = 1;
                    base1Monster = 2;
                    base2Monster = 3;
                    base3Monster = 3;
                    bossMonster = 1;
                    break;
            }
        }
        else  // 8 스테이지 이후
        {    
            base0Monster = 2;
            base1Monster = 1;
            base2Monster = 1;
            base3Monster = mainStage - 7; // 8 스테이지 이후부터 몬스터 증가를 위한 값  
            bossMonster = 1;
        }
    }

    // 몬스터 수 초기화
    void NextMonsterNumSetting()
    {
        base0Monster = 1;
        base1Monster = 0;
        base2Monster = 0;
        base3Monster = 0;
        bossMonster = 0;
        
        stageTimeLimit.stageFail = 0f;
    }

    // 몬스터 스폰
    void SpawnMonsters()
    {
        monsterSpawn.MonsterInstantiate(base0Monster, base1Monster, base2Monster, base3Monster, bossMonster);
        allMonstersSpawned = true;
    }

    // 패시브 선택
    void SelectPass()
    {
        selectPass.passMenu.SetActive(true);
        playerController.isAttacking = true;
        Time.timeScale = 0f;
    }

    // 스테이지 변경
    public void NextStage()
    {
        if (isNextStageAvailable)
        {
            if (mainStage <= 20)
            {
                allMonstersSpawned = false;
                characterSkill.CharacterCoolTime();
                ResetStageState();
                NextStageCharacterReset(); // 스테이지 전환시 캐릭터 상태 관리
                NextStageSetting(); // 스테이지 전환 관리
                NextSetting();
            }
            StartCoroutine(DelayNextStage());
        }
    }
    void NextStageCharacterReset() // 스테이지 전환시 캐릭터 상태 관리
    {
        if (character.currentCharacter == 2)
        {
            characterSkill.useWaterSkill = false;
        }
    }
    void NextStageSetting() // 스테이지 전환 관리
    {
        if (mainStage < 8)
        {
            NextSubStage();

            if (mainStage >= 2)
            {
                if (subStage == 2)
                {
                    selectingPass = true;
                    SelectPass();
                }
            }

            if (subStage == 3)
            {
                selectItem.ItemSelect();
                StartCoroutine(DelayStage());
            }
            else if (subStage > 5)
            {
                subStage = 1;
                NextMainStage();

                selectItem.ItemSelect();
                StartCoroutine(DelayStage());
            }
        }
        else
        {
            NextMainStage();
            if (mainStage == 12 || mainStage == 15 || mainStage == 18)
            {
                selectingPass = true;
                SelectPass();
            }
            else if (mainStage == 10 || mainStage == 16)
            {
                selectItem.ItemSelect();
                StartCoroutine(DelayStage());
            }
        }
    }

    void NextSetting()
    {
        if (mainStage >= 8)
        {
            if (mainStage == 8 || mainStage == 10 || mainStage == 12 || mainStage == 15 || mainStage == 16 || mainStage == 18)
            { }
            else
            {
                passing = true;
                NextStageInforSettingReady();
            }
        }
        else
        {
            if ((mainStage >= 2 && (subStage == 1 || subStage == 2)) || subStage == 3)
            { }
            else
            {
                passing = true;
                NextStageInforSettingReady();
            }
        }
    }

    // 스테이지 변경전 초기화 및 설정
    public void NextStageInforSettingReady()
    {
        if (mainStage <= 20 && passing)
        {
            StartCoroutine(NextStageInforSetting()); // 스테이지 정보 세팅
        }
    }
    
    IEnumerator NextStageInforSetting() // 스테이지 정보 세팅
    {
        stageTimeLimit.stageFail = 0f;
        yield return new WaitForSeconds(0.1f);

        stageChange.ChangeStageText();
        passing = false;
        yield return new WaitForSeconds(3f);

        stageStatusManager.ResetStatus(); // 버프 초기화

        NextMonsterNumSetting(); // 스테이지 이동시 몬스터수 초기화
        StageMonsterSetting(); // 스테이지 몬스터 수 설정
        SpawnMonsters(); // 몬스터 스폰

        stageStatusManager.BuffStatus(allMonstersSpawned); // 버프 설정
    }

    // 스테이지 변경 전 초기화 시간 
    IEnumerator DelayNextStage()
    {
        isNextStageAvailable = false;
        yield return new WaitForSeconds(1f); 
        isNextStageAvailable = true; 
    }

    // 맵에 남아있는 이전 스테이지 데이터 제거
    void ResetStageState()
    {
        GameObject[] skills = GameObject.FindObjectsOfType<GameObject>();

        foreach (GameObject skill in skills)
        {
            if (skill.name == "BossSkill" || skill.name == "PlayerSkill" || skill.name == "MonsterAttack" || skill.name == "HealthUpItem")
            {
                Destroy(skill);
            }
        }

        itemSkill.holyWaving = false;
        playerController.stage5Debuff = false;
    }

    // 스테이지 클리어시 머니 획득
    void NextSubStage()
    {
        subStage++;
        rewardMoney += 10;
    }

    void NextMainStage()
    {
        mainStage++;
        rewardMoney += 100;
    }

    IEnumerator DelayStage()
    {
        yield return new WaitForSeconds(1f);
    }  

    void StageAudio()
    {
        if (SceneManager.GetActiveScene().name == "Game")
        {
            if (mainStage <= 8)
            {
                if (subStage == 5)
                {
                    AudioManager.Instance.PlayBossAudio();
                }
                else
                {
                    AudioManager.Instance.PlayStageAudio();
                }
            }
            else
            {
                AudioManager.Instance.PlayBossAudio();
            }
        }
    }
}
