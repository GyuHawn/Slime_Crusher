using System.Collections;
using UnityEngine;

public class Stage4_2 : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject bulletsubPrefab;
    public float bulletSpd;

    private Vector3 lastBulletPos;

    public void Attack()
    {
        StartCoroutine(MonsterAttack());
    }

    IEnumerator MonsterAttack()
    {
        float randomAngle = Random.Range(45f, 135f);

        Vector3 direction = new Vector3(Mathf.Cos(randomAngle * Mathf.Deg2Rad), Mathf.Sin(randomAngle * Mathf.Deg2Rad), 1f);
        Vector3 bulletPos = new Vector3(transform.position.x, transform.position.y, -10f);
        GameObject bullet = Instantiate(bulletPrefab, bulletPos, Quaternion.identity);
        bullet.name = "MonsterAttack";
        bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpd;

        yield return new WaitForSeconds(2f);
        lastBulletPos = bullet.transform.position;
        Destroy(bullet);

        ShootBulletSub();
    }

    void ShootBulletSub()
    {
        float[] angles = { 45f, 135f, 225f, 315f };

        foreach (float angle in angles)
        {
            float radianAngle = angle * Mathf.Deg2Rad;
            Vector3 direction = new Vector3(Mathf.Cos(radianAngle), Mathf.Sin(radianAngle), 1f);
            Vector3 bulletsubPos = lastBulletPos;
            GameObject bulletsub = Instantiate(bulletsubPrefab, bulletsubPos, Quaternion.identity);
            bulletsub.name = "BulletSub";
            bulletsub.GetComponent<Rigidbody2D>().velocity = direction * bulletSpd;

            Destroy(bulletsub, 2f);
        }
    }
}
