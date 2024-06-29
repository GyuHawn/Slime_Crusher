using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Character : MonoBehaviour
{
    public int currentCharacter; // 현재 선택한 캐릭터
  
    // 캐릭터
    public bool rock;
    public bool water;
    public bool lihgt;
    public bool luck;

    void Start()
    {
        currentCharacter = PlayerPrefs.GetInt("SelectChar"); // 선택한 캐릭터 값 받아오기

        ResetCharacter(); // 캐릭터 초기화
    }

    void ResetCharacter() // 캐릭터 초기화
    {
        rock = false;
        water = false;
        lihgt = false;
        luck = false;
    }

    
    void Update()
    {
        SelectedCharacter(); // 선택한 캐릭터
    }

    void SelectedCharacter() // 선택한 캐릭터
    {
        if (currentCharacter == 1)
        {
            rock = true;
        }
        else if (currentCharacter == 2)
        {
            water = true;
        }
        else if (currentCharacter == 3)
        {
            lihgt = true;
        }
        else if (currentCharacter == 4)
        {
            luck = true;
        }
    }
}
