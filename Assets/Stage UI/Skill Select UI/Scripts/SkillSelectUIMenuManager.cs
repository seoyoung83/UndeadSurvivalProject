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
    [SerializeField] Transform skillMenuSpace; //��ų ���� �޴� Transform
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

    //�÷��̾ ������ ���� ���
    void InitializedWeaponIcon()
    {
        int _playerWeaponNumber = PlayerWeaponController.Instance.PlayerWeaponType;

        GameObject creatNew = GetSkillMenu(_playerWeaponNumber, 0);
        creatNew.SetActive(false);
        CreatAcquiredSkillIcon(_playerWeaponNumber);
    }

    //��ų ���� UI Open 
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

    //�޴� Ÿ�� ���� �� ���ʽ��޴� �������� ����
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

        if (count >= 3) //������� 3�� ����
            menueCreatCount = 3;
        else// 0�� : ���ʽ� ��ų , 1 ~ 2��: �޴����� ī��Ʈ ����
            menueCreatCount = count;
    }

    //���� �޴� ����
    void OnClickResetSkillMenueButton() 
    {
        AudioManager.Instance.OnClickButtonAudioEvent();

        DestroySkillMenuUI();

        RegisterSkillMenueUI();
    }

    //�޴� ����
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

    //������ ��ų ������ ���
    public void RegisterAcquiredSkillIcon(int _typeNumber)
    {
        // ó�� �� �ѹ� ����
        if (ComboSkillManager.Instance.currentSkillConditionDictionary[_typeNumber].level == -1)
            CreatAcquiredSkillIcon(_typeNumber);

        bool isActiveSkill = !(_typeNumber < separationIndex);
        bool isNeedUpdate = isActiveSkill && ComboSkillManager.Instance.currentSkillConditionDictionary[_typeNumber].level == 4;

        if (!isNeedUpdate)
            return;

        //[ActiveSkill ����] Final Level ���޽� ������Ʈ
        UpdateAcquiredSkillIcon(_typeNumber);
    }

    //������ ��ų ������ ����
    void CreatAcquiredSkillIcon(int _typeNumber)
    {
        int spaceIndex = _typeNumber < separationIndex ? 0 : 1;

        GameObject skillImageObject = Instantiate(skillImagePrefab);
        skillImageObject.GetComponent<SkillIcon>().InitializedSkillIconImage(_typeNumber);
        skillImageObject.transform.SetParent(acquiredSkillSpace[spaceIndex]);
        skillImageObject.transform.localScale = Vector3.one;

        skiilIconDictionary.Add(_typeNumber, skillImageObject);

    }

    //������ ��ų ������ ������Ʈ
    void UpdateAcquiredSkillIcon(int _typeNumber)
    {
        //������ �̹��� ����
        skiilIconDictionary[_typeNumber].gameObject.GetComponent<SkillIcon>().UpdateSkillIconImage();

        // ���  ���� ��� ���޽� ��Ƽ�� ���� - 1 & �̼��� ��ų �ر�
        if (_typeNumber == (int)PlayerSkillCategory.ACTIVE_DRONE_A || _typeNumber == (int)PlayerSkillCategory.ACTIVE_DRONE_B)
        {
            int destroyType = _typeNumber == (int)PlayerSkillCategory.ACTIVE_DRONE_A ? (int)PlayerSkillCategory.ACTIVE_DRONE_B : (int)PlayerSkillCategory.ACTIVE_DRONE_A;

            Destroy(skiilIconDictionary[destroyType].gameObject);
            skiilIconDictionary.Remove(destroyType);

            ComboSkillManager.Instance.RemoveAcquiredSkillCount();
        }
    }

    //����� �޴� ����
    void DestroySkillMenuUI()
    {
        foreach(GameObject menu in currentSkillMenuQueue)
            Destroy(menu);

        currentSkillMenuQueue.Clear();
    }
}
