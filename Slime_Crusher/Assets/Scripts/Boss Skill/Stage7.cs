using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage7 : MonoBehaviour
{
    private StageManager stageManager;

    public GameObject bossEffect;
    public List<GameObject> skills;

    public GameObject pos;
    public Vector3 boxSize;

    private void Awake()
    {
        stageManager = GameObject.Find("Manager").GetComponent<StageManager>();
    }

    void Start()
    {
        pos = GameObject.Find("Stage7 SkillPos");
        boxSize = new Vector3(-13f, 5.5f, 0);
        InvokeRepeating("Stage7BossSkill", 5f, 10f);
    }

    void Stage7BossSkill()
    {
        StartCoroutine(Skill());
    }

    IEnumerator Skill()
    {
        for (int i = 0; i < 10; i++)
        {
            Vector3 randomPosition = new Vector3(
                Random.Range(pos.transform.position.x - boxSize.x / 2, pos.transform.position.x + boxSize.x / 2),
                Random.Range(pos.transform.position.y - boxSize.y / 2, pos.transform.position.y + boxSize.y / 2),
                pos.transform.position.z
            );

            GameObject skill = Instantiate(bossEffect, randomPosition, Quaternion.identity);
            skill.name = "BossSkill";
            skills.Add(skill);
        }

        yield return new WaitForSeconds(5f);

        foreach (GameObject skill in skills)
        {
            Destroy(skill);
        }
        skills.Clear();
    }
}
