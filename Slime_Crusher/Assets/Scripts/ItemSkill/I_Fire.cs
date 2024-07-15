using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I_Fire : MonoBehaviour, I_Skill
{
    private ItemSkill itemSkill;

    public I_Fire(ItemSkill itemSkill)
    {
        this.itemSkill = itemSkill;
    }

    public void Execute(ItemSkill itemSkill, Vector3 targetPosition, int num)
    {
        if (itemSkill.isFire)
        {
            AudioManager.Instance.PlayFireAudio();
            Vector3 firePos = new Vector3(targetPosition.x, targetPosition.y, targetPosition.z + 3f);
            itemSkill.fireInstance = Instantiate(itemSkill.fireEffect, firePos, Quaternion.Euler(-90, 0, 0));
            itemSkill.fireInstance.name = "PlayerSkill";

            Destroy(itemSkill.fireInstance, 3f);
        }
    }
}
