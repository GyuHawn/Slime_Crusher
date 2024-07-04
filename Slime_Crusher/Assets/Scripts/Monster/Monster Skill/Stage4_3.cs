using System.Collections;
using UnityEngine;

public class Stage4_3 : MonoBehaviour
{
    public GameObject bulletPrefab;

    public void Attack()
    {
        StartCoroutine(MonsterAttack());
    }

    IEnumerator MonsterAttack()
    {
        for (int i = 0; i < 3; i++)
        {
            float randomX = transform.position.x + Random.Range(-2f, 2f);
            Vector3 bulletPos = new Vector3(randomX, transform.position.y + 1.5f, -10f);
            GameObject bullet = Instantiate(bulletPrefab, bulletPos, Quaternion.identity);
            bullet.name = "MonsterAttack";

            Destroy(bullet, 2f);

            yield return new WaitForSeconds(0.5f);
        }
    }
}
