using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage8_5 : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject bulletSubPrefab;
    public float bulletSpd;
    public float bulletSubSpd;
    public float sizeReduction;

    void Start()
    {
        //InvokeRepeating("Attack", 6.5f, 5.5f);
    }

    public void Attack()
    {
        float randomAngle = Random.Range(0f, 360f);

        Vector3 direction = new Vector3(Mathf.Cos(randomAngle * Mathf.Deg2Rad), Mathf.Sin(randomAngle * Mathf.Deg2Rad), 1f);
        Vector3 bulletPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 1.2f, -10f);
        GameObject bullet = Instantiate(bulletPrefab, bulletPos, Quaternion.identity);

        bullet.name = "MonsterAttack";

        StartCoroutine(DestroyBullet(bullet, direction, bulletSpd));

        StartCoroutine(FireSubBullet(bullet.transform, bulletSubSpd));
    }

    IEnumerator DestroyBullet(GameObject bullet, Vector3 direction, float bulletSpd)
    {
        SpriteRenderer spriteRenderer = bullet.GetComponent<SpriteRenderer>();
        Rigidbody2D rigidbody = bullet.GetComponent<Rigidbody2D>();

        while (spriteRenderer.transform.localScale.x > 0)
        {
            Vector3 newScale = spriteRenderer.transform.localScale - Vector3.one * sizeReduction * Time.deltaTime;
            spriteRenderer.transform.localScale = newScale;

            rigidbody.velocity = direction * bulletSpd;

            yield return null;
        }

        Destroy(bullet);
    }

    IEnumerator FireSubBullet(Transform parentTransform, float bulletSubSpd)
    {
        for(int i = 0; i < 10; i++) 
        {
            float randomAngle = Random.Range(0f, 360f);
            Vector3 direction = new Vector3(Mathf.Cos(randomAngle * Mathf.Deg2Rad), Mathf.Sin(randomAngle * Mathf.Deg2Rad), 1f);

            GameObject bulletSub = Instantiate(bulletSubPrefab, parentTransform.position, Quaternion.identity);
            bulletSub.name = "MonsterAttack";
            bulletSub.GetComponent<Rigidbody2D>().velocity = direction * bulletSubSpd;

            yield return new WaitForSeconds(0.5f);
            Destroy(bulletSub,2f);
        }
    }
}
