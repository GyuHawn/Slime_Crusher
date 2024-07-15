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

            GameObject holyShotInstance = Instantiate(itemSkill.holyShotEffect, targetPosition, Quaternion.identity);
            holyShotInstance.name = "PlayerSkill";

            if (holyShotInstance != null)
            {
                StartCoroutine(RotateHolyShot(holyShotInstance, 5f));
            }
        }
    }
    private IEnumerator RotateHolyShot(GameObject holyShotInstance, float duration)
    {
        if (holyShotInstance == null)
        {
            yield break;
        }

        float elapsedTime = 0f;
        float rotationSpd = 360f / duration;

        while (elapsedTime < duration)
        {
            if (holyShotInstance == null)
            {
                yield break;
            }

            holyShotInstance.transform.Rotate(rotationSpd * Time.deltaTime, 0f, 0f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
