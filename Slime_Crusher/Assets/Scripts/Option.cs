using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Option : MonoBehaviour
{
    public Button settingButton;
    public GameObject settingMenu; // 옵션 버튼

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
