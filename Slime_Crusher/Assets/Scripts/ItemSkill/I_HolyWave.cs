using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I_HolyWave : MonoBehaviour, I_Skill
{
    private ItemSkill itemSkill;

    public I_HolyWave(ItemSkill itemSkill)
    {
        this.itemSkill = itemSkill;
    }

    public void Execute(ItemSkill itemSkill, Vector3 targetPosition, int num)
    {
        if (itemSkill.isHolyWave)
        {
            AudioManager.Instance.PlayHolyWaveAudio();

            itemSkill.WaveInstance = Instantiate(itemSkill.holyWaveEffect, itemSkill.holyWavePos.position, Quaternion.identity);
            itemSkill.WaveInstance.name = "PlayerSkill";
            itemSkill.holyWaving = true;

            if (itemSkill.WaveInstance != null)
            {
                itemSkill.ExecuteHolyWave();
            }
        }
    }
}
