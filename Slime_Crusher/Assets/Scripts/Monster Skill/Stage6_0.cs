using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage6_0 : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletSpd;
    public void Attack()
    {
        StartCoroutine(MonsterAttack());
    }

    IEnumerator MonsterAttack()
    {
        for (int i = 0; i < 8; i++)
        {
            float randomX = transform.position.x + Random.Range(-2f, 2f);

            Vector3 bulletPos = new Vector3(randomX, transform.position.y - 0.5f, -10f);
            Vector2 direction = Vector2.up;

            GameObject bullet = Instantiate(bulletPrefab, bulletPos, Quaternion.identity);
            bullet.name = "MonsterAttack";

            bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpd;

            Destroy(bullet, 3f);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
