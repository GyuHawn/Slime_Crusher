using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Stage3 : MonoBehaviour
{
    private MonsterSpwan monsterSpwan;
    
    private GameObject[] spwanMonster;

    public GameObject bossEffect;

    private void Awake()
    {
        monsterSpwan = GameObject.Find("Manager").GetComponent<MonsterSpwan>();
    }
   
    void Start()
    {
        if(spwanMonster == null)
        {
            spwanMonster = monsterSpwan.stage3Monsters.Take(4).ToArray();
        }

        InvokeRepeating("stage3BossSkill", 5f, 10f);
    }
    
    void Update()
    {
        
    }

    void stage3BossSkill()
    {
        StartCoroutine(SpwanMonster());
    }

    IEnumerator SpwanMonster()
    {
        for (int i = 0; i < 3; i++)
        {
            int randomIndex = Random.Range(0, monsterSpwan.bossStageSpawnPoints.Length);

            Vector3 randomPosition = monsterSpwan.GetRandomPosition(monsterSpwan.bossStageSpawnPoints[randomIndex].transform.position);
            Vector3 bossEffectPosition = new Vector3(randomPosition.x, randomPosition.y - 0.3f, randomPosition.z);

            GameObject skill = Instantiate(bossEffect, bossEffectPosition, Quaternion.Euler(-90, 0, 0));
            skill.name = "BossSkill";
            yield return new WaitForSeconds(0.2f);

            GameObject spawnedMonster = Instantiate(spwanMonster[i], randomPosition, Quaternion.identity);
            monsterSpwan.spawnedMonsters.Add(spawnedMonster);
        }
    }
}
