using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage5_1 : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject bulletsubPrefab;
    public float bulletSpd;

    public void Attack()
    {
        StartCoroutine(MonsterAttack());
    }

    IEnumerator MonsterAttack()
    {
        Vector3 randomPos = new Vector3(Random.Range(-3f, 3f), Random.Range(-1f, 2f), -10f);
        GameObject skill = Instantiate(bulletPrefab, randomPos, Quaternion.identity);
        skill.name = "MonsterAttack";

        yield return new WaitForSeconds(1f);

        Destroy(skill);

        for (int i = 0; i < 5; i++)
        {
            float randomAngle = Random.Range(0f, 360f);

            Vector3 direction = new Vector3(Mathf.Cos(randomAngle * Mathf.Deg2Rad), Mathf.Sin(randomAngle * Mathf.Deg2Rad), +1f);
            Vector3 bulletPos = new Vector3(randomPos.x, randomPos.y, -10f);
            GameObject bulletSub = Instantiate(bulletsubPrefab, bulletPos, Quaternion.identity);
            bulletSub.name = "MonsterAttack";
            bulletSub.GetComponent<Rigidbody2D>().velocity = direction * bulletSpd;

            Destroy(bulletSub, 5f);
        }
    }
}
