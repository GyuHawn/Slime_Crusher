using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingMenuManager : MonoBehaviour
{
    public static SettingMenuManager Instance { get; private set; }

    public GameObject settingMenu; // 옵션 메뉴
    public float moveDuration; // 옵션창 이동시간
    private Vector3 startMenuPos; // 옵션창 시작 위치
    private Vector3 endMenuPos; // 옵션창 이동 후 위치
    private bool onSetting; // 옵션 메뉴 활성화 상태

    private Coroutine moveCoroutine; // 이동 코루틴 참조

    public RectTransform settingMenuRectTransform;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시 파괴 방지
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        moveDuration = 1.0f;
    }

    public void InitializeOptionMenu(GameObject menu)
    {
        settingMenuRectTransform = menu.GetComponent<RectTransform>(); // RectTransform 가져오기
        if (settingMenuRectTransform != null)
        {
            if (SceneManager.GetActiveScene().name == "MainMenu")
            {
                // 초기 위치 설정
                startMenuPos = new Vector3(730f, settingMenuRectTransform.localPosition.y, settingMenuRectTransform.localPosition.z);
                endMenuPos = new Vector3(389f, settingMenuRectTransform.localPosition.y, settingMenuRectTransform.localPosition.z);
                onSetting = false;
            }
            else if(SceneManager.GetActiveScene().name == "Game")
            {
                // 초기 위치 설정
                startMenuPos = new Vector3(600f, settingMenuRectTransform.localPosition.y, settingMenuRectTransform.localPosition.z);
                endMenuPos = new Vector3(0f, settingMenuRectTransform.localPosition.y, settingMenuRectTransform.localPosition.z);
                onSetting = false;
            }
        }
    }

    public void ToggleSettingMenu()
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine); // 이전 이동 코루틴 중지
        }
        moveCoroutine = StartCoroutine(MoveSettingMenu());
    }

    private IEnumerator MoveSettingMenu()
    {
        float elapsed = 0f;
        Vector3 targetPos = onSetting ? startMenuPos : endMenuPos;
        Vector3 startPos = onSetting ? endMenuPos : startMenuPos;

        while (elapsed < moveDuration)
        {
            settingMenu.transform.localPosition = Vector3.Lerp(startPos, targetPos, elapsed / moveDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        settingMenu.transform.localPosition = targetPos;
        onSetting = !onSetting;
    }
}
