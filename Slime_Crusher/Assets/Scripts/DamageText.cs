using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    public float moveSpd; // 텍스트 이동 속도
    public float alphaSpd; // 텍스트 투명화 속도
    public float destroyTime; // 텍스트 제거 시간
    private TextMeshPro text;

    Color alpha; 
    public int damege;

    void Start()
    {
        text = GetComponent<TextMeshPro>();
        text.text = damege.ToString(); // 데미지의 값을 받음
        alpha = text.color;
        Invoke("DestroyText", destroyTime); // 일정시간 이후 제거
    }

    void Update()
    {
        transform.Translate(new Vector3(0, moveSpd * Time.deltaTime, 0)); // 이동 값 적용
        alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * alphaSpd); // 투명화 값 적용
        text.color = alpha;
    }

    void DestroyText()
    {
        Destroy(gameObject);
    }
}
