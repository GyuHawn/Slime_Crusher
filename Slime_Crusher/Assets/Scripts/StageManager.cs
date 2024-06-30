using System.Collections;
using UnityEngine;
using TMPro;

public class StageManager : MonoBehaviour
{
    private MonsterSpwan monsterSpawn;
    private SelectItem selectItem;
    private SelectPass selectPass;
    private StageTimeLimit stageTimeLimit;
    private PlayerController playerController;
    private CharacterSkill characterSkill;
    private Character character;
    private ItemSkill itemSkill;
    private StageStatus stageStatus;
    private StageChange stageChange;
    private Combo combo;

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

    public Canvas canvas;

    private void Awake()
    {
        monsterSpawn = GameObject.Find("Manager").GetComponent<MonsterSpwan>();
        selectItem = GameObject.Find("Manager").GetComponent<SelectItem>();
        selectPass = GameObject.Find("Manager").GetComponent<SelectPass>();
        stageTimeLimit = GameObject.Find("Manager").GetComponent<StageTimeLimit>();
        playerController = GameObject.Find("Manager").GetComponent<PlayerController>();
        characterSkill = GameObject.Find("Manager").GetComponent<CharacterSkill>();
        character = GameObject.Find("Manager").GetComponent<Character>();
        itemSkill = GameObject.Find("Manager").GetComponent<ItemSkill>();
        stageStatus = GameObject.Find("Manager").GetComponent<StageStatus>();
        stageChange = GameObject.Find("Manager").GetComponent<StageChange>();
        combo = GameObject.Find("Manager").GetComponent<Combo>();
    }

    void Start()
    {
        // 1-1 시작 설정 후 게임 시작
        if (!gameStart)
        {
            mainStage = 1;
            subStage = 1;
            StageMonsterSetting();
            SpawnMonsters();
            selectingPass = false;
            gameStart = true;

            totalTime = 0;
            rewardMoney = 0;
        }   
    }

    void Update()
    {
        // 게임 클리어
        if(mainStage > 20)
        {
            clearText.text = "게임 클리어!!";
            clearText.fontSize = 40;
            Time.timeScale = 0f;
            Reward();
            gameStart = false;
            playerController.gameover.SetActive(true);
        }

        UpdateMap(); // 스테이지 맵 변경
        UpdateStageText(); // 스테이지 표시 텍스트 변경
        TimeOver(); // 제한시간 초과시 게임종료
        GameOver();  // 게임종료 결과 표시, 집계
    }
    void UpdateMap() // 스테이지 맵 변경
    {
        for (int i = 0; i < Mathf.Min(8, mainStage); ++i)
        {
            map[i].SetActive(i == mainStage - 1);
        }
        /*
         if (mainStage <= 8)
        {
            for (int i = 0; i < mainStage - 1; ++i)
            {
                map[i].SetActive(false);
            }

            map[mainStage - 1].SetActive(true);
        }
        else
        {
            for (int i = 0; i < 7; ++i)
            {
                map[i].SetActive(false);
            }

            map[7].SetActive(true);
        }
        */
    }
    void UpdateStageText() // 스테이지 표시 텍스트 변경
    { 
        if (mainStage <= 7)
        {
            stageText.text = "스테이지 " + mainStage + "-" + subStage;
        }
        else if (mainStage >= 8)
        {
            stageText.text = "스테이지 " + mainStage;
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

    void GameOver()  // 게임종료 결과 표시, 집계
    {
        totalTime = playerController.gameTime;
        totalTimeText.text = string.Format("{0:00}:{1:00}", Mathf.Floor(totalTime / 60), totalTime % 60);
        if (mainStage < 8)
        {
            finalWaveText.text = mainStage + " - " + subStage + " Wave";
        }
        else if (mainStage >= 8)
        {
            finalWaveText.text = mainStage + " Wave";
        }
        maxComboText.text = combo.maxComboNum.ToString();
        rewardMoneyText.text = rewardMoney.ToString();
    }



    // 게임중 머니 획득 (결과 머니 게임중 획득)
    public void Reward()
    {
        if (gameStart)
        {
            rewardMoney += (int)(totalTime * 1);
            rewardMoney += combo.maxComboNum;

            int playerMoney = PlayerPrefs.GetInt("GameMoney", 0);
            playerMoney += rewardMoney;
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
    void NextStageSetting()
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

                if (character.currentCharacter == 2)
                {
                    characterSkill.useWaterSkill = false;
                }

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

                if (mainStage >= 8)
                {
                    if (mainStage == 8 || mainStage == 10 || mainStage == 12 || mainStage == 15 || mainStage == 16 || mainStage == 18)
                    {}
                    else
                    {
                        passing = true;
                        NextSetting();
                    }
                }
                else
                {
                    if ((mainStage >= 2 && (subStage == 1 || subStage == 2)) || subStage == 3)
                    {}
                    else
                    {
                        passing = true;
                        NextSetting();
                    }
                }

            }
            StartCoroutine(DelayNextStage());
        }
    }

    // 스테이지 변경 전 초기화 시간 
    IEnumerator DelayNextStage()
    {
        isNextStageAvailable = false;
        yield return new WaitForSeconds(1f); 
        isNextStageAvailable = true; 
    }

    // 스테이지 변경전 초기화 및 설정
    public void NextSetting()
    {
        if (mainStage <= 20 && passing)
        {
            StartCoroutine(NextStageOrTile());
        }
    }

    IEnumerator NextStageOrTile()
    {
        stageTimeLimit.stageFail = 0f;
        yield return new WaitForSeconds(0.1f);

        stageChange.ChangeStageText();
        passing = false;
        yield return new WaitForSeconds(3f);

        stageStatus.ResetStatus(); // 버프 초기화

        NextStageSetting(); // 스테이지 이동시 몬스터수 초기화
        StageMonsterSetting(); // 스테이지 몬스터 수 설정
        SpawnMonsters(); // 몬스터 스폰

        stageStatus.BuffStatus(allMonstersSpawned); // 버프 설정
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

        itemSkill.holyWave = false;
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
}
