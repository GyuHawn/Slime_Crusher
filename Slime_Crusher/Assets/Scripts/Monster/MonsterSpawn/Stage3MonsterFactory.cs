using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage3MonsterFactory : MonsterFactory
{
    private GameObject[] stageMonsters;

    public Stage3MonsterFactory(GameObject[] stageMonsters)
    {
        this.stageMonsters = stageMonsters;
    }

    public GameObject CreateMonster(int monsterType)
    {
        if (monsterType >= 0 && monsterType < stageMonsters.Length)
        {
            return stageMonsters[monsterType];
        }
        else
        {
            return null;
        }
    }
}
