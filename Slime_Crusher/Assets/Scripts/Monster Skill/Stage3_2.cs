using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage3_2 : MonoBehaviour
{
    private MonsterSpwan monsterSpwan;

    public GameObject eggPrefab;
    public GameObject monsterPrefab;

    void Awake()
    {
        monsterSpwan = GameObject.Find("Manager").GetComponent<MonsterSpwan>();
    }

    public void Attack()
    {
        StartCoroutine(SpawnEggAndMonster());
    }

    IEnumerator SpawnEggAndMonster()
    {
        Vector3 randomPos = new Vector3(Random.Range(-4f, 4f), Random.Range(-1f, 2f), 1f);
        GameObject egg = Instantiate(eggPrefab, randomPos, Quaternion.identity);
        egg.name = "MonsterAttack";

        yield return new WaitForSeconds(3f);

        if (egg != null)
        {
            Destroy(egg);

            Vector3 monsterPos = new Vector3(randomPos.x, randomPos.y, -2f);
            GameObject monster = Instantiate(monsterPrefab, randomPos, Quaternion.identity);
            monsterSpwan.spawnedMonsters.Add(monster);
        }
    }
}
