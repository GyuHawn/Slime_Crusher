using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_RockSkill : C_Skill
{
    private ItemSkill itemSkill;
    private PlayerController playerController;

    public C_RockSkill(ItemSkill itemSkill, PlayerController playerController)
    {
        this.itemSkill = itemSkill;
        this.playerController = playerController;
    }

    public void Execute(CharacterSkill characterSkill)
    {
        if (characterSkill.rockTime <= 0)
        {
            AudioManager.Instance.PlayRockAudio();
            RockAttack(characterSkill);
        }
    }

    private void RockAttack(CharacterSkill characterSkill)
    {
        characterSkill.rockTime = 4; // 쿨타임 적용

        // 모든 몬스터, 보스 확인
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
        GameObject[] bossMonsters = GameObject.FindGameObjectsWithTag("Boss");

        // 몬스터 리스트에 할당
        List<GameObject> allMonsters = new List<GameObject>(monsters);
        allMonsters.AddRange(bossMonsters);

        foreach (GameObject monster in allMonsters)
        {
            MonsterController monsterController = monster.GetComponent<MonsterController>();

            if (monsterController != null && monsterController.pRockTakeDamage)
            {
                // 이펙트 생성
                GameObject rockInstance = GameObject.Instantiate(itemSkill.rockEffect, monsterController.gameObject.transform.position, Quaternion.identity);

                playerController.CRockDamageText(monsterController); // 데미지 텍스트 생성
                monsterController.currentHealth -= characterSkill.rockDamage; // 데미지 적용
                monsterController.PlayerRockDamegeCoolDown(0.5f, 0.2f); // 피격 시간 및 시각적 효과

                characterSkill.ItemSkill(monsterController); // 아이템 사용

                GameObject.Destroy(rockInstance, 2f); // 이펙트 제거
            }
        }
    }
}
