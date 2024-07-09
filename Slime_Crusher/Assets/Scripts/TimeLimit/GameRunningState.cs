using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRunningState : StageState
{
    public void UpdateTime(StageTimeLimit stageTimeLimit)
    {
        if (!stageTimeLimit.stageManager.selectingPass)
        {
            // 시간 관리
            if (stageTimeLimit.stageFail > stageTimeLimit.stageTime)
            {
                stageTimeLimit.stageFail = 0.0f;
                stageTimeLimit.timeImage.fillAmount = 0.0f;
            }
            else
            {
                stageTimeLimit.stageFail += Time.deltaTime;
                stageTimeLimit.timeImage.fillAmount = 1.0f - (Mathf.Lerp(0, 100, stageTimeLimit.stageFail / stageTimeLimit.stageTime) / 100);
            }
        }
    }
}
