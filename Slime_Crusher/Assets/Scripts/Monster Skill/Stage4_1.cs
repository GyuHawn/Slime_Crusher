using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage4_1 : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletSpd;

    public void Attack()
    {
        float[] angles = { 0f, 45f, 90f, 135f, 180f, 225f, 270f, 315f, 360f };

        foreach (float angle in angles)
        {
            float radianAngle = angle * Mathf.Deg2Rad;
            Vector3 direction = new Vector3(Mathf.Cos(radianAngle), Mathf.Sin(radianAngle), 1f);

            Vector3 bulletPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -10f);
            GameObject bullet = Instantiate(bulletPrefab, bulletPos, Quaternion.Euler(0, 0, angle - 90));
            bullet.name = "MonsterAttack";
            bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpd;

            Destroy(bullet, 4f);
        }
    }
}
