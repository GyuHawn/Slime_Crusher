using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage7_2 : MonoBehaviour
{
    public GameObject bulletPrefabs;
    public float bulletSpd;
    public void Attack()
    {
        StartCoroutine(MonsterAttack());
    }

    IEnumerator MonsterAttack()
    {
        for (int i = 0; i < 12; i++)
        {
            float randomAngle = Random.Range(0f, 360f);
            Vector3 bulletPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -10f);
            Vector3 direction = new Vector3(Mathf.Cos(randomAngle * Mathf.Deg2Rad), Mathf.Sin(randomAngle * Mathf.Deg2Rad), 1f);

            GameObject bullet = Instantiate(bulletPrefabs, bulletPos, Quaternion.identity);
            bullet.name = "MonsterAttack";
            bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpd;

            Destroy(bullet, 3f);

            yield return new WaitForSeconds(0.1f);
        }
    }
}
