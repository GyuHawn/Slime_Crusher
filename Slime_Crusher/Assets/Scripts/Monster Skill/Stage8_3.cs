using System.Collections;
using UnityEngine;

public class Stage8_3 : MonoBehaviour
{
    public GameObject effectPrefab;   
    public GameObject poisonPrefab;   
    public GameObject skillPos;      
    public Vector3 boxSize;          
    public float moveSpd = 3f;     

    private Vector3 beforePos;       

    void Start()
    {
        skillPos = GameObject.Find("Stage7 SkillPos");
        boxSize = new Vector3(-13f, 5.5f, 0);
        beforePos = new Vector3 (transform.position.x, transform.position.y, transform.position.z + 3f);
    }

    public void Attack()
    {
        StartCoroutine(RandomMovement());
    }

    IEnumerator RandomMovement()
    {
        StartCoroutine(SpawnEffect(beforePos));

        float randomX = Random.Range(skillPos.transform.position.x - boxSize.x / 2, skillPos.transform.position.x + boxSize.x / 2);
        float randomY = Random.Range(skillPos.transform.position.y - boxSize.y / 2, skillPos.transform.position.y + boxSize.y / 2);
        float randomZ = -10f;

        Vector3 targetPosition = new Vector3(randomX, randomY, randomZ);

        float distance = Vector3.Distance(transform.position, targetPosition);
        float duration = distance / moveSpd;

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        beforePos = new Vector3(transform.position.x, transform.position.y, transform.position.z + 3f);
    }

    IEnumerator SpawnEffect(Vector3 position)
    {
        Vector3 newPosition = new Vector3(position.x, position.y, position.z + 3f);
        GameObject poison = Instantiate(poisonPrefab, newPosition, Quaternion.identity);
        GameObject effect = Instantiate(effectPrefab, newPosition, Quaternion.Euler(60f, 0f, 0f));
        poison.name = "MonsterAttack";

        yield return new WaitForSeconds(8f);

        Destroy(poison);
    }
}
