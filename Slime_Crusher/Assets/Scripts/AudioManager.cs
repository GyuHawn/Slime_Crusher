 using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    private StageManager stageManager;

    // BGM
    public AudioSource bgmMainMenu; // 메인메뉴
    public AudioSource bgmCharacterMenu; // 캐릭터메뉴
    public AudioSource bgmStage; // 스테이지
    public AudioSource bgmBossStage; // 보스스테이지
    public AudioSource bgmSelectMenu; // 아이템선택메뉴
    public AudioSource bgmResultMenu; // 결과

    // function
    public AudioSource attackAudio; // 공격
    public AudioSource defenseAudio; // 방어
    public AudioSource hitAudio; // 피격
    public AudioSource monsterAttackAudio; // 몬스터 공격
    public AudioSource buttonAudio; // 버튼
    public AudioSource startAudio; // 시작

    // Item
    public AudioSource fireAudio; // 파이어
    public AudioSource fireShotAudio; // 파이어샷
    public AudioSource holyShotAudio; // 홀리샷
    public AudioSource holyWaveAudio; // 홀리웨이브
    public AudioSource meleeAudio; // 난투
    public AudioSource posionAudio; // 독
    public AudioSource rockAudio; // 돌
    public AudioSource sturnAudio; // 스턴

    // Character
    public AudioSource water;

    public Slider bgmSlider;
    public Slider generalSlider;

    private void Awake()
    {
        stageManager = GameObject.Find("Manager").GetComponent<StageManager>();
    }

    void Start()
    {
        StopAudio();

        // 전체 볼륨 조절
        float bgmVolume = PlayerPrefs.GetFloat("BGMVolume", 1.0f);
        float genVolume = PlayerPrefs.GetFloat("GenVolume", 1.0f);

        bgmSlider.value = bgmVolume;
        generalSlider.value = genVolume;

        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            bgmMainMenu.volume = bgmVolume;

            startAudio.volume = genVolume;
            buttonAudio.volume = genVolume;
            bgmMainMenu.Play();
        }
        else if (SceneManager.GetActiveScene().name == "Loding")
        {
            bgmMainMenu.volume = bgmVolume;
        }
        else if (SceneManager.GetActiveScene().name == "Character")
        {
            bgmCharacterMenu.volume = bgmVolume;

            startAudio.volume = genVolume;
            buttonAudio.volume = genVolume;
        }
        else if (SceneManager.GetActiveScene().name == "Game")
        {
            bgmStage.volume = bgmVolume;
            bgmBossStage.volume = bgmVolume;
            bgmSelectMenu.volume = bgmVolume;
            bgmResultMenu.volume = bgmVolume;
        }
    }

    void Update()
    {
        // 전체 볼륨 조절
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            bgmMainMenu.volume = bgmSlider.value;

            startAudio.volume = generalSlider.value;
            buttonAudio.volume = generalSlider.value;
        }
        else if (SceneManager.GetActiveScene().name == "Loding")
        {
            bgmMainMenu.volume = bgmSlider.value;
        }
        else if (SceneManager.GetActiveScene().name == "Character")
        {
            bgmCharacterMenu.volume = bgmSlider.value;

            startAudio.volume = generalSlider.value;
            buttonAudio.volume = generalSlider.value;
        }
        else if (SceneManager.GetActiveScene().name == "Game")
        {
            bgmStage.volume = bgmSlider.value;
            bgmBossStage.volume = bgmSlider.value;
            bgmSelectMenu.volume = bgmSlider.value;
            bgmResultMenu.volume = bgmSlider.value;

            attackAudio.volume = generalSlider.value;
            defenseAudio.volume = generalSlider.value;
            hitAudio.volume = generalSlider.value;
           // monsterAttackAudio.volume = generalSlider.value;
            buttonAudio.volume = generalSlider.value;

            fireAudio.volume = generalSlider.value;
            fireShotAudio.volume = generalSlider.value;
            holyShotAudio.volume = generalSlider.value;
            holyWaveAudio.volume = generalSlider.value;
            meleeAudio.volume = generalSlider.value;
            posionAudio.volume = generalSlider.value;
            rockAudio.volume = generalSlider.value;
            sturnAudio.volume = generalSlider.value;

            water.volume = generalSlider.value;
        }

        PlayerPrefs.SetFloat("BGMVolume", bgmSlider.value);
        PlayerPrefs.SetFloat("GenVolume", generalSlider.value);
    }

    // 소리 재생
    public void AttackAudio()
    {
        attackAudio.Play();
    }
    public void DefenseAudio()
    {
        defenseAudio.Play();
    }
    public void HitAudio()
    {
        hitAudio.Play();
    }
    public void MonsterAttackAudio()
    {
        monsterAttackAudio.Play();
    }
    public void ButtonAudio()
    {
        buttonAudio.Play();
    }
    public void StartAudio()
    {
        startAudio.Play();
    }

    // Item
    public void FireAudio()
    {
        fireAudio.Play();
    }
    public void FireShotAudio()
    {
        fireShotAudio.Play();
    }
    public void HolyShotAudio()
    {
        holyShotAudio.Play();
    }
    public void HolyWaveAudio()
    {
        holyWaveAudio.Play();
    }
    public void MeleeAudio()
    {
        meleeAudio.Play();
    }
    public void PosionAudio()
    {
        posionAudio.Play();
    }
    public void RockAudio()
    {
        rockAudio.Play();
    }
    public void SturnAudio()
    {
        sturnAudio.Play();
    }

    // Character
    public void WaterAudio()
    {
        water.Play();
    }

    // 시작시 소리 중복 제거용
    void StopAudio()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            buttonAudio.Stop();
            startAudio.Stop();
        }
        else if (SceneManager.GetActiveScene().name == "Character")
        {
            buttonAudio.Stop();
            startAudio.Stop();
        }
        else if (SceneManager.GetActiveScene().name == "Game")
        {
            if (stageManager.mainStage <= 8)
            {
                if (stageManager.subStage == 5)
                {
                    bgmStage.Stop();
                    bgmBossStage.Play();
                }
                else
                {
                    bgmBossStage.Stop();
                    bgmStage.Play();
                }
            }
            else
            {
                bgmStage.Stop();
                bgmBossStage.Play();
            }

            bgmBossStage.Stop();
            bgmSelectMenu.Stop();
            bgmResultMenu.Stop();

            attackAudio.Stop();
            defenseAudio.Stop();
            hitAudio.Stop();
            monsterAttackAudio.Stop();
            buttonAudio.Stop();

            fireAudio.Stop();
            fireShotAudio.Stop();
            holyShotAudio.Stop();
            holyWaveAudio.Stop();
            meleeAudio.Stop();
            posionAudio.Stop();
            rockAudio.Stop();
            sturnAudio.Stop();

            water.Stop();
        }
    }
}