using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawn : MonoBehaviour
{
    private StageManager stageManager;
    private MonsterFactoryManager factoryManager;

    public GameObject[] stage1Monsters;
    public GameObject[] stage2Monsters;
    public GameObject[] stage3Monsters;
    public GameObject[] stage4Monsters;
    public GameObject[] stage5Monsters;
    public GameObject[] stage6Monsters;
    public GameObject[] stage7Monsters;
    public GameObject[] InfiniteMonsters;

    public GameObject[] monsterSpawnPoints;
    public GameObject[] bossStageSpawnPoints;

    public List<GameObject> spawnedMonsters = new List<GameObject>();
    public Transform pos;

    private void Awake()
    {
        stageManager = GameObject.Find("Manager").GetComponent<StageManager>();

        // MonsterFactoryManager 인스턴스 생성 시에 필요한 몬스터 배열들을 전달합니다.
        factoryManager = new MonsterFactoryManager(stage1Monsters, stage2Monsters, stage3Monsters, stage4Monsters,
                                                  stage5Monsters, stage6Monsters, stage7Monsters, InfiniteMonsters);
    }

    public void MonsterInstantiate(int base0Count, int base1Count, int base2Count, int base3Count, int bossCount)
    {
        MonsterFactory factory = factoryManager.GetFactory(stageManager.mainStage);

        // 보스 스테이지 O
        if (stageManager.subStage == 5)
        {
            for (int i = 0; i < base0Count; i++)
            {
                InstantiateRandom(factory.CreateMonster(0), bossStageSpawnPoints);
            }
            for (int i = 0; i < base1Count; i++)
            {
                InstantiateRandom(factory.CreateMonster(1), bossStageSpawnPoints);
            }
            for (int i = 0; i < base2Count; i++)
            {
                InstantiateRandom(factory.CreateMonster(2), bossStageSpawnPoints);
            }
            for (int i = 0; i < base3Count; i++)
            {
                InstantiateRandom(factory.CreateMonster(3), bossStageSpawnPoints);
            }
            if (bossCount > 0)
            {
                Vector3 bossPos = new Vector3(pos.position.x, pos.position.y, pos.position.z - 5);
                GameObject bossMonster = Instantiate(factory.CreateMonster(4), bossPos, Quaternion.identity);
                spawnedMonsters.Add(bossMonster);
            }
        }
        else // 보스 스테이지 X
        {
            for (int i = 0; i < base0Count; i++)
            {
                InstantiateRandom(factory.CreateMonster(0), monsterSpawnPoints);
            }
            for (int i = 0; i < base1Count; i++)
            {
                InstantiateRandom(factory.CreateMonster(1), monsterSpawnPoints);
            }
            for (int i = 0; i < base2Count; i++)
            {
                InstantiateRandom(factory.CreateMonster(2), monsterSpawnPoints);
            }
            for (int i = 0; i < base3Count; i++)
            {
                InstantiateRandom(factory.CreateMonster(3), monsterSpawnPoints);
            }
            if (stageManager.mainStage > 7 && bossCount > 0)
            {
                Vector3 bossPos = new Vector3(pos.position.x, pos.position.y, pos.position.z - 5);
                GameObject bossMonster = Instantiate(factory.CreateMonster(6), bossPos, Quaternion.identity);
                spawnedMonsters.Add(bossMonster);
            }
        }
    }

    public void InstantiateRandom(GameObject monsterPrefab, GameObject[] spawnPoints)
    {
        int randomIndex = Random.Range(0, spawnPoints.Length);
        Vector3 randomPosition = GetRandomPosition(spawnPoints[randomIndex].transform.position);
        GameObject spawnedMonster = Instantiate(monsterPrefab, randomPosition, Quaternion.identity);
        spawnedMonsters.Add(spawnedMonster);
    }

    public Vector3 GetRandomPosition(Vector3 center)
    {
        float radius = 1.0f;
        float x = Random.Range(center.x - radius, center.x + radius);
        float y = Random.Range(center.y - radius, center.y + radius);
        float z = -5;

        return new Vector3(x, y, z);
    }

    public void RemoveMonsterFromList(GameObject monster)
    {
        spawnedMonsters.Remove(monster);
        if (spawnedMonsters.Count == 0)
        {
            stageManager.NextStage();
        }
    }
}
