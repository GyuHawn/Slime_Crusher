using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    private StageManager stageManager;

    // BGM
    public AudioSource bgmMainMenu;
    public AudioSource bgmCharacterMenu;
    public AudioSource bgmStage;
    public AudioSource bgmBossStage;
    public AudioSource bgmSelectMenu;
    public AudioSource bgmResultMenu;

    // 기능
    public AudioSource attackAudio;
    public AudioSource defenseAudio;
    public AudioSource hitAudio;
    public AudioSource monsterAttackAudio;
    public AudioSource buttonAudio;
    public AudioSource startAudio;

    // 아이템
    public AudioSource fireAudio;
    public AudioSource fireShotAudio;
    public AudioSource holyShotAudio;
    public AudioSource holyWaveAudio;
    public AudioSource meleeAudio;
    public AudioSource poisonAudio;
    public AudioSource rockAudio;
    public AudioSource stunAudio;

    // 캐릭터
    public AudioSource water;

    // 오디오 슬라이더
    public Slider bgmSlider;
    public Slider generalSlider;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        FindSlider();
        StopAllAudio();
        VolumeSetting();
    }

    void VolumeSetting()
    {
        float bgmVolume = PlayerPrefs.GetFloat("BGMVolume", 1.0f);
        float genVolume = PlayerPrefs.GetFloat("GenVolume", 1.0f);

        bgmSlider.value = bgmVolume;
        generalSlider.value = genVolume;

        UpdateVolume();
    }

    private void Update()
    {
        FindSlider();
        UpdateVolume();
    }

    void FindSlider()
    {
        if (bgmSlider == null)
        {
            GameObject bgmSliderObject = GameObject.Find("BGMSlider");
            if (bgmSliderObject != null)
                bgmSlider = bgmSliderObject.GetComponent<Slider>();
        }

        if (generalSlider == null)
        {
            GameObject generalSliderObject = GameObject.Find("GenSlider");
            if (generalSliderObject != null)
                generalSlider = generalSliderObject.GetComponent<Slider>();
        }
    }

    void UpdateVolume()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName == "MainMenu")
        {
            bgmMainMenu.volume = bgmSlider.value;
            startAudio.volume = generalSlider.value;
            buttonAudio.volume = generalSlider.value;
        }
        else if (sceneName == "Loading")
        {
            bgmMainMenu.volume = bgmSlider.value;
        }
        else if (sceneName == "Character")
        {
            bgmCharacterMenu.volume = bgmSlider.value;
            startAudio.volume = generalSlider.value;
            buttonAudio.volume = generalSlider.value;
        }
        else if (sceneName == "Game")
        {
            bgmStage.volume = bgmSlider.value;
            bgmBossStage.volume = bgmSlider.value;
            bgmSelectMenu.volume = bgmSlider.value;
            bgmResultMenu.volume = bgmSlider.value;

            attackAudio.volume = generalSlider.value;
            defenseAudio.volume = generalSlider.value;
            hitAudio.volume = generalSlider.value;
            buttonAudio.volume = generalSlider.value;

            fireAudio.volume = generalSlider.value;
            fireShotAudio.volume = generalSlider.value;
            holyShotAudio.volume = generalSlider.value;
            holyWaveAudio.volume = generalSlider.value;
            meleeAudio.volume = generalSlider.value;
            poisonAudio.volume = generalSlider.value;
            rockAudio.volume = generalSlider.value;
            stunAudio.volume = generalSlider.value;

            water.volume = generalSlider.value;
        }

        PlayerPrefs.SetFloat("BGMVolume", bgmSlider.value);
        PlayerPrefs.SetFloat("GenVolume", generalSlider.value);
    }

    // BGM 실행 메서드
    public void PlayMainMenuAudio()
    {
        StopAllAudio();
        bgmMainMenu.Play();
    }

    public void PlayCharacterMenuAudio()
    {
        StopAllAudio();
        bgmCharacterMenu.Play();
    }

    public void PlayStageAudio()
    {
        StopAllAudio();
        bgmStage.Play();
    }

    public void PlayBossAudio()
    {
        StopAllAudio();
        bgmBossStage.Play();
    }

    public void PlaySelectMenuAudio()
    {
        StopAllAudio();
        bgmSelectMenu.Play();
    }

    public void PlayResultAudio()
    {
        StopAllAudio();
        bgmResultMenu.Play();
    }

    // 소리 재생 메서드
    public void PlayAttackAudio()
    {
        attackAudio.Play();
    }

    public void PlayDefenseAudio()
    {
        defenseAudio.Play();
    }

    public void PlayHitAudio()
    {
        hitAudio.Play();
    }

    public void PlayMonsterAttackAudio()
    {
        monsterAttackAudio.Play();
    }

    public void PlayButtonAudio()
    {
        buttonAudio.Play();
    }

    public void PlayStartAudio()
    {
        startAudio.Play();
    }

    // 아이템 소리 재생 메서드
    public void PlayFireAudio()
    {
        fireAudio.Play();
    }

    public void PlayFireShotAudio()
    {
        fireShotAudio.Play();
    }

    public void PlayHolyShotAudio()
    {
        holyShotAudio.Play();
    }

    public void PlayHolyWaveAudio()
    {
        holyWaveAudio.Play();
    }

    public void PlayMeleeAudio()
    {
        meleeAudio.Play();
    }

    public void PlayPoisonAudio()
    {
        poisonAudio.Play();
    }

    public void PlayRockAudio()
    {
        rockAudio.Play();
    }

    public void PlayStunAudio()
    {
        stunAudio.Play();
    }

    // 캐릭터 소리 재생 메서드
    public void PlayWaterAudio()
    {
        water.Play();
    }

    // 시작시 소리 중복 제거용
    void StopAllAudio()
    {
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audioSource in allAudioSources)
        {
            audioSource.Stop();
        }
    }
}
