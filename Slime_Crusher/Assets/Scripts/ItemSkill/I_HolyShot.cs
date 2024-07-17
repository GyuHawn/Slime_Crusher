using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I_HolyShot : MonoBehaviour, I_Skill
{
    private ItemSkill itemSkill;

    public I_HolyShot(ItemSkill itemSkill)
    {
        this.itemSkill = itemSkill;
    }

    public void Execute(ItemSkill itemSkill, Vector3 targetPosition, int num)
    {
        if (itemSkill.isHolyShot)
        {
            AudioManager.Instance.PlayHolyShotAudio();

            itemSkill.holyShotInstance = Instantiate(itemSkill.holyShotEffect, targetPosition, Quaternion.identity);
            itemSkill.holyShotInstance.name = "PlayerSkill";
            Destroy(itemSkill.holyShotInstance, itemSkill.holyShotDuration);

            if (itemSkill.holyShotInstance != null)
            {
                itemSkill.ExecuteHolyShot();
            }
        }
    }
    
}
