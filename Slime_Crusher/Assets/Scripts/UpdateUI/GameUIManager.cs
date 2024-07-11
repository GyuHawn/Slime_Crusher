using UnityEngine;
using TMPro;
using System.Collections;

public class GameUIManager : MonoBehaviour, Observer
{
    public TMP_Text gameTimeText; // 진행시간 텍스트
    public TMP_Text comboText; // 콤보 텍스트

    public int comboNum; // 현재 콤보수
    public int maxComboNum; // 최대 콤보수
    public float maxScale; // 크기 증가
    public float minScale; // 크기 감소
    public float scaleSpeed; // 증감 속도

    private Coroutine scaleCoroutine;

    void Start()
    {
        // 텍스트 크기 및 콤보수 초기화
        maxScale = 70f;
        minScale = 50f;
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

    public void UpdateTime(float gameTime)
    {
        gameTimeText.text = string.Format("{0:00}:{1:00}", Mathf.Floor(gameTime / 60), gameTime % 60);
    }
    public void UpdateCombo(int comboNum, int maxComboNum)
    {
        this.comboNum = comboNum;
        this.maxComboNum = maxComboNum;

        if (scaleCoroutine != null)
        {
            StopCoroutine(scaleCoroutine);
        }
        scaleCoroutine = StartCoroutine(ScaleComboText());

        if (comboNum <= 0)
        {
            comboText.gameObject.SetActive(false);
        }
        else
        {
            comboText.gameObject.SetActive(true);
        }

        comboText.text = "x " + comboNum.ToString();
    }

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
