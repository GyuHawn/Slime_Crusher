using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageTimeLimit : MonoBehaviour
{
    public StageManager stageManager;

    public Image timeImage; // 제한시간 시각적 관리용 이미지

    public float stageTime; // 제한시간
    public float stageFail; // 경과한 시간
    public TMP_Text timeText; // 시간 텍스트

    private StageState currentState;

    private void Awake()
    {
        stageManager = GameObject.Find("Manager").GetComponent<StageManager>();
    }

    private void Start()
    {
        stageTime = 20; // 제한시간 설정
        SetState(new GameRunningState()); // 초기 상태 설정
    }

    void Update()
    {
        // 남은 시간 표시
        timeText.text = ((int)(stageTime - stageFail)).ToString();

        if (stageManager.gameStart)
        {
            currentState.UpdateTime(this); // 현재 상태에 따라 시간 업데이트
        }
    }

    public void SetState(StageState newState)
    {
        currentState = newState;
    }
}
