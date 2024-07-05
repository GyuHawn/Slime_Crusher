using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpCharacterCommand : Command
{
    private CharacterSelect characterSelect;
    private int cost;
    private int level;
    private string levelKey;
    private int newLevel;

    public LevelUpCharacterCommand(CharacterSelect characterSelect, int cost, ref int level, string levelKey)
    {
        this.characterSelect = characterSelect;
        this.cost = cost;
        this.level = level;
        this.levelKey = levelKey;
        this.newLevel = level; // 초기 레벨 값을 저장
    }

    public void Execute()
    {
        if (level < 20 && characterSelect.playerMoney >= cost)
        {
            AudioManager.Instance.PlayButtonAudio();
            newLevel++;
            characterSelect.playerMoney -= cost;
            PlayerPrefs.SetInt(levelKey, newLevel);
            PlayerPrefs.SetInt("GameMoney", characterSelect.playerMoney);

            // characterSelect의 레벨 값을 업데이트
            if (levelKey == "rockLevel") characterSelect.rockLevel = newLevel;
            else if (levelKey == "waterLevel") characterSelect.waterLevel = newLevel;
            else if (levelKey == "lightLevel") characterSelect.lightLevel = newLevel;
            else if (levelKey == "luckLevel") characterSelect.luckLevel = newLevel;
        }
    }
}
