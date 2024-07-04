using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage6_3 : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject pos;
    private Vector3 newPos;
    public Vector3 boxSize;

    void Start()
    {
        pos = GameObject.Find("Stage7 SkillPos"); // ¸Ê Áß¾Ó À§Ä¡ ºó ¿ÀºêÁ§Æ®
        newPos = new Vector3(pos.transform.position.x, pos.transform.position.y + 2, pos.transform.position.z);
        boxSize = new Vector3(-13f, 5.5f, 0);
    }

    public void Attack()
    {
        StartCoroutine(MonsterAttack());
    }

    IEnumerator MonsterAttack()
    {
        for(int i = 0; i < 5; i++)
        { 
            float randomX = Random.Range(newPos.x - boxSize.x / 2, newPos.x + boxSize.x / 2);
            float randomY = Random.Range(newPos.y - boxSize.y / 2, newPos.y + boxSize.y / 2);
            float randomZ = -10f;

           Vector3 bulletPos = new Vector3(randomX, randomY, randomZ);

            GameObject bullet = Instantiate(bulletPrefab, bulletPos, Quaternion.Euler(180, 0, 0));
            bullet.name = "MonsterAttack";

            Destroy(bullet, 2f);
            yield return new WaitForSeconds(0.8f);
        }
    }
}
