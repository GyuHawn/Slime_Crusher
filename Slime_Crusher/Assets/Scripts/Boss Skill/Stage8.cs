using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Stage8 : MonoBehaviour
{
    private MonsterController monsterController;
    private MonsterSpwan monsterSpwan;
    private PlayerController playerController;
    private StageManager stageManager;

    public int randomSkillIndex;

    // Skill 1 
    public GameObject bossEffect1;

    // Skill 2
    public GameObject bossEffect2;
    public GameObject subBoss;
    public float onSkillHealth;
    private bool onSkill = false;
    public GameObject skillPos2;

    // Skill 3
    private GameObject[] spwanMonster;
    public GameObject bossEffect3;

    // Skill 4
    public GameObject bossEffect4;

    // Skill 5
    public GameObject bossEffect5;

    // Skill 6
    public GameObject bossEffect6;
    public GameObject skillPos6;

    // Skill 7
    public GameObject bossEffect7;
    public List<GameObject> skills;
    public GameObject pos;
    public Vector3 boxSize;

    private void Awake()
    {
        monsterController = gameObject.GetComponent<MonsterController>();
        monsterSpwan = GameObject.Find("Manager").GetComponent<MonsterSpwan>();
        playerController = GameObject.Find("Manager").GetComponent<PlayerController>();
        stageManager = GameObject.Find("Manager").GetComponent<StageManager>();
    }

    void Start()
    {
        randomSkillIndex = Random.Range(1, 7);

        // Skill 2
        skillPos2 = GameObject.Find("Stage2BossSkill");

        // Skill 3
        if (spwanMonster == null)
        {
            spwanMonster = monsterSpwan.InfiniteMonsters.Take(5).ToArray();
        }

        // Skill 6
        skillPos6 = GameObject.Find("BossSkillPos");

        // Skill 7
        pos = GameObject.Find("Stage7 SkillPos");
        boxSize = new Vector3(-13f, 5.5f, 0);
        if (randomSkillIndex == 1)
        {
            InvokeRepeating("Stage1BossSkill", 5f, 10f);
        }
        else if (randomSkillIndex == 3)
        {
            InvokeRepeating("stage3BossSkill", 5f, 10f);
        }
        else if (randomSkillIndex == 4)
        {
            InvokeRepeating("Stage4BossSkill", 5f, 10f);
        }
        else if (randomSkillIndex == 5)
        {
            InvokeRepeating("Stage5bossSkill", 5f, 10f);
        }
        else if (randomSkillIndex == 6)
        {
            StartCoroutine(SpawnBossSkill());
        }
        else if (randomSkillIndex == 7)
        {
            InvokeRepeating("Stage7BossSkill", 5f, 10f);
        }
    }

    private void Update()
    {
        if (randomSkillIndex == 2)
        {
            // Skill 2
            if (monsterController.currentHealth <= onSkillHealth && !onSkill)
            {
                Stage2bossSkill();
            }
        }        
    }

    void Stage1BossSkill()
    {
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");

        foreach (GameObject monster in monsters)
        {
            MonsterController monsterController = monster.GetComponent<MonsterController>();
            Vector3 skillPos = new Vector3(monster.transform.position.x, monster.transform.position.y, monster.transform.position.z - 8);
            GameObject skill = Instantiate(bossEffect1, monster.transform.position, Quaternion.identity);
            skill.name = "BossSkill";
            monsterController.boss1Defending = true;

            Destroy(skill, 3f);
            StartCoroutine(EndDefending(monsterController, 3f));
        }
    }

    IEnumerator EndDefending(MonsterController monsterController, float time)
    {
        yield return new WaitForSeconds(time);
        monsterController.boss1Defending = false;
    }

    // Skill 2
    public void Stage2bossSkill()
    {
        onSkill = true;
        GameObject skill = Instantiate(bossEffect2, skillPos2.transform.position, Quaternion.identity);
        GameObject subInstantiate = Instantiate(subBoss, skillPos2.transform.position, Quaternion.identity);
        monsterSpwan.spawnedMonsters.Add(subInstantiate);
    }

    // Skill 3
    void stage3BossSkill()
    {
        StartCoroutine(SpwanMonster());
    }

    IEnumerator SpwanMonster()
    {
        for (int i = 0; i < 5; i++)
        {
            int randomIndex = Random.Range(0, monsterSpwan.bossStageSpawnPoints.Length);

            Vector3 randomPosition = monsterSpwan.GetRandomPosition(monsterSpwan.bossStageSpawnPoints[randomIndex].transform.position);
            Vector3 bossEffectPosition = new Vector3(randomPosition.x, randomPosition.y - 0.3f, randomPosition.z);

            GameObject skill = Instantiate(bossEffect3, bossEffectPosition, Quaternion.Euler(-90, 0, 0));
            skill.name = "BossSkill";
            yield return new WaitForSeconds(0.2f);

            GameObject spawnedMonster = Instantiate(spwanMonster[i], randomPosition, Quaternion.identity);
            monsterSpwan.spawnedMonsters.Add(spawnedMonster);
        }
    }

    // Skill 4
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
            GameObject skill = Instantiate(bossEffect4, randomPosition, Quaternion.identity);
            skill.name = "BossSkill";

            yield return new WaitForSeconds(0.5f);
        }
    }

    // Skill 5
    public void Stage5bossSkill()
    {
        StartCoroutine(PlayerDebuff());
    }

    IEnumerator PlayerDebuff()
    {
        GameObject skill = Instantiate(bossEffect5, gameObject.transform.position, Quaternion.identity);
        skill.name = "BossSkill";
        playerController.stage5Debuff = true;

        yield return new WaitForSeconds(5f);

        Destroy(skill);
        playerController.stage5Debuff = false;
    }

    // Skill 6
    IEnumerator SpawnBossSkill()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            Vector3 randomPosition = new Vector3(
                Random.Range(skillPos6.transform.position.x - 4f, skillPos6.transform.position.x + 4f),
                Random.Range(skillPos6.transform.position.y - 1f, skillPos6.transform.position.y + 2.5f),
                skillPos6.transform.position.z - 5
            );

            GameObject skill = Instantiate(bossEffect6, randomPosition, Quaternion.identity);
            skill.name = "BossSkill";

            yield return new WaitForSeconds(3f);

            Destroy(skill);
        }
    }

    // Skill 7
    void Stage7BossSkill()
    {
        StartCoroutine(Skill7());
    }

    IEnumerator Skill7()
    {
        for (int i = 0; i < 10; i++)
        {
            Vector3 randomPosition = new Vector3(
                Random.Range(pos.transform.position.x - boxSize.x / 2, pos.transform.position.x + boxSize.x / 2),
                Random.Range(pos.transform.position.y - boxSize.y / 2, pos.transform.position.y + boxSize.y / 2),
                pos.transform.position.z
            );

            GameObject skill = Instantiate(bossEffect7, randomPosition, Quaternion.identity);
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
