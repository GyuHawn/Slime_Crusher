using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_SturnSkill : C_Skill
{
    private ItemSkill itemSkill;

    public C_SturnSkill(ItemSkill itemSkill)
    {
        this.itemSkill = itemSkill;
    }

    public void Execute(CharacterSkill characterSkill)
    {
        if (characterSkill.sturnTime <= 0)
        {
            AudioManager.Instance.PlayStunAudio();
            SturnAttack(characterSkill);
        }
    }

    private void SturnAttack(CharacterSkill characterSkill)
    {
        characterSkill.sturnTime = 4; // 쿨타임 적용

        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster"); // 모든 몬스터 확인

        foreach (GameObject monster in monsters)
        {
            MonsterController monsterController = monster.GetComponent<MonsterController>();
            if (monsterController != null)
            {
                GameObject sturnInstance = GameObject.Instantiate(itemSkill.sturnEffect, monster.transform.position, Quaternion.identity); // 스턴 이펙트 생성
                GameObject sturnimageInstance = GameObject.Instantiate(itemSkill.sturnImage, monsterController.sturn.transform.position, Quaternion.identity); // 스턴 이미지 생성

                monsterController.stop = true; // 몬스터 기절 적용
                monsterController.attackTime += 5; // 몬스터 공격 시간 늘리기
                characterSkill.monsterToSturnImage[monster] = sturnimageInstance; // 각 몬스터와 스턴 이미지 함께 관리 (몬스터 제거시 이미지도 함께 삭제)

                GameObject.Destroy(sturnimageInstance, 3f); // 스턴 이미지 제거
            }
        }
        characterSkill.StartCoroutine(Removestun(characterSkill));
    }

    private IEnumerator Removestun(CharacterSkill character)
    {
        // 스턴 지속시간 종료시 몬스터의 기절 적용 해제
        yield return new WaitForSeconds(character.sturnDuration);
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
}
