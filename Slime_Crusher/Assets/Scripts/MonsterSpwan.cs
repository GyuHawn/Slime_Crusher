using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpwan : MonoBehaviour
{
    private StageManager stagerManage;

    private int stage; // 현재 스테이지

    // sub == 1 -> [0] / sub == 2 -> [0,1] / sub == 3 -> [0,1,2] / sub == 4 -> [0,1,2,3] / sub == 5 -> [0,1,2,3,4(Boss)] 
    public GameObject[] stage1Monsters; // 스테이지 별 몬스터
    public GameObject[] stage2Monsters;
    public GameObject[] stage3Monsters;
    public GameObject[] stage4Monsters;
    public GameObject[] stage5Monsters;
    public GameObject[] stage6Monsters;
    public GameObject[] stage7Monsters;
    public GameObject[] InfiniteMonsters;

    public GameObject[] monsterSpawnPoints; // 몬스터 소환 위치
    public GameObject[] bossStageSpawnPoints; // 보스 스테이지 몬스터 소환 위치

    public List<GameObject> spawnedMonsters = new List<GameObject>(); // 소환한 몬스터

    public Transform pos;

    private void Awake()
    {
        stagerManage = GameObject.Find("Manager").GetComponent<StageManager>();
    }

    private void Update()
    {
        stage = stagerManage.mainStage;
    }

    // 몬스터 소환
    public void MonsterInstantiate(int base0Count, int base1Count, int base2Count, int base3Count, int bossCount)
    {
        // 보스 스테이지 O
        if (stagerManage.subStage == 5)
        {
            GameObject[] currentStageMonsters = null;

            if (stagerManage.mainStage <= 7)
            {
                switch (stagerManage.mainStage)
                {
                    case 1:
                        currentStageMonsters = stage1Monsters;
                        break;
                    case 2:
                        currentStageMonsters = stage2Monsters;
                        break;
                    case 3:
                        currentStageMonsters = stage3Monsters;
                        break;
                    case 4:
                        currentStageMonsters = stage4Monsters;
                        break;
                    case 5:
                        currentStageMonsters = stage5Monsters;
                        break;
                    case 6:
                        currentStageMonsters = stage6Monsters;
                        break;
                    case 7:
                        currentStageMonsters = stage7Monsters;
                        break;
                }

                for (int i = 0; i < base0Count; i++)
                {
                    InstantiateRandom(currentStageMonsters[0], bossStageSpawnPoints);
                }
                for (int i = 0; i < base1Count; i++)
                {
                    InstantiateRandom(currentStageMonsters[1], bossStageSpawnPoints);
                }
                for (int i = 0; i < base2Count; i++)
                {
                    InstantiateRandom(currentStageMonsters[2], bossStageSpawnPoints);
                }
                for (int i = 0; i < base3Count; i++)
                {
                    InstantiateRandom(currentStageMonsters[3], bossStageSpawnPoints);
                }
                if (bossCount > 0)
                {
                    Vector3 bossPos = new Vector3(pos.position.x, pos.position.y, pos.position.z -5);
                    GameObject bossMonster = Instantiate(currentStageMonsters[4], bossPos, Quaternion.identity);
                    spawnedMonsters.Add(bossMonster);
                }
            }
        }
        
        // 보스 스테이지 X
        if (stagerManage.subStage != 5)
        {
            GameObject[] currentStageMonsters = null;

            if (stagerManage.mainStage <= 7)
            {
                switch (stagerManage.mainStage)
                {
                    case 1:
                        currentStageMonsters = stage1Monsters;
                        break;
                    case 2:
                        currentStageMonsters = stage2Monsters;
                        break;
                    case 3:
                        currentStageMonsters = stage3Monsters;
                        break;
                    case 4:
                        currentStageMonsters = stage4Monsters;
                        break;
                    case 5:
                        currentStageMonsters = stage5Monsters;
                        break;
                    case 6:
                        currentStageMonsters = stage6Monsters;
                        break;
                    case 7:
                        currentStageMonsters = stage7Monsters;
                        break;
                }

                for (int i = 0; i < base0Count; i++)
                {
                    InstantiateRandom(currentStageMonsters[0], monsterSpawnPoints);
                }
                for (int i = 0; i < base1Count; i++)
                {
                    InstantiateRandom(currentStageMonsters[1], monsterSpawnPoints);
                }
                for (int i = 0; i < base2Count; i++)
                {
                    InstantiateRandom(currentStageMonsters[2], monsterSpawnPoints);
                }
                for (int i = 0; i < base3Count; i++)
                {
                    InstantiateRandom(currentStageMonsters[3], monsterSpawnPoints);
                }
            }
            else
            {
                // 8스테이지 이후부터는 InfiniteMonsters[]만 사용
                for (int i = 0; i < (base0Count + base1Count + base2Count + base3Count); i++)
                {
                    int randomIndex = Random.Range(0, 6);
                    InstantiateRandom(InfiniteMonsters[randomIndex], monsterSpawnPoints);
                }
                if (bossCount > 0)
                {
                    Vector3 bossPos = new Vector3(pos.position.x, pos.position.y, pos.position.z + -5);
                    GameObject bossMonster = Instantiate(InfiniteMonsters[6], bossPos, Quaternion.identity);
                    spawnedMonsters.Add(bossMonster);
                }
            }
        }
    }

    // 스테이지 몬스터를 랜덤위치로 소환
    public void InstantiateRandom(GameObject monsterPrefab, GameObject[] spawnPoints)
    {
        int randomIndex = Random.Range(0, spawnPoints.Length);
        Vector3 randomPosition = GetRandomPosition(spawnPoints[randomIndex].transform.position);
        GameObject spawnedMonster = Instantiate(monsterPrefab, randomPosition, Quaternion.identity);
        spawnedMonsters.Add(spawnedMonster); // 몬스터를 리스트에 추가
    }

    public Vector3 GetRandomPosition(Vector3 center)
    {
        float radius = 1.0f;

        float x = Random.Range(center.x - radius, center.x + radius);
        float y = Random.Range(center.y - radius, center.y + radius);
        float z = -5;

        return new Vector3(x, y, z);
    }

    // 몬스터 사망시 소환한 몬스터 리스트에서 제거
    public void RemoveMonsterFromList(GameObject monster)
    {
        spawnedMonsters.Remove(monster);
        if (spawnedMonsters.Count == 0)
        {
            stagerManage.NextStage();
        }
    }
}
