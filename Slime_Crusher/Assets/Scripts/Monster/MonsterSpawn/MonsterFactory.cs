using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface MonsterFactory
{
    GameObject CreateMonster(int monsterType);
}
