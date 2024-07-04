using System.Collections;
using UnityEngine;

public class Stage8_2 : MonoBehaviour
{
    public GameObject attackEffectPrefab; // ÀÌÆåÆ® ÇÁ¸®ÆÕ
    public GameObject bulletPrefab; // ÃÑ¾Ë ÇÁ¸®ÆÕ
    public float bulletSpd;

    public GameObject pos;
    public Vector3 boxSize;

    void Start()
    {
        pos = GameObject.Find("Stage7 SkillPos"); // ¸Ê Áß¾Ó À§Ä¡ ºó ¿ÀºêÁ§Æ®
        boxSize = new Vector3(-13f, 5.5f, 0);
    }

    public void Attack()
    {
        StartCoroutine(MonsterAttack());
    }

    IEnumerator MonsterAttack()
    {
        float randomX = Random.Range(pos.transform.position.x - boxSize.x / 2, pos.transform.position.x + boxSize.x / 2);
        float randomY = Random.Range(pos.transform.position.y - boxSize.y / 2, pos.transform.position.y + boxSize.y / 2);
        float randomZ = 2f;

        Vector3 bulletPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -10f);
        GameObject bullet = Instantiate(bulletPrefab, bulletPos, Quaternion.identity);
        bullet.name = "MonsterAttack";

        Vector3 targetPosition = new Vector3(randomX, randomY, randomZ);
        float distance = Vector3.Distance(bullet.transform.position, targetPosition);
        float duration = distance / bulletSpd;

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            bullet.transform.position = Vector3.Lerp(bullet.transform.position, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(bullet);

        StartCoroutine(SpawnEffect(targetPosition));
    }

    IEnumerator SpawnEffect(Vector3 position)
    {
        GameObject effect = Instantiate(attackEffectPrefab, position, Quaternion.identity);
        effect.name = "MonsterAttack";
        yield return new WaitForSeconds(10f);

        Destroy(effect);
    }
}
