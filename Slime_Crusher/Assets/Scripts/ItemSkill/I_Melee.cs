using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I_Melee : MonoBehaviour, I_Skill
{
    private ItemSkill itemSkill;

    public I_Melee(ItemSkill itemSkill)
    {
        this.itemSkill = itemSkill;
    }

    public void Execute(ItemSkill itemSkill, Vector3 targetPosition, int num)
    {
        if (itemSkill.isMelee)
        {
            StartCoroutine(MeleeInstantiate(itemSkill, targetPosition, num));
        }
    }
    IEnumerator MeleeInstantiate(ItemSkill itemSkill, Vector3 targetPosition, int numEffects)
    {
        for (int i = 0; i < numEffects; i++)
        {
            AudioManager.Instance.PlayMeleeAudio();

            Vector3 randomOffset = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);
            Vector3 spawnPosition = targetPosition + randomOffset;

            GameObject meleeInstance = Instantiate(itemSkill.meleeEffect, spawnPosition, Quaternion.identity);
            meleeInstance.name = "PlayerSkill";
            Destroy(meleeInstance, 0.2f);

            yield return new WaitForSeconds(0.1f);
        }
    }
}
