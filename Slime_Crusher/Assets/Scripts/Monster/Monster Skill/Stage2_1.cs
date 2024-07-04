using System.Collections;
using UnityEngine;

public class Stage2_1 : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletSpd = 180f;

    public void Attack()
    {
        StartCoroutine(RotateAndDeactivate(bulletPrefab, new Vector3(0f, 0f, 1f), bulletSpd, 3f));
    }

    IEnumerator RotateAndDeactivate(GameObject obj, Vector3 axis, float spd, float duration)
    {
        obj.SetActive(true);
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            obj.transform.Rotate(axis * spd * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        obj.SetActive(false);

        yield return new WaitForSeconds(3f);
    }
}
