using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SelectPass : MonoBehaviour
{
    private PlayerController playerController;
    private StageManager stageManager;
    private StageTimeLimit stageTimeLimit;

    public GameObject passMenu; // 패시브 선택 UI

    public int selecPass; // 선택한 패시브

    // 패시브 이름, 설명
    public TMP_Text passName;
    public TMP_Text passEx;

    private void Awake()
    {
        playerController = GameObject.Find("Manager").GetComponent<PlayerController>();
        stageManager = GameObject.Find("Manager").GetComponent<StageManager>();
        stageTimeLimit = GameObject.Find("Manager").GetComponent<StageTimeLimit>();
    }

    private void Start()
    {
        selecPass = 0; // 선택한 패시브
    }

    // 패시브 선택, 설명표시
    public void powerUP()
    {
        selecPass = 1;
        passName.text = "공격력 상승";
        passEx.text = "플레이어의 공격력이 향상됩니다.";
    }
    public void TimeUP()
    {
        selecPass = 2;
        passName.text = "시간 추가";
        passEx.text = "스테이지의 제한시간이 확장됩니다.";
    }
    public void fullHealth()
    {
        selecPass = 3;
        passName.text = "체력회복";
        passEx.text = " 체력이 완전히 회복됩니다.";
    }
    public void GetMoney()
    {
        selecPass = 4;
        passName.text = "머니 획득";
        passEx.text = "게임 머니를 100원 획득합니다.";
    }

    void UpdateSelectPass() // 선택한 패시브의 값 업데이트
    {
        if (selecPass == 1)
        {
            playerController.damage += 5;
        }
        else if (selecPass == 2)
        {
            stageTimeLimit.stageTime += 5;
        }
        else if (selecPass == 3)
        {
            playerController.playerHealth = 8;
        }
        else if (selecPass == 4)
        {
            PlayerPrefs.SetInt("GameMoney", PlayerPrefs.GetInt("GameMoney", 0) + 100);
        }
    }

    // 패시브 선택 결정
    public void EnterPass()
    {
        UpdateSelectPass(); // 선택한 패시브의 값 업데이트

        stageManager.selectingPass = false; // 패시브 선택중이지 않음 (패시브 선택중 - 참)
        playerController.isAttacking = false; // 다시 공격 가능 (선택중 공격 금지)
        Time.timeScale = 1f; // 시간이 다시 진행 (선택중 시간 멈춤)
        selecPass = 0; // 선택한 패시브 값 초기화
        passName.text = ""; // 패시브 이름, 설명 초기화
        passEx.text = "";

        stageManager.passing = true; // 스테이지 변경
        stageManager.NextSetting();

        passMenu.SetActive(false); // UI 비활성화
    }
}
