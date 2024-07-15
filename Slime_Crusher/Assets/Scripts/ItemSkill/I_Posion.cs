using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I_Posion : MonoBehaviour, I_Skill
{
    private ItemSkill itemSkill;

    public I_Posion(ItemSkill itemSkill)
    {
        this.itemSkill = itemSkill;
    }

    public void Execute(ItemSkill itemSkill, Vector3 targetPosition, int num)
    {
        if (itemSkill.isPosion)
        {
            AudioManager.Instance.PlayPoisonAudio();

            GameObject posionInstance = Instantiate(itemSkill.posionEffect, targetPosition, Quaternion.identity);
            Destroy(posionInstance, 5f);
        }
    }
}
