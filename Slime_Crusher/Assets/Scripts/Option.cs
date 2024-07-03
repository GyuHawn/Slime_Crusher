using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Option : MonoBehaviour
{
    public Button settingButton;
    public GameObject settingMenu; // 옵션 버튼

    /*public float moveDuration = 1.0f; // 옵션창 이동시간
    private Vector3 startMenuPos; // 옵션창 위치관리
    private Vector3 endMenuPos; // 옵션창 위치관리
    private bool onSetting; // 옵션 활성화*/

    private void Start()
    {
        SettingMenuManager.Instance.InitializeOptionMenu(settingMenu);
        settingButton.onClick.AddListener(OnSettingMenu);
    }
    void OnSettingMenu()
    {
        SettingMenuManager.Instance.ToggleSettingMenu();
    }

    public void MainMenu() // 메인메뉴로 이동
    {
        SceneManager.LoadScene("MainMenu");
        //LodingController.LoadNextScene("MainMenu");
    }
}
