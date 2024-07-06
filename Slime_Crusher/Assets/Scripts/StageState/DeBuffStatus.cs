using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDownStatus : StageStatus
{   
    private int saveDamage;
    private bool isDamageDown;
    public void Apply(StageStatusManager stageStatusManager)
    {
        stageStatusManager.buffText.text = "플레이어의 기본 공격력이 감소합니다.";

        isDamageDown = true;
        saveDamage = stageStatusManager.playerController.damage;
        stageStatusManager.playerController.damage -= (int)(stageStatusManager.playerController.damage * 0.5f);
    }

    public void Reset(StageStatusManager stageStatusManager)
    {
        isDamageDown = true;
        stageStatusManager.playerController.damage = saveDamage;
    }
}
public class MonsterHealthUPStatus : StageStatus
{
    private bool isMonsterHealthUP;
    public void Apply(StageStatusManager stageStatusManager)
    {
        stageStatusManager.buffText.text = "몬스터의 기본 체력이 증가합니다.";

        isMonsterHealthUP = true;
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");

        foreach (GameObject monster in monsters)
        {
            MonsterController monsterController = monster.GetComponent<MonsterController>();
            monsterController.currentHealth = (monsterController.currentHealth * 1.5f);
        }
    }

    public void Reset(StageStatusManager stageStatusManager){}
}
public class TimeDownStatus : StageStatus
{
    private bool isTimeDown;
    public void Apply(StageStatusManager stageStatusManager)
    {
        stageStatusManager.buffText.text = "스테이지 제한 시간이 감소합니다.";

        isTimeDown = true;
        stageStatusManager.stageTimeLimit.stageTime -= 10;
    }

    public void Reset(StageStatusManager stageStatusManager)
    {
        isTimeDown = true;
        stageStatusManager.stageTimeLimit.stageTime += 10;
    }
}
public class PercentDownStatus : StageStatus
{
    private bool isPercentDown;
    public void Apply(StageStatusManager stageStatusManager)
    {
        stageStatusManager.buffText.text = "아이템 발동 확률이 감소합니다.";

        isPercentDown = true;

        stageStatusManager.itemSkill.firePercent -= 5f;
        stageStatusManager.itemSkill.fireShotPercent -= 5f;
        stageStatusManager.itemSkill.holyShotPercent -= 5f;
        stageStatusManager.itemSkill.holyWavePercent -= 5f;
        stageStatusManager.itemSkill.rockPercent -= 5f;
        stageStatusManager.itemSkill.posionPercent -= 5f;
        stageStatusManager.itemSkill.meleePercent -= 5f;
        stageStatusManager.itemSkill.sturnPercent -= 5f;
    }

    public void Reset(StageStatusManager stageStatusManager)
    {
        isPercentDown = true;

        stageStatusManager.itemSkill.firePercent += 5f;
        stageStatusManager.itemSkill.fireShotPercent += 5f;
        stageStatusManager.itemSkill.holyShotPercent += 5f;
        stageStatusManager.itemSkill.holyWavePercent += 5f;
        stageStatusManager.itemSkill.rockPercent += 5f;
        stageStatusManager.itemSkill.posionPercent += 5f;
        stageStatusManager.itemSkill.meleePercent += 5f;
        stageStatusManager.itemSkill.sturnPercent += 5f;
    }
}
public class MonsterAttackSpdUpStatus : StageStatus
{
    private bool isMonsterAttackSpdUp;
    public void Apply(StageStatusManager stageStatusManager)
    {
        stageStatusManager.buffText.text = "투사체 속도가 증가합니다.";

        isMonsterAttackSpdUp = true;
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");

        foreach (GameObject monster in monsters)
        {
            MonoBehaviour[] scripts = monster.GetComponents<MonoBehaviour>();

            foreach (MonoBehaviour script in scripts)
            {
                if (script.GetType().Name.Contains("Stage"))
                {
                    System.Reflection.FieldInfo field = script.GetType().GetField("bulletSpd");

                    if (field != null)
                    {
                        field.SetValue(script, (float)field.GetValue(script) + 1);
                    }
                }
            }
        }
    }

    public void Reset(StageStatusManager stageStatusManager){}
}
public class MonsterSizeDownStatus : StageStatus
{
    private bool isMonsterSizeDown;
    public void Apply(StageStatusManager stageStatusManager)
    {
        stageStatusManager.buffText.text = "몬스터의 사이즈가 감소합니다.";

        isMonsterSizeDown = true;
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");

        foreach (GameObject monster in monsters)
        {
            Transform monsterTransform = monster.transform;
            Vector3 newScale = monsterTransform.localScale - new Vector3(0.05f, 0.05f, 0f);
            monsterTransform.localScale = newScale;
        }
    }

    public void Reset(StageStatusManager stageStatusManager){}
}