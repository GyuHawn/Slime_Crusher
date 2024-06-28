using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageTimeLimit : MonoBehaviour
{
    private StageManager stageManager;

    public Image timeImage; // 제한시간 시각적 관리용 이미지

    public float stageTime; // 제한시간
    public float stageFail; // 경과한 시간
    public TMP_Text timeText;

    private void Awake()
    {
        stageManager = GameObject.Find("Manager").GetComponent<StageManager>();
    }

    private void Start()
    {
        stageTime = 20; // 제한시간 설정
    }

    void Update()
    {
        // 남은 시간 표시
        timeText.text = ((int)(stageTime - stageFail)).ToString();

        if (stageManager.gameStart)
        {
            if (!stageManager.selectingPass)
            {
                // 시간 관리
                if (stageFail > stageTime)
                {
                    stageFail = 0.0f;
                    timeImage.fillAmount = 0.0f;
                }
                else
                {
                    stageFail = stageFail + Time.deltaTime;
                    timeImage.fillAmount = 1.0f - (Mathf.Lerp(0, 100, stageFail / stageTime) / 100);
                }
            }          
        }
    }
}
