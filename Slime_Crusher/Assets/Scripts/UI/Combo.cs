using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Combo : MonoBehaviour
{
    public int comboNum; // 현재 콤보수
    public int maxComboNum; // 최대 콤보수

    // 콤보수 변경시 텍스트 크기 관리
    public TMP_Text comboText; // 콤보 텍스트
    public float maxScale; // 크기 증가
    public float minScale; // 크기 감소
    public float scaleSpeed; // 증감 속도

    void Start()
    {
        // 텍스트 크기 및 콤보수 초기화
        maxScale = 100f;
        minScale = 55f;
        scaleSpeed = 400f;
        maxComboNum = 0;

        comboText.gameObject.SetActive(false); // 콤보 텍스트 활성화
    }

    void Update()
    {
        comboText.text = "x " + comboNum.ToString(); // 콤보수 변경

        ComboActivate(); // 콤보수 0일시 비활성화
    }
    void ComboActivate() // 콤보수 0일시 비활성화
    {
        if (comboNum <= 0)
        {
            comboText.gameObject.SetActive(false);
        }
        else
        {
            comboText.gameObject.SetActive(true);
        }
    }


    public void ComboUp() // 콤보 증가
    {
        comboNum++; // 콤보 값 증가

        StartCoroutine(ScaleComboText()); // 콤보수 변경시 텍스트의 크기가 키웠다가 줄어듬

        // 최대 콤보수 관리
        if (comboNum > maxComboNum)
        {
            maxComboNum = comboNum;
        }
    }

    // 콤보수 변경시 텍스트의 크기가 키웠다가 줄어듬
    IEnumerator ScaleComboText()
    {
        float currentScale = comboText.fontSize;

        while (currentScale < maxScale)
        {
            currentScale += scaleSpeed * Time.deltaTime;
            comboText.fontSize = Mathf.Min(currentScale, maxScale);
            yield return null;
        }

        while (currentScale > minScale)
        {
            currentScale -= scaleSpeed * Time.deltaTime;
            comboText.fontSize = Mathf.Max(currentScale, minScale);
            yield return null;
        }
    }
}
