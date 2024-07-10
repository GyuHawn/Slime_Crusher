using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StageChange : MonoBehaviour
{
    private StageManager stageManager;

    public GameObject nextStageUI;

    public int mainStage;
    public int subStage;

    public TMP_Text mainStageText;
    public TMP_Text subStageText;

    private void Awake()
    {
        if (!stageManager)
            stageManager = FindObjectOfType<StageManager>();
    }

    private void Start()
    {
        mainStage = stageManager.mainStage;
        subStage = stageManager.subStage;
    }

    private void Update()
    {
        if(stageManager.mainStage >= 8)
        {
            subStage = 1;
        }
    }

    public void ChangeStageText()
    {
        StartCoroutine(ChangeText());
    }

    IEnumerator ChangeText()
    {
        nextStageUI.SetActive(true);
        yield return new WaitForSeconds(2f);

        mainStage = stageManager.mainStage;
        subStage = stageManager.subStage;

        mainStageText.text = mainStage.ToString();
        subStageText.text = subStage.ToString();

        yield return new WaitForSeconds(1f);
        nextStageUI.SetActive(false);
    }
}
