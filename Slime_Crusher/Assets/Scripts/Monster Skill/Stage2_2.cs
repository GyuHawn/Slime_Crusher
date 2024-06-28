using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2_2 : MonoBehaviour
{
    public GameObject[] bulletPrefabs;
    public float bulletSpd;

    public void Attack()
    {
        StartCoroutine(MonsterAttack());
    }

    IEnumerator MonsterAttack()
    {
        for (int i = 0; i < 6; i++)
        {
            Vector3 bulletPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -10f);
            GameObject bullet = Instantiate(bulletPrefabs[i % bulletPrefabs.Length], bulletPos, Quaternion.identity);
            bullet.name = "MonsterAttack";
            float randomAngle = Random.Range(0f, 360f);
            Vector3 direction = new Vector3(Mathf.Cos(randomAngle * Mathf.Deg2Rad), Mathf.Sin(randomAngle * Mathf.Deg2Rad), +1);

            bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpd;

            Destroy(bullet, 5f);

            yield return new WaitForSeconds(0.2f);
        }
    }
}
