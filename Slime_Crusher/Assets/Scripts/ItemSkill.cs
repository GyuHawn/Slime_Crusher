using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemSkill : MonoBehaviour
{
    private SelectItem selectItem;
    private PlayerController playerController;
    private Character character;
    private CharacterSkill characterSkill;

    // fire
    private GameObject fireInstance;
    public GameObject fireEffect;
    public float fireDamage;
    public float fireDamagePercent;
    public float fireDuration;
    public float firePercent;
    public bool isFire;

    // fireShot
    public GameObject fireShotEffect;
    public GameObject fireShotSub;
    public int fireShotSubNum;
    public float fireShotDamage;
    public float fireShotDamagePercent;
    public float fireShotSubDamage;
    public float fireShotSubDamagePercent;
    public float fireShotPercent;
    public bool isFireShot;

    // holyWave
    private GameObject WaveInstance;
    public GameObject holyWaveEffect;
    public Transform holyWavePos;
    public bool holyWave;
    public float holyWaveDuration;
    public float holyWaveDamage;
    public float holyWaveDamagePercent;
    public float holyWavePercent;
    public bool isHolyWave;

    // holyShot
    public GameObject holyShotEffect;
    public float holyShotDuration;
    public float holyShotDamage;
    public float holyShotDamagePercent;
    public float holyShotPercent;
    public bool isHolyShot;

    // melee
    public GameObject meleeEffect;
    public int meleeNum;
    public float meleePercent;
    public bool isMelee;

    // posion
    public GameObject posionEffect;
    public float posionDuration;
    public float poisonDamage;
    public float poisonDamagePercent;
    public float posionPercent;
    public bool isPosion;

    // rock
    public GameObject rockEffect;
    public float rockDamage;
    public float rockDamagePercent;
    public float rockPercent;
    public bool isRock;

    // sturn
    public GameObject sturnEffect;
    public GameObject sturnImage;
    public float sturnDuration;
    public float sturnPercent;
    public bool isSturn;
    private GameObject currentAttackedMonster; // 선택된 몬스터

    private void Awake()
    {
        selectItem = GameObject.Find("Manager").GetComponent<SelectItem>();
        playerController = GameObject.Find("Manager").GetComponent<PlayerController>();
        character = GameObject.Find("Manager").GetComponent<Character>();
        characterSkill = GameObject.Find("Manager").GetComponent<CharacterSkill>();
    }

    void Start()
    {
        // 사용 여부
        holyWave = false;
    }

    private void Update()
    {
        UpdateDamagePercents(); // 데미지 퍼센트 업데이트
        UpdateDamage(); // 데미지 업데이트
        UpdateSkillCounts(); // 스킬 개수 업데이트
        UpdateSkillDurations(); // 지속시간 업데이트
        UpdateSkillPercents(); // 발동 확률 업데이트
    }

    void UpdateDamagePercents() // 데미지 퍼센트 업데이트
    {
        fireDamagePercent = 0.4f + (0.1f * selectItem.fireLv);
        fireShotDamagePercent = 1.3f + (0.2f * selectItem.fireShotLv);
        fireShotSubDamagePercent = 0.3f + (0.2f * selectItem.fireShotLv);
        holyWaveDamagePercent = 0.25f + (0.05f * selectItem.holyWaveLv);
        holyShotDamagePercent = 0.5f + (0.2f * selectItem.holyShotLv);
        poisonDamagePercent = 0.35f + (0.05f * selectItem.posionLv);
        rockDamagePercent = 1.5f + (0.5f * selectItem.rockLv);
    }
    void UpdateDamage() // 데미지 업데이트
    {
        fireDamage = (playerController.damage + playerController.comboDamage) * fireDamagePercent;
        fireShotDamage = (playerController.damage + playerController.comboDamage) * fireShotDamagePercent;
        fireShotSubDamage = (playerController.damage + playerController.comboDamage) * fireShotSubDamagePercent;
        holyWaveDamage = (playerController.damage + playerController.comboDamage) * holyWaveDamagePercent;
        holyShotDamage = (playerController.damage + playerController.comboDamage) * holyShotDamagePercent;
        poisonDamage = (playerController.damage + playerController.comboDamage) * poisonDamagePercent;
        rockDamage = (playerController.damage + playerController.comboDamage) * rockDamagePercent;
    }
    void UpdateSkillCounts() // 스킬 개수 업데이트
    {
        fireShotSubNum = 2 + (1 * selectItem.fireShotLv);
        meleeNum = 2 + (1 * selectItem.meleeLv);
    }
    void UpdateSkillDurations() // 지속시간 업데이트
    {
        fireDuration = 2.5f + (0.5f * selectItem.fireLv);
        holyShotDuration = 1.5f + (0.5f * selectItem.holyShotLv);
        holyWaveDuration = 3.5f + (0.5f * selectItem.holyWaveLv);
        posionDuration = 5f;
        sturnDuration = 2f + (1 * selectItem.sturnLv);
    }
    void UpdateSkillPercents() // 발동 확률 업데이트
    {     
        if (character.currentCharacter == 4)
        {
            firePercent = 10f + (0.5f * characterSkill.luckLevel);
            fireShotPercent = 20f + (0.5f * characterSkill.luckLevel);
            holyShotPercent = 10f + (0.5f * characterSkill.luckLevel);
            holyWavePercent = 5f + (0.5f * characterSkill.luckLevel);
            rockPercent = 30f + (0.5f * characterSkill.luckLevel);
            posionPercent = 10f + (0.5f * characterSkill.luckLevel);
            meleePercent = 60f + (0.5f * characterSkill.luckLevel);
            sturnPercent = 30f + (0.5f * characterSkill.luckLevel);
        }
        else
        {
            firePercent = 10f;
            fireShotPercent = 20f;
            holyShotPercent = 10f;
            holyWavePercent = 5f;
            rockPercent = 30f;
            posionPercent = 10f;
            meleePercent = 60f;
            sturnPercent = 30f;
        }
    }



    // 아이템 획득
    public void GetItem()
    {
        // 특정 오브젝트를 찾아서 null이 아니면 true 반환
        isFire = GameObject.Find("FirePltem") != null;
        isFireShot = GameObject.Find("Fire ShotPltem") != null;
        isHolyWave = GameObject.Find("Holy WavePltem") != null;
        isHolyShot = GameObject.Find("Holy ShotPltem") != null;
        isRock = GameObject.Find("RockPltem") != null;
        isPosion = GameObject.Find("PosionPltem") != null;
        isMelee = GameObject.Find("MeleePltem") != null;
        isSturn = GameObject.Find("SturnPltem") != null;
    }

    // fire
    public void Fire(Vector3 targetPosition)
    {
        if(isFire)
        {
            AudioManager.Instance.PlayFireAudio();

            Vector3 firePos = new Vector3(targetPosition.x, targetPosition.y, targetPosition.z + 3f);
            fireInstance = Instantiate(fireEffect, firePos, Quaternion.Euler(-90, 0, 0));
            fireInstance.name = "PlayerSkill";

            Destroy(fireInstance, 3f);
        }      
    }

    // fireShot
    public void FireShot(Vector3 targetPosition)
    {
        if (isFireShot)
        {
            AudioManager.Instance.PlayFireShotAudio();

            GameObject fireShotInstance = Instantiate(fireShotEffect, targetPosition, Quaternion.identity);
            List<GameObject> sub = new List<GameObject>();

            for (int i = 0; i < fireShotSubNum; i++)
            {
                GameObject subShot = Instantiate(fireShotSub, targetPosition, Quaternion.identity);
                subShot.name = "PlayerSkill";
                Vector2 randomDirection = Random.insideUnitCircle.normalized;
                subShot.GetComponent<Rigidbody2D>().velocity = randomDirection * 5f;

                sub.Add(subShot);
            }

            StartCoroutine(DestroySubShots(sub, 3f));
            Destroy(fireShotInstance, 1f);
        }
    } 

    private IEnumerator DestroySubShots(List<GameObject> subShots, float delay)
    {
        yield return new WaitForSeconds(delay);

        foreach (GameObject subShot in subShots)
        {
            Destroy(subShot);
        }
    }

    // holyWave 

    public void HolyWave()
    {
        if(isHolyWave)
        {
            AudioManager.Instance.PlayHolyWaveAudio();

            WaveInstance = Instantiate(holyWaveEffect, holyWavePos.position, Quaternion.identity);
            WaveInstance.name = "PlayerSkill";
            holyWave = true;

            Destroy(WaveInstance, holyWaveDuration);
            StartCoroutine(DestroyWave());
        }      
    }

    IEnumerator DestroyWave()
    {
        yield return new WaitForSeconds(holyWaveDuration);
        Destroy(WaveInstance);
        holyWave = false;
    }

    // holyShot 

    public void HolyShot(Vector3 targetPosition)
    {
        if (isHolyShot)
        {
            AudioManager.Instance.PlayHolyShotAudio();

            GameObject holyShotInstance = Instantiate(holyShotEffect, targetPosition, Quaternion.identity);
            holyShotInstance.name = "PlayerSkill";

            if (holyShotInstance != null) 
            {
                StartCoroutine(RotateHolyShot(holyShotInstance, 5f));       
            }
        }       
    }

    private IEnumerator RotateHolyShot(GameObject holyShotInstance, float duration)
    {
        if (holyShotInstance == null)
        {
            yield break;
        }

        float elapsedTime = 0f;
        float rotationSpd = 360f / duration;

        while (elapsedTime < duration)
        {
            if (holyShotInstance == null)
            {
                yield break;
            }

            holyShotInstance.transform.Rotate(rotationSpd * Time.deltaTime, 0f, 0f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }



    // melee 
    public void Melee(Vector3 targetPosition, int numEffects)
    {
        if (isMelee)
        {
            StartCoroutine(MeleeInstantiate(targetPosition, numEffects));
        }     
    }

    IEnumerator MeleeInstantiate(Vector3 targetPosition, int numEffects)
    {
        for (int i = 0; i < numEffects; i++)
        {
            AudioManager.Instance.PlayMeleeAudio();

            Vector3 randomOffset = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);
            Vector3 spawnPosition = targetPosition + randomOffset;

            GameObject meleeInstance = Instantiate(meleeEffect, spawnPosition, Quaternion.identity);
            meleeInstance.name = "PlayerSkill";
            Destroy(meleeInstance, 0.2f);

            yield return new WaitForSeconds(0.1f);
        }
    }

    // posion 
    public void Posion(Vector3 targetPosition)
    {
        if(isPosion)
        {
            AudioManager.Instance.PlayPoisonAudio();

            GameObject posionInstance = Instantiate(posionEffect, targetPosition, Quaternion.identity);
            Destroy(posionInstance, 5f);
        }       
    }

    // rock 
    public void Rock(Vector3 targetPosition)
    {
        if(isRock)
        {
            AudioManager.Instance.PlayRockAudio();

            GameObject rockInstance = Instantiate(rockEffect, targetPosition, Quaternion.identity);
            Destroy(rockInstance, 5f);
        }       
    }

    // sturn 
    private Dictionary<GameObject, GameObject> monsterToSturnImage = new Dictionary<GameObject, GameObject>();

    public void Sturn()
    {
        if (isSturn)
        {
            if (currentAttackedMonster != null)
            {
                AudioManager.Instance.PlayStunAudio();

                MonsterController monsterController = currentAttackedMonster.GetComponent<MonsterController>();
                if (monsterController != null)
                {
                    GameObject sturnInstance = Instantiate(sturnEffect, currentAttackedMonster.transform.position, Quaternion.identity);
                    Vector3 imagePos = new Vector3(monsterController.sturn.transform.position.x, monsterController.sturn.transform.position.y, monsterController.sturn.transform.position.z - 0.5f);
                    GameObject sturnimageInstance = Instantiate(sturnImage, monsterController.sturn.transform.position, Quaternion.identity);
                    sturnimageInstance.name = "PlayerSkill";

                    monsterController.stop = true;
                    monsterController.attackTime += 5;
                    monsterToSturnImage[currentAttackedMonster] = sturnimageInstance;

                    Destroy(sturnimageInstance, 2f);
                }
            }

            StartCoroutine(Removestun());
        }
    }

    IEnumerator Removestun()
    {
        yield return new WaitForSeconds(3f);

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

    public void SetCurrentAttackedMonster(GameObject monster) // 현재 공격받는 몬스터 설정
    {
        currentAttackedMonster = monster;
    }

    // 기절중인 몬스터 사망시 기절 이미지도 삭제
    public void DestroyMonster(GameObject monster)
    {
        if (monsterToSturnImage.ContainsKey(monster))
        {
            Destroy(monsterToSturnImage[monster]);
            monsterToSturnImage.Remove(monster);
        }

        Destroy(monster);
    }
}
