using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using UnityEditorInternal.Profiling.Memory.Experimental;

public class SelectItem : MonoBehaviour
{
    private StageManager stageManager;
    private PlayerController playerController;
    private Character character;
    private ItemSkill itemSkill;

    public bool itemSelecting; // 아이템 선택중

    public GameObject[] items; // 전체 아이템
    public GameObject itemPos1; // 아이템 선택 표시 위치
    public GameObject itemPos2;
    public GameObject itemPos3;
    public List<GameObject> selectItems; // 아이템 선택지 3개 설정 리스트
    public List<GameObject> playerItems; // 획득한 아이템 리스트

    public int selectNum; // 선택한 아이템

    public GameObject selectItemPos1; // 획득한 아이템 표시
    public GameObject selectItemPos2;
    public GameObject selectItemPos3;
    public GameObject selectItemPos4;

    public TMP_Text itemName; // 선택한 아이템 이름과 설명
    public TMP_Text itemEx;
    public TMP_Text item1LvText; // 획득한 아이템 레벨 표시
    public TMP_Text item2LvText;
    public TMP_Text item3LvText;
    public TMP_Text item4LvText;
    
    public GameObject[] characters; // 캐릭터 리스트
    public GameObject charPos; // 캐릭터 표시 위치
    public bool selectChar;

    // 레벨관리
    public int passLv;
    public int fireLv;
    public int fireShotLv;
    public int holyWaveLv;
    public int holyShotLv;
    public int meleeLv;
    public int posionLv;
    public int rockLv;
    public int sturnLv;

    // 아이템 선택 관리
    public bool fireSelect;
    public bool fireShotSelect;
    public bool holyWaveSelect;
    public bool holyShotSelect;
    public bool meleeSelect;
    public bool posionSelect;
    public bool rockSelect;
    public bool sturnSelect;

    public GameObject selectItemMenu; // 아이템 선택 메뉴

    public bool selectedItem; // 아이템을 선택하였는지 확인

    public Canvas canvas;
  //  public GameObject getItemUIPos; // 생성한 아이템 오브젝트 부모용 오브젝트

    private void Awake()
    {
        character = GameObject.Find("Manager").GetComponent<Character>();
        stageManager = GameObject.Find("Manager").GetComponent<StageManager>();
        playerController = GameObject.Find("Manager").GetComponent<PlayerController>();
        itemSkill = GameObject.Find("Manager").GetComponent<ItemSkill>();
    }

    private void Start()
    {
        itemSelecting = false;
        selectChar = false;
    }

    private void Update()
    {
        ItemLevelOpen();
        CharacterInstant();
    }

    // 선택한 캐릭터 위치 선택
    void CharacterInstant()
    {
        if (!selectChar)
        {
            characters[character.currentCharacter - 1].transform.position = charPos.transform.position;

            selectChar = true;
        }
    }

