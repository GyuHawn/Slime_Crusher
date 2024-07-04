using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CharacterSelect : MonoBehaviour
{
    // ĳ���� ����
    public GameObject rockEx;
    public GameObject waterEx;
    public GameObject lightEx;
    public GameObject luckEx;

    // ĳ���� ���� ����
    public bool waterChar = false;
    public bool lightChar = false;
    public bool luckChar = false;

    // ĳ���� ���� ���� UI
    public GameObject useWater;
    public GameObject useLight;
    public GameObject useLuck;

    // ������ ĳ����
    public int selectChar;
    public GameObject[] checkChar;

    // �÷��̾� �� ����
    public int playerMoney;
    public TMP_Text playerMoneyText;
    
    // ĳ���� ���� UI
    public GameObject waterOpenBtn;
    public GameObject lightOpenBtn;
    public GameObject luckOpenBtn;

    // ĳ���� ������ UI
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

    // ���ӽ���
    public bool enter;
    public TMP_Text enterText;

    private CommandInvoker invoker = new CommandInvoker();

    private void Start()
    {
        LoadPlayerDatas(); // ĳ���� ������ ����

        // ���ӽ��� ����
        enter = false;
        enterText.color = Color.black;
    }

    void LoadPlayerDatas() // ĳ���� ������ ����
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
        UpdatePlayerMoney(); // �÷��̾� �Ӵ� ����
        SelectCharacterText(); // ĳ���� ���� ���� �ؽ�Ʈ
        UpdateOpenCharacter(); // ĳ���� ���� ����
        UpdateCharacterLevelText(); // ĳ���� ���� �ؽ�Ʈ
        MaxCharacterLevel(); // ĳ���� ���� ����
    }

    void UpdatePlayerMoney() // �÷��̾� �Ӵ� ����
    {
        playerMoney = PlayerPrefs.GetInt("GameMoney", 0);
        playerMoneyText.text = playerMoney.ToString();
    }

    void SelectCharacterText() // ĳ���� ���� ���� �ؽ�Ʈ
    {   
        if (!enter)
        {
            enterText.fontSize = 35f;
            enterText.text = "ĳ���͸�" + "\n" + "������ �ּ���.";
        }
        else
        {
            enterText.fontSize = 50f;
            enterText.text = "���ӽ���";
        }
    }

    void UpdateOpenCharacter() // ĳ���� ���� ����
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

    void UpdateCharacterLevelText() // ĳ���� ���� �ؽ�Ʈ
    {
        rockLevelText.text = rockLevel.ToString();
        waterLevelText.text = waterLevel.ToString();
        lightLevelText.text = lightLevel.ToString();
        luckLevelText.text = luckLevel.ToString();
    }

    void MaxCharacterLevel() // ĳ���� ���� ����
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

    void CheckReset() // ĳ���� ���� UI �ʱ�ȭ
    {
        foreach(GameObject check in checkChar)
        {
            check.SetActive(false);
        }
    }

    // ĳ���� ����
    public void RockChar() // �� ĳ����
    {
        AudioManager.Instance.PlayButtonAudio();
        playerMoney += 100000;
        PlayerPrefs.SetInt("GameMoney", playerMoney);

        CheckReset(); // ĳ���� ���� UI �ʱ�ȭ
        checkChar[0].SetActive(true); // ĳ���� ���� UI Ȱ��ȭ

        // �� ĳ���� ���� Ȱ��ȭ, ������ ��Ȱ��ȭ
        rockEx.SetActive(true);
        waterEx.SetActive(false);
        lightEx.SetActive(false);
        luckEx.SetActive(false);

        enter = true; // ���� ���� ���� ����
        selectChar = 1; // ������ ĳ���� ��
    }

    public void WaterChar()
    {
        if (waterChar)
        {
            AudioManager.Instance.PlayButtonAudio();
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
            AudioManager.Instance.PlayButtonAudio();
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
            AudioManager.Instance.PlayButtonAudio();
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

    // ĳ���� ����
    public void OpenWater() // �� ĳ���� ����
    {
        var openWaterCommand = new OpenCharacterCommand(this, 100, "WaterCharOpen", () => { waterChar = true; });
        invoker.AddCommand(openWaterCommand);
        invoker.ExecuteCommands();
    }

    public void OpenLight()
    {
        var openLightCommand = new OpenCharacterCommand(this, 500, "LightCharOpen", () => { lightChar = true; });
        invoker.AddCommand(openLightCommand);
        invoker.ExecuteCommands();
    }

    public void OpenLuck()
    {
        var openLuckCommand = new OpenCharacterCommand(this, 1000, "LuckCharOpen", () => { luckChar = true; });
        invoker.AddCommand(openLuckCommand);
        invoker.ExecuteCommands();
    }

    // ĳ���� ������
    public void LevelUpRock() // �� ĳ���� ������
    {
        var levelUpRockCommand = new LevelUpCharacterCommand(this, 1000, ref rockLevel, "rockLevel");
        invoker.AddCommand(levelUpRockCommand);
        invoker.ExecuteCommands();
    }

    public void LevelUpWater()
    {
        var levelUpWaterCommand = new LevelUpCharacterCommand(this, 1000, ref waterLevel, "waterLevel");
        invoker.AddCommand(levelUpWaterCommand);
        invoker.ExecuteCommands();
    }

    public void LevelUpLight()
    {
        var levelUpLightCommand = new LevelUpCharacterCommand(this, 1000, ref lightLevel, "lightLevel");
        invoker.AddCommand(levelUpLightCommand);
        invoker.ExecuteCommands();
    }

    public void LevelUpLuck()
    {
        var levelUpLuckCommand = new LevelUpCharacterCommand(this, 1000, ref luckLevel, "luckLevel");
        invoker.AddCommand(levelUpLuckCommand);
        invoker.ExecuteCommands();
    }

    // ���ӽ���
    public void GameStart()
    {
        if (enter) // ���ӽ��� �����϶�
        {
            AudioManager.Instance.PlayButtonAudio();
            enterText.color = Color.white; // �ؽ�Ʈ �ð�ȭ
            PlayerPrefs.SetInt("SelectChar", selectChar);
            StartCoroutine(GameStartButton()); // ���ӽ���
        }
        else
        {
            StartCoroutine(EnterTextColor()); // ĳ���� ���ý� �ؽ�Ʈ (���ۺҰ�)
        }
    }

    // ĳ���� ���ý� �ؽ�Ʈ (���ۺҰ�)
    IEnumerator EnterTextColor()
    {
        enterText.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        enterText.color = Color.black;
    }

    // ĳ���� ���ý� �����̵�
    IEnumerator GameStartButton()
    {
        yield return new WaitForSeconds(1f); // ������ ���� 1�� ���

        SceneManager.LoadScene("Game");
       // LodingController.LoadNextScene("Game");
    }
}