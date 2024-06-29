using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CharacterSelect : MonoBehaviour
{
    // 캐릭터 설명
    public GameObject rockEx;
    public GameObject waterEx;
    public GameObject lightEx;
    public GameObject luckEx;

    // 캐릭터 오픈 여부
    public bool waterChar = false;
    public bool lightChar = false;
    public bool luckChar = false;

    // 캐릭터 선택 방지 UI
    public GameObject useWater;
    public GameObject useLight;
    public GameObject useLuck;

    // 선택한 캐릭터
    public int selectChar;
    public GameObject[] checkChar;

    // 플레이어 돈 관리
    public int playerMoney;
    public TMP_Text playerMoneyText;
    
    // 캐릭터 오픈 UI
    public GameObject waterOpenBtn;
    public GameObject lightOpenBtn;
    public GameObject luckOpenBtn;

    // 캐릭터 레벨업 UI
    public GameObject rockLevelUPButton;
    public int rockLevel;
    public TMP_Text rockLevelText;
    public GameObject waterLevelUPButton;
    public int waterLevel;
    public TMP_Text waterLevelText;
    public GameObject lightLevelUPButton;
    public int lightLevel;
    public TMP_Text lightLevelText;
    public GameObject luckLevelUPButton;
    public int luckLevel;
    public TMP_Text luckLevelText;

    // 게임시작
    public bool enter;
    public TMP_Text enterText;

    private void Start()
    {
        LoadPlayerDatas(); // 캐릭터 데이터 관리

        // 게임시작 관리
        enter = false;
        enterText.color = Color.black;
    }

    void LoadPlayerDatas() // 캐릭터 데이터 관리
    {
        waterChar = PlayerPrefs.GetInt("WaterCharOpen", 0) == 1;
        lightChar = PlayerPrefs.GetInt("LightCharOpen", 0) == 1;
        luckChar = PlayerPrefs.GetInt("LuckCharOpen", 0) == 1;

        rockLevel = PlayerPrefs.GetInt("rockLevel", 1);
        waterLevel = PlayerPrefs.GetInt("waterLevel", 1);
        lightLevel = PlayerPrefs.GetInt("lightLevel", 1);
        luckLevel = PlayerPrefs.GetInt("luckLevel", 1);
    }

    void Update()
    {
        UpdatePlayerMoney(); // 플레이어 머니 관리
        SelectCharacterText(); // 캐릭터 선택 여부 텍스트
        UpdateOpenCharacter(); // 캐릭터 오픈 여부
        UpdateCharacterLevelText(); // 캐릭터 레벨 텍스트
        MaxCharacterLevel(); // 캐릭터 레벨 상한
    }

    void UpdatePlayerMoney() // 플레이어 머니 관리
    {
        playerMoney = PlayerPrefs.GetInt("GameMoney", 0);
        playerMoneyText.text = playerMoney.ToString();
    }

    void SelectCharacterText() // 캐릭터 선택 여부 텍스트
    {   
        if (!enter)
        {
            enterText.fontSize = 35f;
            enterText.text = "캐릭터를" + "\n" + "선택해 주세요.";
        }
        else
        {
            enterText.fontSize = 50f;
            enterText.text = "게임시작";
        }
    }

    void UpdateOpenCharacter() // 캐릭터 오픈 여부
    {
        if (waterChar)
        {
            useWater.SetActive(false);
            waterOpenBtn.SetActive(false);
            waterLevelUPButton.SetActive(true);
            waterLevelText.gameObject.SetActive(true);
        }
        if (lightChar)
        {
            useLight.SetActive(false);
            lightOpenBtn.SetActive(false);
            lightLevelUPButton.SetActive(true);
            lightLevelText.gameObject.SetActive(true);
        }
        if (luckChar)
        {
            useLuck.SetActive(false);
            luckOpenBtn.SetActive(false);
            luckLevelUPButton.SetActive(true);
            luckLevelText.gameObject.SetActive(true);
        }
    } 

    void UpdateCharacterLevelText() // 캐릭터 레벨 텍스트
    {
        rockLevelText.text = rockLevel.ToString();
        waterLevelText.text = waterLevel.ToString();
        lightLevelText.text = lightLevel.ToString();
        luckLevelText.text = luckLevel.ToString();
    }

    void MaxCharacterLevel() // 캐릭터 레벨 상한
    {
        if (rockLevel >= 20)
        {
            rockLevelUPButton.SetActive(false);
        }
        if (waterLevel >= 20)
        {
            waterLevelUPButton.SetActive(false);
        }
        if (lightLevel >= 20)
        {
            lightLevelUPButton.SetActive(false);
        }
        if (luckLevel >= 20)
        {
            luckLevelUPButton.SetActive(false);
        }
    }

    void CheckReset() // 캐릭터 선택 UI 초기화
    {
        foreach(GameObject check in checkChar)
        {
            check.SetActive(false);
        }
    }

    // 캐릭터 선택
    public void RockChar() // 돌 캐릭터
    {
        CheckReset(); // 캐릭터 선택 UI 초기화
        checkChar[0].SetActive(true); // 캐릭터 선택 UI 활성화

        // 돌 캐릭터 설명 활성화, 나머지 비활성화
        rockEx.SetActive(true);
        waterEx.SetActive(false);
        lightEx.SetActive(false);
        luckEx.SetActive(false);

        enter = true; // 게임 시작 가능 여부
        selectChar = 1; // 선택한 캐릭터 값
    }

    public void WaterChar()
    {
        if (waterChar)
        {
            CheckReset();
            checkChar[1].SetActive(true);

            waterEx.SetActive(true);
            rockEx.SetActive(false);
            lightEx.SetActive(false);
            luckEx.SetActive(false);

            enter = true;
            selectChar = 2;
        }
    }

    public void LightChar()
    {
        if (lightChar)
        {
            CheckReset();
            checkChar[2].SetActive(true);

            lightEx.SetActive(true);
            rockEx.SetActive(false);
            waterEx.SetActive(false);
            luckEx.SetActive(false);

            enter = true;
            selectChar = 3;
        }
    }

    public void LuckChar()
    {
        if (luckChar)
        {
            CheckReset();
            checkChar[3].SetActive(true);

            luckEx.SetActive(true);
            rockEx.SetActive(false);
            waterEx.SetActive(false);
            lightEx.SetActive(false);

            enter = true;
            selectChar = 4;
        }
    }

    // 캐릭터 오픈
    public void OpenWater() // 물 캐릭터 오픈
    {
        if (playerMoney >= 100) // 100원 이상일때
        {
            waterChar = true; // 캐릭터 오픈
            playerMoney -= 100; // 사용한 돈 제거
            PlayerPrefs.SetInt("WaterCharOpen", 1);
            PlayerPrefs.SetInt("GameMoney", playerMoney);
        }
    }

    public void OpenLight()
    {
        if (playerMoney >= 500)
        {
            lightChar = true;
            playerMoney -= 500;
            PlayerPrefs.SetInt("LightCharOpen", 1);
            PlayerPrefs.SetInt("GameMoney", playerMoney);
        }
    }

    public void OpenLuck()
    {
        if (playerMoney >= 1000)
        {
            luckChar = true;
            playerMoney -= 1000;
            PlayerPrefs.SetInt("LuckCharOpen", 1);
            PlayerPrefs.SetInt("GameMoney", playerMoney);
        }
    }

    // 캐릭터 레벨업
    public void LevelUpRock() // 돌 캐릭터 레벨업
    {
        if (rockLevel <= 20) // 레벨이 20이하일때
        {
            if (playerMoney >= 1000) // 1000원 이상일때
            {
                rockLevel++; // 레벨얼
                playerMoney -= 1000; // 사용한 1000원 제거
                PlayerPrefs.SetInt("rockLevel", rockLevel);
                PlayerPrefs.SetInt("GameMoney", playerMoney);
            }
        }
    }
    public void LevelUpWater()
    {
        if (waterLevel <= 20)
        {
            if (playerMoney >= 1000) 
            {
                waterLevel++;
                playerMoney -= 1000;
                PlayerPrefs.SetInt("waterLevel", waterLevel);
                PlayerPrefs.SetInt("GameMoney", playerMoney);
            }
        }
    }
    public void LevelUpLight()
    {
        if (lightLevel <= 20)
        {
            if (playerMoney >= 1000)
            {
                lightLevel++;
                playerMoney -= 1000;
                PlayerPrefs.SetInt("lightLevel", lightLevel);
                PlayerPrefs.SetInt("GameMoney", playerMoney);
            }
        }
    }
    public void LevelUpLuck()
    {
        if (luckLevel <= 20)
        {
            if (playerMoney >= 1000)
            {
                luckLevel++;
                playerMoney -= 1000;
                PlayerPrefs.SetInt("luckLevel", luckLevel);
                PlayerPrefs.SetInt("GameMoney", playerMoney);
            }
        }
    }

    // 게임시작
    public void GameStart()
    {
        if (enter) // 게임시작 가능일때
        {
            enterText.color = Color.white; // 텍스트 시각화
            PlayerPrefs.SetInt("SelectChar", selectChar);
            StartCoroutine(GameStartButton()); // 게임시작
        }
        else
        {
            StartCoroutine(EnterTextColor()); // 캐릭터 비선택시 텍스트 (시작불가)
        }
    }

    // 캐릭터 비선택시 텍스트 (시작불가)
    IEnumerator EnterTextColor()
    {
        enterText.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        enterText.color = Color.black;
    }

    // 캐릭터 선택시 게임이동
    IEnumerator GameStartButton()
    {
        yield return new WaitForSeconds(1f); // 만약을 위한 1초 대기

        LodingController.LoadNextScene("Game");
    }
}
