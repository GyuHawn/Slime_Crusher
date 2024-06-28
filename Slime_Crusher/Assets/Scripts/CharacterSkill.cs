using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterSkill : MonoBehaviour
{
    private ItemSkill itemSkill;
    private AudioManager audioManager;
    private PlayerController playerController;
    private MonsterController monsterController;

    // 캐릭터 변수
    public int rockLevel;
    public int rockDamage;
    public int rockTime; // 쿨타임
    public GameObject rockCoolTime;
    public TMP_Text rockCoolTimeText;

    public bool useWaterSkill = false; // 스킬 사용관리 
    public int waterLevel;
    public GameObject waterEffect;
    public int waterDamage;
    public int waterTime;
    public GameObject waterCoolTime;
    public TMP_Text waterCoolTimeText;

    public int sturnLevel;
    public float sturnDuration; // 지속시간
    public float sturnTime;
    public GameObject sturnCoolTime;
    public TMP_Text sturnCoolTimeText;

    public int luckLevel;

    private void Awake()
    {
        itemSkill = GameObject.Find("Manager").GetComponent<ItemSkill>();
        audioManager = GameObject.Find("Manager").GetComponent<AudioManager>();
        playerController = GameObject.Find("Manager").GetComponent<PlayerController>();
        monsterController = GameObject.Find("Manager").GetComponent<MonsterController>();
    }

    void Start()
    { 
        // 캐릭터 레벨 받아오기
        rockLevel = PlayerPrefs.GetInt("rockLevel", 1);
        waterLevel = PlayerPrefs.GetInt("waterLevel", 1);
        sturnLevel = PlayerPrefs.GetInt("lightLevel", 1);
        luckLevel = PlayerPrefs.GetInt("luckLevel", 1);
    }

    void Update()
    {
        // 캐릭터 수치 관리
        rockDamage = (int)((playerController.damage + playerController.comboDamage) + (playerController.damage * (0.1f * rockLevel)));
        waterDamage = (int)((playerController.damage + playerController.comboDamage) * (0.1f * waterLevel));
        sturnTime = 3 + (0.1f * sturnLevel);

        // 쿨타임 중 사용제어
        if (rockTime > 0)
        {
            rockCoolTime.SetActive(true);
            rockCoolTimeText.text = rockTime.ToString();
        }
        else
        {
           rockCoolTime.SetActive(false);
        }
        if (waterTime > 0)
        {
            waterCoolTime.SetActive(true);
            waterCoolTimeText.text = waterTime.ToString();
        }
        else
        {
            waterCoolTime.SetActive(false);
        }
        if (sturnTime > 0)
        {
            sturnCoolTime.SetActive(true);
            sturnCoolTimeText.text = sturnTime.ToString();
        }
        else
        {
            sturnCoolTime.SetActive(false);
        }
    }

    // 스킬사용
    public void Rock()
    {
        if(rockTime <= 0)
        {
            audioManager.RockAudio();
            RockAttack();
        }
    }

    void RockAttack()
    {
        rockTime = 4; // 쿨타임 적용

        // 모든 몬스터, 보스 확인
        GameObject[] mosters = GameObject.FindGameObjectsWithTag("Monster");
        GameObject[] bossMonsters = GameObject.FindGameObjectsWithTag("Boss");

        // 몬스터 리스트에 할당
        List<GameObject> allMonsters = new List<GameObject>(mosters);
        allMonsters.AddRange(bossMonsters);

        foreach (GameObject monster in allMonsters)
        {
            MonsterController monsterController = monster.GetComponent<MonsterController>();

            if (monsterController != null)
            {
                // 이펙트 생성
                GameObject rockInstance = Instantiate(itemSkill.rockEffect, monsterController.gameObject.transform.position, Quaternion.identity);

                if (monsterController.pRockTakeDamage)
                {
                    playerController.CRockDamageText(monsterController); // 데미지 텍스트 생성
                    monsterController.currentHealth -= rockDamage; // 데미지 적용
                    monsterController.PlayerRockDamegeCoolDown(0.5f, 0.2f); // 피격 시간 및 시각적 효과
                }

                ItemSkill(monsterController); // 아이템 사용

                Destroy(rockInstance, 2f); // 이펙트 제거
            }
        }
    }

    // 스킬사용
    public void Sturn()
    {
        if (sturnTime <= 0)
        {
            audioManager.SturnAudio();
            SturnAttack();
        }      
    }
    
    void SturnAttack()
    {
        sturnTime = 4; // 쿨타임 적용

        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster"); // 모든 몬스터 확인

        foreach (GameObject monster in monsters)
        {
            MonsterController monsterController = monster.GetComponent<MonsterController>();
            GameObject sturnInstance = Instantiate(itemSkill.sturnEffect, monster.transform.position, Quaternion.identity); // 스턴 이펙트 생성
            GameObject sturnimageInstance = Instantiate(itemSkill.sturnImage, monsterController.sturn.transform.position, Quaternion.identity); // 스턴 이미지 생성
            if (monsterController != null)
            {
                monsterController.stop = true; // 몬스터 기절 적용
                monsterController.attackTime += 5; // 몬스터 공격 시간 늘리기
            }
            monsterToSturnImage[monster] = sturnimageInstance; // 각 몬스터와 스턴 이미지 함께 관리 (몬스터 제거시 이미지도 함께 삭제)
            Destroy(sturnimageInstance, 3f); // 스턴 이미지 제거
        }
        StartCoroutine(Removestun());
    }

    private Dictionary<GameObject, GameObject> monsterToSturnImage = new Dictionary<GameObject, GameObject>(); // 각 몬스터와 스턴 이미지 함께 관리 (몬스터 제거시 이미지도 함께 삭제)
    IEnumerator Removestun()
    {
        // 스터 지속시간 종료시 몬스터의 기적 적용 해제
        yield return new WaitForSeconds(sturnDuration);
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
        foreach (GameObject monster in monsters)
        {
            MonsterController monsterController = monster.GetComponent<MonsterController>();
            if (monsterController != null)
            {
                monsterController.stop = false;
            }
        }
    }

    // 몬스터 제거시 스턴 이미지도 함께 삭제
    public void DestroyMonster(GameObject monster)
    {
        if (monsterToSturnImage.ContainsKey(monster))
        {
            Destroy(monsterToSturnImage[monster]);
            monsterToSturnImage.Remove(monster);
        }
        Destroy(monster);
    }

    // 스킬사용
    public void Water()
    {
        if (waterTime <= 0)
        {
            useWaterSkill = true; // 스킬사용 활성화
            StartCoroutine(WaterAttack());
        }       
    }

    IEnumerator WaterAttack()
    {
        if (useWaterSkill)
        {
            waterTime = 4; // 쿨타임 적용

            List<GameObject> MonsterList = new List<GameObject>(); // 몬스터 확인

            for (int i = 0; i < 20; i++)
            {
                List<GameObject> selectedMonsters = new List<GameObject>(); // 현재 존재하는 몬스터 재 확인
                if (MonsterList.Count == 0)
                {
                    // 모든 몬스터, 보스 확인
                    GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
                    GameObject[] bossMonsters = GameObject.FindGameObjectsWithTag("Boss");

                    // 몬스터 리스트에 할당
                    List<GameObject> allMonsters = new List<GameObject>(monsters);
                    allMonsters.AddRange(bossMonsters);

                    if (allMonsters.Count > 0)
                    {
                        // 랜덤 몬스터 선택
                        selectedMonsters.Add(allMonsters[Random.Range(0, allMonsters.Count)]);
                    }

                    MonsterList.AddRange(selectedMonsters);
                }
                else
                {
                    if (MonsterList.Count > 0)
                    {
                        selectedMonsters.Add(MonsterList[Random.Range(0, MonsterList.Count)]);
                    }
                }


                foreach (GameObject monster in selectedMonsters)
                {
                    MonsterController monsterController = monster.GetComponent<MonsterController>();

                    if (monsterController != null)
                    { 
                        // 이펙트 생성 위치
                        Vector3 waterPosition = new Vector3(monsterController.gameObject.transform.position.x, monsterController.gameObject.transform.position.y - 0.2f, monsterController.gameObject.transform.position.z);
                        GameObject waterInstance = Instantiate(waterEffect, waterPosition, Quaternion.Euler(90, 0, 0)); // 이펙트 생성
                        audioManager.WaterAudio(); // 오디오 실행

                        if (monsterController.pWaterTakeDamage)
                        {
                            playerController.CWaterDamageText(monsterController); // 데미지 텍스트 생성
                            monsterController.currentHealth -= waterDamage; // 데미지 적용
                            monsterController.PlayerWaterDamegeCoolDown(0.5f, 0.1f); // 피격 시간 및 시각적 효과
                        }

                        ItemSkill(monsterController); // 아이템 사용

                        Destroy(waterInstance, 2f); // 이펙트 제거
                    }
                }

                if (MonsterList.Count > 0)
                {
                    // 리스트 초기화
                    MonsterList.Clear();
                }

                if (!useWaterSkill)
                { 
                    // 스킬 비활성화시 스킬사용 종료
                    yield break;
                }

                yield return new WaitForSeconds(0.15f);
            }
        }
    }

    // 캐릭터 스킬로 인한 공격중 확률적으로 아이템 사용
    void ItemSkill(MonsterController monsterController)
    {
        if (itemSkill.isFire && Random.Range(0f, 100f) <= itemSkill.firePercent)
        {
            itemSkill.Fire(monsterController.gameObject.transform.position);
        }
        if (itemSkill.isFireShot && Random.Range(0f, 100f) <= itemSkill.fireShotPercent)
        {
            itemSkill.FireShot(monsterController.gameObject.transform.position);
        }
        if (itemSkill.isHolyWave && Random.Range(0f, 100f) <= itemSkill.holyWavePercent)
        {
            itemSkill.HolyWave();
        }
        if (itemSkill.isHolyShot && Random.Range(0f, 100f) <= itemSkill.holyShotPercent)
        {
            itemSkill.HolyShot(monsterController.gameObject.transform.position);
        }
        if (itemSkill.isPosion && Random.Range(0f, 100f) <= itemSkill.posionPercent)
        {
            itemSkill.Posion(monsterController.gameObject.transform.position);
        }
        if (itemSkill.isRock && Random.Range(0f, 100f) <= itemSkill.rockPercent)
        {
            itemSkill.Rock(monsterController.gameObject.transform.position);
        }
        if (itemSkill.isSturn && Random.Range(0f, 100f) <= itemSkill.sturnPercent)
        {
            itemSkill.Sturn();
        }
    }

    // 쿨타임 관리 (스테이지 이동시 사용)
    public void CharacterCoolTime()
    {
        if (rockTime > 0)
        {
            rockTime--;
        }
        else if (waterTime > 0)
        {
            waterTime--;
        }
        else if (sturnTime > 0)
        {
            sturnTime--;
        }
    }
}
