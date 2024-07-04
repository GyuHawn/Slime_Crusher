using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1_3 : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletSpd;

    public void Attack()
    {
        for (int i = 0; i < 2; i++)
        {
            float randomAngle = Random.Range(0f, 360f);

            Vector3 direction = new Vector3(Mathf.Cos(randomAngle * Mathf.Deg2Rad), Mathf.Sin(randomAngle * Mathf.Deg2Rad), 1f);
            Vector3 bulletPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -10f);
            GameObject bullet = Instantiate(bulletPrefab, bulletPos, Quaternion.identity);
            bullet.name = "MonsterAttack";
            bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpd;

            Destroy(bullet, 3f);
        }
    }
}
