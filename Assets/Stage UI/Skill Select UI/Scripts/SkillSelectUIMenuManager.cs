using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SkillSelectUIMenuManager : MonoBehaviour 
{
    public static SkillSelectUIMenuManager Instance;

    [Header("Menue UI")]
    [SerializeField] Transform skillMenuSpace; //스킬 선택 메뉴 Transform
    [SerializeField] GameObject[] skillMenuTypePrefab;

    [Header("About Reset")]
    [SerializeField] Button skillResetButton;

    [Header("Acquired Skill UI")]
    [SerializeField] Transform[] acquiredSkillSpace;
    [SerializeField] GameObject skillImagePrefab;

    bool isThereUpgradeableSkillMenu = true;

    int menueCreatCount = 3;

    int separationIndex = (int)PlayerSkillCategory.TOP_ACTIVE_BOOMERANG;

    Queue<GameObject> currentSkillMenuQueue = new Queue<GameObject>();
    Dictionary<int, GameObject> skiilIconDictionary = new Dictionary<int, GameObject>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(false);

        skillResetButton.onClick.AddListener(OnClickResetSkillMenueButton);
    }

    //플레이어가 선택한 무기 등록
    void InitializedWeaponIcon()
    {
        int _playerWeaponNumber = PlayerWeaponController.Instance.PlayerWeaponType;

        GameObject creatNew = GetSkillMenu(_playerWeaponNumber, 0);
        creatNew.SetActive(false);
        CreatAcquiredSkillIcon(_playerWeaponNumber);
    }

    //스킬 선택 UI Open 
    public void OpenSkillSelectUI(bool _open)
    {
        transform.GetChild(0).gameObject.SetActive(_open);

        AudioManager.Instance.SkillSelectUIOpen(_open);

        if (_open)
        {
            if (skiilIconDictionary.Count == 0)
                InitializedWeaponIcon();

            DestroySkillMenuUI();

            RegisterSkillMenueUI();
        }
    }

    //메뉴 타입 결정 및 보너스메뉴 생성여부 결정
    void RegisterSkillMenueUI()
    {
        List<int> randomSkillTypeNumberList = new List<int>();
       
        int _randomMenuIndex = 0;

        while (randomSkillTypeNumberList.Count < menueCreatCount && isThereUpgradeableSkillMenu)
        {
            CheckCreatMenueCount();

            _randomMenuIndex = (int)RandomMenu.GetRandomSkillMenuInfo(ComboSkillManager.Instance.currentSkillConditionDictionary);

            if (_randomMenuIndex == -1)
            {
                isThereUpgradeableSkillMenu = false;
                skillResetButton.gameObject.SetActive(false);
                break;
            }
            else
            {
                int duplicateCount = randomSkillTypeNumberList.Count(type => type == _randomMenuIndex);

                if (duplicateCount == 0)
                    randomSkillTypeNumberList.Add(_randomMenuIndex);
                else
                    continue;
            }
        }

        if (isThereUpgradeableSkillMenu)
        {
            for (int i = 0; i < menueCreatCount; ++i)
            {
                int _skillLevel = ComboSkillManager.Instance.currentSkillConditionDictionary[randomSkillTypeNumberList[i]].level + 1;
                GameObject _newMenu = GetSkillMenu(randomSkillTypeNumberList[i], _skillLevel);

                _newMenu.SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < 2 ; ++i)
            {
                int bonusEnumNumber = (int)PlayerSkillCategory.TOP_BONUS_GOLD;
                GameObject _bonusMenu = GetSkillMenu(bonusEnumNumber + i, 0);
                _bonusMenu.SetActive(true);
            }
        }
    }


    void CheckCreatMenueCount()
    {
        int count = (int)RandomMenu.CheckExportMenueCount(ComboSkillManager.Instance.currentSkillConditionDictionary);

        if (count >= 3) //예정대로 3개 생성
            menueCreatCount = 3;
        else// 0개 : 보너스 스킬 , 1 ~ 2개: 메뉴생성 카운트 변경
            menueCreatCount = count;
    }

    //선택 메뉴 리셋
    void OnClickResetSkillMenueButton() 
    {
        AudioManager.Instance.OnClickButtonAudioEvent();

        DestroySkillMenuUI();

        RegisterSkillMenueUI();
    }

    //메뉴 생성
    GameObject GetSkillMenu(int _type, int _level)
    {
        GameObject _menu;

        if ((int)_type < separationIndex)
            _menu = Instantiate(skillMenuTypePrefab[0]);
        else if((int)_type >= separationIndex && (int)_type < (int)PlayerSkillCategory.TOP_BONUS_GOLD)
            _menu = (_level != 5) ? Instantiate(skillMenuTypePrefab[1]) : Instantiate(skillMenuTypePrefab[2]);
        else
            _menu = Instantiate(skillMenuTypePrefab[3]);

        _menu.GetComponent<SkillMenu>().SkillMenuInitialize(PlayerSkillCategory.TOP_PASSIVE_EXO_BRACER + _type, _level); 
        _menu.transform.SetParent(skillMenuSpace);
        _menu.transform.localScale = Vector3.one;
        _menu.SetActive(false);
        currentSkillMenuQueue.Enqueue(_menu);
        return _menu;
    }

    //습득한 스킬 아이콘 등록
    public void RegisterAcquiredSkillIcon(int _typeNumber)
    {
        // 처음 단 한번 생성
        if (ComboSkillManager.Instance.currentSkillConditionDictionary[_typeNumber].level == -1)
            CreatAcquiredSkillIcon(_typeNumber);

        bool isActiveSkill = !(_typeNumber < separationIndex);
        bool isNeedUpdate = isActiveSkill && ComboSkillManager.Instance.currentSkillConditionDictionary[_typeNumber].level == 4;

        if (!isNeedUpdate)
            return;

        //[ActiveSkill 한정] Final Level 도달시 업데이트
        UpdateAcquiredSkillIcon(_typeNumber);
    }

    //습득한 스킬 아이콘 생성
    void CreatAcquiredSkillIcon(int _typeNumber)
    {
        int spaceIndex = _typeNumber < separationIndex ? 0 : 1;

        GameObject skillImageObject = Instantiate(skillImagePrefab);
        skillImageObject.GetComponent<SkillIcon>().InitializedSkillIconImage(_typeNumber);
        skillImageObject.transform.SetParent(acquiredSkillSpace[spaceIndex]);
        skillImageObject.transform.localScale = Vector3.one;

        skiilIconDictionary.Add(_typeNumber, skillImageObject);

    }

    //습득한 스킬 아이콘 업데이트
    void UpdateAcquiredSkillIcon(int _typeNumber)
    {
        //아이콘 이미지 변경
        skiilIconDictionary[_typeNumber].gameObject.GetComponent<SkillIcon>().UpdateSkillIconImage();

        // 드론  레드 등급 도달시 액티브 슬롯 - 1 & 미선택 스킬 해금
        if (_typeNumber == (int)PlayerSkillCategory.ACTIVE_DRONE_A || _typeNumber == (int)PlayerSkillCategory.ACTIVE_DRONE_B)
        {
            int destroyType = _typeNumber == (int)PlayerSkillCategory.ACTIVE_DRONE_A ? (int)PlayerSkillCategory.ACTIVE_DRONE_B : (int)PlayerSkillCategory.ACTIVE_DRONE_A;

            Destroy(skiilIconDictionary[destroyType].gameObject);
            skiilIconDictionary.Remove(destroyType);

            ComboSkillManager.Instance.RemoveAcquiredSkillCount();
        }
    }

    //사용한 메뉴 정리
    void DestroySkillMenuUI()
    {
        foreach(GameObject menu in currentSkillMenuQueue)
            Destroy(menu);

        currentSkillMenuQueue.Clear();
    }
}
