using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Option : MonoBehaviour
{
    public GameObject settingMenu; // 옵션 버튼
    public float moveDuration = 1.0f; // 옵션창 이동시간
    private Vector3 startMenuPos; // 옵션창 위치관리
    private Vector3 endMenuPos;
    private bool onSetting; // 옵션 활성화

    private void Start()
    {
        onSetting = false;

        startMenuPos = new Vector3(950f, settingMenu.transform.localPosition.y, settingMenu.transform.localPosition.z);
        endMenuPos = new Vector3(350f, settingMenu.transform.localPosition.y, settingMenu.transform.localPosition.z);
    }
    public void OnSettingMenu()
    {
        StartCoroutine(MoveSettingMenu());
    }

    IEnumerator MoveSettingMenu()
    {
        // 옵션창 비활성화시 처음 위치로 이동
        if (!onSetting)
        {
            float elapsed = 0f;

            while (elapsed < moveDuration)
            {
                settingMenu.transform.localPosition = Vector3.Lerp(startMenuPos, endMenuPos, elapsed / moveDuration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            settingMenu.transform.localPosition = endMenuPos;
            onSetting = true;
        }
        else // 옵션창 활성화시 정해진 위치로 이동
        {
            float elapsed = 0f;

            while (elapsed < moveDuration)
            {
                settingMenu.transform.localPosition = Vector3.Lerp(endMenuPos, startMenuPos, elapsed / moveDuration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            settingMenu.transform.localPosition = startMenuPos;
            onSetting = false;
        }

    }

    public void MainMenu()
    {
        LodingController.LoadNextScene("MainMenu");
    }
}
