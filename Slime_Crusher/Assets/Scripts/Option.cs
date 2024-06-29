using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Option : MonoBehaviour
{
    public GameObject settingMenu; // 옵션 버튼

    public float moveDuration = 1.0f; // 옵션창 이동시간
    private Vector3 startMenuPos; // 옵션창 위치관리
    private Vector3 endMenuPos; // 옵션창 위치관리

    private bool onSetting; // 옵션 활성화
    
    private void Start()
    {
        SettingMovePosition(); // 옵션 UI 위치값 설정

        // 기초 값 설정
        onSetting = false;
    }
    void SettingMovePosition() // 옵션 UI 위치값 설정
    {
        startMenuPos = new Vector3(870f, settingMenu.transform.localPosition.y, settingMenu.transform.localPosition.z);
        endMenuPos = new Vector3(540f, settingMenu.transform.localPosition.y, settingMenu.transform.localPosition.z);
    }

    public void OnSettingMenu() // 옵션메뉴 활성화
    {
        StartCoroutine(MoveSettingMenu()); // 옵션메뉴 이동
    }

    IEnumerator MoveSettingMenu() // 옵션메뉴 이동
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

    public void MainMenu() // 메인메뉴로 이동
    {
        LodingController.LoadNextScene("MainMenu");
    }
}
