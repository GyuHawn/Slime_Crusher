using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpCharacterCommand : Command
{
    private CharacterSelect characterSelect;
    private int cost;
    private int level;
    private string levelKey;

    public LevelUpCharacterCommand(CharacterSelect characterSelect, int cost, ref int level, string levelKey)
    {
        this.characterSelect = characterSelect;
        this.cost = cost;
        this.level = level;
        this.levelKey = levelKey;
    }

    public void Execute()
    {
        if (level < 20 && characterSelect.playerMoney >= cost)
        {
            AudioManager.Instance.PlayButtonAudio();
            level++;
            characterSelect.playerMoney -= cost;
            PlayerPrefs.SetInt(levelKey, level);
            PlayerPrefs.SetInt("GameMoney", characterSelect.playerMoney);
        }
    }
}
