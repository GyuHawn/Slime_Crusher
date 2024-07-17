using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I_Sturn : MonoBehaviour, I_Skill
{
    private ItemSkill itemSkill;

    public I_Sturn(ItemSkill itemSkill)
    {
        this.itemSkill = itemSkill;
    }

    public void Execute(ItemSkill itemSkill, Vector3 targetPosition, int num)
    {
        if (itemSkill.isSturn)
        {
            if (itemSkill.currentAttackedMonster != null)
            {
                AudioManager.Instance.PlayStunAudio();

                MonsterController monsterController = itemSkill.currentAttackedMonster.GetComponent<MonsterController>();
                if (monsterController != null)
                {
                    GameObject sturnInstance = Instantiate(itemSkill.sturnEffect, itemSkill.currentAttackedMonster.transform.position, Quaternion.identity);
                    Vector3 imagePos = new Vector3(monsterController.sturn.transform.position.x, monsterController.sturn.transform.position.y, monsterController.sturn.transform.position.z - 0.5f);
                    GameObject sturnimageInstance = Instantiate(itemSkill.sturnImage, monsterController.sturn.transform.position, Quaternion.identity);
                    sturnimageInstance.name = "PlayerSkill";

                    monsterController.stop = true;
                    monsterController.attackTime += 5;
                    itemSkill.monsterToSturnImage[itemSkill.currentAttackedMonster] = sturnimageInstance;

                    Destroy(sturnimageInstance, 2f);
                }
            }

            itemSkill.ExecuteSturnRemove();
        }
    }
}
