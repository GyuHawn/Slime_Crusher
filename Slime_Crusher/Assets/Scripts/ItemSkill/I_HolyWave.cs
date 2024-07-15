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

            Destroy(itemSkill.WaveInstance, itemSkill.holyWaveDuration);
            StartCoroutine(DestroyWave(itemSkill));
        }
    }

    IEnumerator DestroyWave(ItemSkill itemSkill)
    {
        yield return new WaitForSeconds(itemSkill.holyWaveDuration);
        Destroy(itemSkill.WaveInstance);
        itemSkill.holyWaving = false;
    }
}
