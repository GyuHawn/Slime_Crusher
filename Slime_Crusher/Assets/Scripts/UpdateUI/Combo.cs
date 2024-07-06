using TMPro;
using UnityEngine;

public class Combo : MonoBehaviour
{
    private int comboNum;
    private int maxComboNum;

    public TMP_Text comboText;

    void Start()
    {
        comboText.gameObject.SetActive(false);
    }

    public void ComboUp()
    {
        comboNum++;

        if (comboNum > maxComboNum)
        {
            maxComboNum = comboNum;
        }

        // 옵저버에게 콤보 업데이트 알림
        NotifyObservers();
    }

    void NotifyObservers()
    {
        GameUIManager gameUIManager = FindObjectOfType<GameUIManager>();
        if (gameUIManager != null)
        {
            gameUIManager.UpdateCombo(comboNum, maxComboNum);
        }
    }
}
