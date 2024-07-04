using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage4 : MonoBehaviour
{
    public GameObject bossEffect;

    void Start()
    {
        InvokeRepeating("Stage4BossSkill", 5f, 10f);
    }

    void Stage4BossSkill()
    {
        StartCoroutine(Skill());   
    }

    IEnumerator Skill()
    {
        for (int i = 0; i < 5; i++)
        {
            Vector3 randomPosition = new Vector3(
                Random.Range(gameObject.transform.position.x - 4f, gameObject.transform.position.x + 4f),
                Random.Range(gameObject.transform.position.y - 1f, gameObject.transform.position.y + 3f),
                gameObject.transform.position.z
            );
            GameObject skill = Instantiate(bossEffect, randomPosition, Quaternion.identity);
            skill.name = "BossSkill";
            yield return new WaitForSeconds(0.5f);
        }
    }
}
