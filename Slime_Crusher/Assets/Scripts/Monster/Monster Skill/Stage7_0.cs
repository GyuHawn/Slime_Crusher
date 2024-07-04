using System.Collections;
using UnityEngine;

public class Stage7_0 : MonoBehaviour
{
    public GameObject bulletPrefab; // ÃÑ¾Ë ÇÁ¸®ÆÕ
    public float bulletSpd; // ÃÑ¾Ë ¼Óµµ
    public Vector3 monsterPos; // ¸ó½ºÅÍ À§Ä¡

    void Start()
    {
        monsterPos = gameObject.transform.position;
    }

    public void Attack()
    {
        float[] angles = { 0f, 90f, 180f, 270f };

        foreach (float angle in angles)
        {
            StartCoroutine(MoveBullet(angle, bulletSpd, 2f));
        }
    }

    IEnumerator MoveBullet(float angle, float spd, float duration)
    {
        Vector3 bulletPos = new Vector3(transform.position.x, transform.position.y, -10f);
        GameObject bullet = Instantiate(bulletPrefab, bulletPos, Quaternion.Euler(0, 0, angle));
        bullet.name = "MonsterAttack";
        Vector3 bulletDirection = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0f);
        bullet.GetComponent<Rigidbody2D>().velocity = bulletDirection * spd;

        yield return new WaitForSeconds(duration);

        bullet.GetComponent<Rigidbody2D>().velocity = -bulletDirection * spd;

        yield return new WaitForSeconds(duration);

        Destroy(bullet);
    }
}
