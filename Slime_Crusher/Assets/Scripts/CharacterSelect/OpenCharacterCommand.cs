using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCharacterCommand : Command
{
    private CharacterSelect characterSelect;
    private int cost;
    private string characterKey;

    private System.Action onSuccess;

    public OpenCharacterCommand(CharacterSelect characterSelect, int cost, string characterKey, System.Action onSuccess)
    {
        this.characterSelect = characterSelect;
        this.cost = cost;
        this.characterKey = characterKey;
        this.onSuccess = onSuccess;
    }

    public void Execute()
    {
        if(characterSelect.playerMoney >= cost)
        {
            AudioManager.Instance.PlayButtonAudio();
            characterSelect.playerMoney -= cost;
            PlayerPrefs.SetInt(characterKey, 1);
            PlayerPrefs.SetInt("GameMoney", characterSelect.playerMoney);
            onSuccess();
        }  
    }
}