    // 아이템 선택
    public void ItemSelect()
    {
        stageManager.selectingPass = true;
        playerController.isAttacking = true;
        selectItemMenu.SetActive(true);
        selectItems.Clear();

        itemSelecting = true;
        selectedItem = false;

        if (playerItems.Count >= 4)
        {
            while (selectItems.Count < 3)
            {
                int randomIndex = UnityEngine.Random.Range(0, playerItems.Count);
                GameObject selectedItem = playerItems[randomIndex];

                if (!selectItems.Contains(selectedItem))
                {
                    selectItems.Add(selectedItem);

                    GameObject itemFromItems = Array.Find(items, item => (item.name + "Pltem") == selectedItem.name);

                    while (itemFromItems == null || selectItems.Contains(itemFromItems))
                    {
                        randomIndex = UnityEngine.Random.Range(0, playerItems.Count);
                        selectedItem = playerItems[randomIndex];

                        if (!selectItems.Contains(selectedItem))
                        {
                            selectItems.RemoveAt(selectItems.Count - 1);
                            selectItems.Add(selectedItem);

                            itemFromItems = Array.Find(items, item => (item.name + "Pltem") == selectedItem.name);
                        }
                    }

                    // 선택한 아이템 알맞은 위치 설정
                    GameObject itemPos = null;

                    switch (selectItems.Count)
                    {
                        case 1:
                            itemPos = itemPos1;
                            break;
                        case 2:
                            itemPos = itemPos2;
                            break;
                        case 3:
                            itemPos = itemPos3;
                            break;
                        default:
                            itemPos = null;
                            break;
                    }


                    itemFromItems.transform.position = itemPos.transform.position;
                }
            }
        }
        else
        {
            while (selectItems.Count < 3)
            {
                int randomIndex = UnityEngine.Random.Range(0, items.Length);
                GameObject selectedItem = items[randomIndex];

                if (!selectItems.Contains(selectedItem) && !playerItems.Exists(item => item.name == selectedItem.name + "Pltem"))
                {
                    selectItems.Add(selectedItem);

                    // 선택한 아이템 알맞은 위치 설정
                    GameObject itemPos = null;

                    switch (selectItems.Count)
                    {
                        case 1:
                            itemPos = itemPos1;
                            break;
                        case 2:
                            itemPos = itemPos2;
                            break;
                        case 3:
                            itemPos = itemPos3;
                            break;
                        default:
                            itemPos = null;
                            break;
                    }


                    selectedItem.transform.position = itemPos.transform.position;
                }
            }
        }

        selectNum = UnityEngine.Random.Range(0, selectItems.Count) + 1;

        Time.timeScale = 0f;
    }

    // 아이템 선택 결정
    public void CloseMenu()
    {
        if (selectedItem)
        {
            bool isItemExist = false;

            foreach (GameObject item in playerItems)
            {
                if (item.name == items[selectNum - 1].name + "")
                {
                    isItemExist = true;
                    break;
                }
            }

            if (!isItemExist && playerItems.Count < 4)
            {            
                InstantiateItem();
            }

            foreach (GameObject selectItem in selectItems)
            {
                if (!playerItems.Contains(selectItem))
                {
                    selectItem.transform.position = new Vector3(0, 2000, 0);
                }
            }
            foreach (GameObject item in items)
            {
                item.transform.position = new Vector3(0, 2000, 0);
            }

            foreach (GameObject playerItem in playerItems)
            {
                playerItem.SetActive(true);
            }

            ItemTextClear();
            ItemLevelUp();
            //itemSkill.ItemValueUp();
            itemSkill.GetItem();    
            itemSelecting = false;
            stageManager.selectingPass = false;

            stageManager.passing = true;
            stageManager.NextSetting();

            playerController.isAttacking = false;
            Time.timeScale = 1f;
            selectItemMenu.SetActive(false);
        }
    }

    // 획득한 아이템 생성
    void InstantiateItem()
    {
        GameObject newItem = Instantiate(items[selectNum - 1], selectItemPos1.transform.position, Quaternion.identity);

        int playerItemsCount = playerItems.Count;

        switch (playerItemsCount)
        {
            case 0:
                newItem.transform.SetParent(selectItemPos1.transform, false);
                newItem.transform.localPosition = Vector3.zero;
                break;
            case 1:
                newItem.transform.SetParent(selectItemPos2.transform, false);
                newItem.transform.localPosition = Vector3.zero;
                break;
            case 2:
                newItem.transform.SetParent(selectItemPos3.transform, false);
                newItem.transform.localPosition = Vector3.zero;
                break;
            case 3:
                newItem.transform.SetParent(selectItemPos4.transform, false);
                newItem.transform.localPosition = Vector3.zero;
                break;
        }

        playerItems.Add(newItem);
        newItem.name = newItem.name.Replace("(Clone)", "Pltem");
    }


    // 획득한 아이템과 맞는 레벨 할당
    int GetItemLevel(GameObject item)
    {
        switch (item.name)
        {
            case "FirePltem": return fireLv;
            case "Fire ShotPltem": return fireShotLv;
            case "Holy WavePltem": return holyWaveLv;
            case "Holy ShotPltem": return holyShotLv;
            case "MeleePltem": return meleeLv;
            case "PosionPltem": return posionLv;
            case "RockPltem": return rockLv;
            case "SturnPltem": return sturnLv;
            default: return 0;
        }
    }

