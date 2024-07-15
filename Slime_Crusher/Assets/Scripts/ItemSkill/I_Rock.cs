using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I_Rock : MonoBehaviour, I_Skill
{
    private ItemSkill itemSkill;

    public I_Rock(ItemSkill itemSkill)
    {
        this.itemSkill = itemSkill;
    }

    public void Execute(ItemSkill itemSkill, Vector3 targetPosition, int num)
    {
        if (itemSkill.isRock)
        {
            AudioManager.Instance.PlayRockAudio();

            GameObject rockInstance = Instantiate(itemSkill.rockEffect, targetPosition, Quaternion.identity);
            Destroy(rockInstance, 5f);
        }
    }
}
