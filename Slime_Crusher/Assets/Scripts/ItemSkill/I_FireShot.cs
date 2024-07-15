using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class I_FireShot : MonoBehaviour, I_Skill
{
    private ItemSkill itemSkill;

    public I_FireShot(ItemSkill itemSkill)
    {
        this.itemSkill = itemSkill;
    }

    public void Execute(ItemSkill itemSkill, Vector3 targetPosition, int num)
    {
        if (itemSkill.isFireShot)
        {
            AudioManager.Instance.PlayFireShotAudio();

            GameObject fireShotInstance = Instantiate(itemSkill.fireShotEffect, targetPosition, Quaternion.identity);
            List<GameObject> sub = new List<GameObject>();

            for (int i = 0; i < itemSkill.fireShotSubNum; i++)
            {
                GameObject subShot = Instantiate(itemSkill.fireShotSub, targetPosition, Quaternion.identity);
                subShot.name = "PlayerSkill";
                Vector2 randomDirection = Random.insideUnitCircle.normalized;
                subShot.GetComponent<Rigidbody2D>().velocity = randomDirection * 5f;

                sub.Add(subShot);
            }

            StartCoroutine(DestroySubShots(sub, 3f));
            Destroy(fireShotInstance, 1f);
        }
    }
    private IEnumerator DestroySubShots(List<GameObject> subShots, float delay)
    {
        yield return new WaitForSeconds(delay);

        foreach (GameObject subShot in subShots)
        {
            Destroy(subShot);
        }
    }
}