    // 획득한 아이템의 레벨표시 오픈
    void ItemLevelOpen()
    {
        item1LvText.gameObject.SetActive(playerItems.Count > 0);
        item1LvText.text = playerItems.Count > 0 ? GetItemLevel(playerItems[0]).ToString() : "";

        item2LvText.gameObject.SetActive(playerItems.Count > 1);
        item2LvText.text = playerItems.Count > 1 ? GetItemLevel(playerItems[1]).ToString() : "";

        item3LvText.gameObject.SetActive(playerItems.Count > 2);
        item3LvText.text = playerItems.Count > 2 ? GetItemLevel(playerItems[2]).ToString() : "";

        item4LvText.gameObject.SetActive(playerItems.Count > 3);
        item4LvText.text = playerItems.Count > 3 ? GetItemLevel(playerItems[3]).ToString() : "";
    }

    // 아이템 레벨업
    public void ItemLevelUp()
    {
        switch (selectNum)
        {
            case 1:
                fireLv++;
                break;
            case 2:
                fireShotLv++;
                break;
            case 3:
                holyWaveLv++;
                break;
            case 4:
                holyShotLv++;
                break;
            case 5:
                meleeLv++;
                break;
            case 6:
                posionLv++;
                break;
            case 7:
                rockLv++;
                break;
            case 8:
                sturnLv++;
                break;
        }
    }
    
    // 선택한 아이템 텍스트 초기화

    void ItemTextClear()
    {
        itemName.text = "";
        itemEx.text = "";
    }

    // 선택한 아이템 설명 표시
    public void Fire()
    {
        if (itemSelecting)
        {
            //items[0]
            selectedItem = true;
            selectNum = 1;
            itemName.text = "불";
            itemEx.text = "일정확률로 공격한 위치에 불기둥을 소환합니다.";
        }
    }
    public void FireShot()
    {
        if (itemSelecting)
        {
            //items[1]
            selectedItem = true;
            selectNum = 2;
            itemName.text = "폭탄";
            itemEx.text = "일정확률로 강력한 데미지를 주고 파편을 날려 주위에 데미지를 입힘니다.";
        }
    }
    public void HolyWave()
    {
        if (itemSelecting)
        {
            //items[2]
            selectedItem = true;
            selectNum = 3;
            itemName.text = "별";
            itemEx.text = "일정확률로 빛의 파동이 일렁이며, 일정 시간 동안 전체 적들에게 지속적인 피해를 입힙니다.";
        }
    }
    public void HolyShot()
    {
        if (itemSelecting)
        {
            //items[3]
            selectedItem = true;
            selectNum = 4;
            itemName.text = "전구";
            itemEx.text = "일정확률로 공격한 범위에 빛을 집중하여 일정 시간 동안 데미지를 입힘니다.";
        }
    }
    public void Melee()
    {
        if (itemSelecting)
        {
            //items[4]
            selectedItem = true;
            selectNum = 5;
            itemName.text = "난타";
            itemEx.text = "일정확률로 적에게 추가적으로 공격을 합니다.";
        }
    }
    public void Posion()
    {
        if (itemSelecting)
        {
            //items[5]
            selectedItem = true;
            selectNum = 6;
            itemName.text = "맹독";
            itemEx.text = "일정확률로 몬스터에게 일정 시간 동안 지속적으로 데미지를 입힘니다.";
        }
    }
    public void Rock()
    {
        if (itemSelecting)
        {
            //items[6]
            selectedItem = true;
            selectNum = 7;
            itemName.text = "벽돌";
            itemEx.text = "일정확률로 강력한 데미지를 입힘니다.";
        }
    }
    public void Sturn()
    {
        if (itemSelecting)
        {
            //items[7]
            selectedItem = true;
            selectNum = 8;
            itemName.text = "번개";
            itemEx.text = "일정확률로 적을 공격하여 일정 시간 동안 기절 상태로 만듭니다.";
        }
    }
}
