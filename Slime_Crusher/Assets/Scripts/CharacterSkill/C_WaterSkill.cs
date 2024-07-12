using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_WaterSkill : C_Skill
{
    private ItemSkill itemSkill;
    private PlayerController playerController;

    public C_WaterSkill(ItemSkill itemSkill, PlayerController playerController)
    {
        this.itemSkill = itemSkill;
        this.playerController = playerController;
    }

    public void Execute(CharacterSkill characterSkill)
    {
        if (characterSkill.waterTime <= 0)
        {
            characterSkill.useWaterSkill = true;
            characterSkill.StartCoroutine(WaterSkillRoutine(characterSkill));
        }
    }

    private IEnumerator WaterSkillRoutine(CharacterSkill characterSkill)
    {
        if (characterSkill.useWaterSkill)
        {
            characterSkill.waterTime = 4; // 쿨타임 적용

            characterSkill.MonsterList = new List<GameObject>(); // 몬스터 확인

            for (int i = 0; i < 20; i++)
            {
                WaterAttack(characterSkill);

                if (characterSkill.MonsterList.Count > 0)
                {
                    // 리스트 초기화
                    characterSkill.MonsterList.Clear();
                }

                if (!characterSkill.useWaterSkill)
                {
                    // 스킬 비활성화시 스킬사용 종료
                    yield break;
                }

                yield return new WaitForSeconds(0.15f);
            }
        }
    }

    private void WaterAttack(CharacterSkill character)
    {
        List<GameObject> selectedMonsters = new List<GameObject>(); // 현재 존재하는 몬스터 재 확인
        if (character.MonsterList.Count == 0)
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

            character.MonsterList.AddRange(selectedMonsters);
        }
        else
        {
            if (character.MonsterList.Count > 0)
            {
                selectedMonsters.Add(character.MonsterList[Random.Range(0, character.MonsterList.Count)]);
            }
        }

        foreach (GameObject monster in selectedMonsters)
        {
            MonsterController monsterController = monster.GetComponent<MonsterController>();

            if (monsterController != null)
            {
                // 이펙트 생성 위치
                Vector3 waterPosition = new Vector3(monsterController.gameObject.transform.position.x, monsterController.gameObject.transform.position.y - 0.2f, monsterController.gameObject.transform.position.z);
                GameObject waterInstance = GameObject.Instantiate(character.waterEffect, waterPosition, Quaternion.Euler(90, 0, 0)); // 이펙트 생성
                AudioManager.Instance.PlayWaterAudio(); // 오디오 실행

                if (monsterController.pWaterTakeDamage)
                {
                    playerController.CWaterDamageText(monsterController); // 데미지 텍스트 생성
                    monsterController.currentHealth -= character.waterDamage; // 데미지 적용
                    monsterController.PlayerWaterDamegeCoolDown(0.5f, 0.1f); // 피격 시간 및 시각적 효과
                }

                character.ItemSkill(monsterController); // 아이템 사용

                GameObject.Destroy(waterInstance, 2f); // 이펙트 제거
            }
        }
    }
}