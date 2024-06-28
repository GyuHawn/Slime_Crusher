using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage8_1 : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletSpd;

    public GameObject pos;
    public Vector3 boxSize;

    void Start()
    {
        pos = GameObject.Find("Stage7 SkillPos"); // ¸Ê Áß¾Ó À§Ä¡ ºó ¿ÀºêÁ§Æ®
        boxSize = new Vector3(-13f, 5.5f, 0);
    }

    public void Attack()
    {
        StartCoroutine(MonsterAttack());
    }

    IEnumerator MonsterAttack()
    {
        for(int i = 0; i < 10; i++)
        {
            float randomX = Random.Range(-boxSize.x / 2f, boxSize.x / 2f);
            float randomY = Random.Range(-boxSize.y / 2f, boxSize.y / 2f);

            GameObject bullet = Instantiate(bulletPrefab, pos.transform.position + new Vector3(randomX, randomY, -10f), Quaternion.identity);
            bullet.name = "MonsterAttack";

            bullet.GetComponent<Rigidbody2D>().velocity = Vector2.up * bulletSpd;

            yield return new WaitForSeconds(0.2f);

            Destroy(bullet, 4f);
        }
    }
}
