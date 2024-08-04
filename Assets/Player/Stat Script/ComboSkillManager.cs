using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerSkillCategory
{
    TOP_PASSIVE_EXO_BRACER,
    PASSIVE_HI_POWEREDBULLET,
    PASSIVE_NINJASCROLL,
    PASSIVE_IRON_ARMOR,
    PASSIVE_ENERGYDRINK,
    PASSIVE_ENERGYCUBE,
    PASSIVE_OILBONDS,
    PASSIVE_AMOTHURSTER,
    PASSIVE_SNEAKERS,
    PASSIVE_HIPOWERMAGNET,
    PASSIVE_FITNESSGUIDE,
    PASSIVE_HE_FUEL,

    TOP_ACTIVE_BOOMERANG,
    ACTIVE_BRICK,
    ACTIVE_DRILLSHOT,
    ACTIVE_DRONE_A,
    ACTIVE_DRONE_B,
    ACTIVE_DURIAN,
    ACTIVE_GARDIAN,
    ACTIVE_LIGHTNINGEMITTER,
    ACTIVE_MELTHINGSHIELD,
    ACTIVE_MOLOTOVCOCKTAIL,
    ACTIVE_ROCKETLAUNCHER,
    ACTIVE_SOCCERBALL,

    TOP_WEAPON_BAZOOKA,
    WEAPON_REVOLVER,
    WEAPON_KUNAI,
    WEAPON_SWORD,

    MAX_SIZE,

    TOP_BONUS_GOLD = 50,
    BONUS_MEAT,
}

public struct ComboSkillData
{
    public PlayerSkillCategory Type;
    public int level;
}

[System.Serializable]
public class SkillInfo
{
    public int level; // Default:-1(미습득스킬레벨)
    public bool isConditionMet; //스킬 조건 충족(조건에 해당하는 스킬을 얻었는가)
    public int weight; // 레벨 4&조건 충족 => Weight 200 ,그 외엔 모두 n/%  & 이미 조합된 스킬은 0
    public bool isLock; //습득 최대갯수 초과 시 얻은스킬 외엔 Lock(T) & 레벨4 도달했는데 조건 미충족한 스킬은 Lock (T); 후에 충족시 (F) & 레벨 5도달시 Lock(T)
}

public class ComboSkillManager : MonoBehaviour
{
    public static ComboSkillManager Instance;

    PlayerWeaponController m_playerController;

    PlayerStat m_playerStat;

    ScoreManager m_scoreManager;

    [SerializeField] GameObject[] activeSkillControllerPrefabs;

    public Dictionary<int, SkillInfo> currentSkillConditionDictionary = new Dictionary<int, SkillInfo>();

    [SerializeField] int curentPassiveSkillAcquiredCount = 0;
    [SerializeField] int curentActiveSkillAcquiredCount = 0;
    int numberOfMaxAcquirableSkillCount = 6;

    List<GameObject>[] activeSkillControllerList = new List<GameObject>[(int)PlayerSkillCategory.MAX_SIZE];//액티브 스킬 기능 프리펩 , 무기 제외

    static Queue<ComboSkillData>[] comboSkillDataQueue = new Queue<ComboSkillData>[(int)PlayerSkillCategory.MAX_SIZE];

    private void Awake()
    {
        Instance = this;

        m_playerController = GetComponent<PlayerWeaponController>();

        m_playerStat = GetComponent<PlayerStat>();

        m_scoreManager = GameObject.FindObjectOfType<ScoreManager>();

        for (int i = 0; i < (int)PlayerSkillCategory.MAX_SIZE; ++i)
        {
            activeSkillControllerList[i] = new List<GameObject>();

            comboSkillDataQueue[i] = new Queue<ComboSkillData>();

            SkillInfo skillInfo = new SkillInfo();
            skillInfo.level = -1;

            skillInfo.isConditionMet = false;

            skillInfo.weight = 100 / (int)PlayerSkillCategory.MAX_SIZE;
            skillInfo.isLock = false;

            //사용하는 Weapon 외에는 제외
            bool weaponCategory = (i >= (int)PlayerSkillCategory.TOP_WEAPON_BAZOOKA) && (i < (int)PlayerSkillCategory.MAX_SIZE);
            if (weaponCategory && i != PlayerWeaponController.Instance.PlayerWeaponType)
            {
                skillInfo.weight = 0;
                skillInfo.isLock = true;
            }

            currentSkillConditionDictionary.Add(i, skillInfo);
        }
    }
    private void Update()
    {
        for (int i = 0; i < (int)PlayerSkillCategory.MAX_SIZE; ++i)
        {
            if (comboSkillDataQueue[i].Count > 0)
            {
                ComboSkillData data = comboSkillDataQueue[i].Dequeue();

                UpdateSkillCondition(data);

                if ((int)data.Type < (int)PlayerSkillCategory.TOP_ACTIVE_BOOMERANG)
                    UpdatePassiveSkill(data);
                else
                    UpdateActiveSkill(data);
            }
        }
    }

    static public void AddComboSkill(ComboSkillData _comboSkillData)
    {
        comboSkillDataQueue[(int)_comboSkillData.Type].Enqueue(_comboSkillData);
    }

    void UpdatePassiveSkill(ComboSkillData data)
    {
        string dataTypeKey = (int)data.Type + "";
        float passiveSkillValue = DataManager.Instance.SkillAbilityDataDictionary[dataTypeKey].skillImpactValueOfLevel[data.level];

        if (data.Type == PlayerSkillCategory.PASSIVE_HI_POWEREDBULLET || data.Type == PlayerSkillCategory.PASSIVE_IRON_ARMOR || data.Type == PlayerSkillCategory.PASSIVE_ENERGYDRINK
                    || data.Type == PlayerSkillCategory.PASSIVE_SNEAKERS || data.Type == PlayerSkillCategory.PASSIVE_HIPOWERMAGNET || data.Type == PlayerSkillCategory.PASSIVE_FITNESSGUIDE)
        {
            m_playerStat.DoPassiveBuff(data.Type, passiveSkillValue);
        }
        else if (data.Type == PlayerSkillCategory.TOP_PASSIVE_EXO_BRACER || data.Type == PlayerSkillCategory.PASSIVE_ENERGYCUBE
            || data.Type == PlayerSkillCategory.PASSIVE_AMOTHURSTER || data.Type == PlayerSkillCategory.PASSIVE_HE_FUEL)
        {
            ActiveComboSkill tempActiveComboSkill = activeSkillControllerPrefabs[0].GetComponent<ActiveComboSkill>();
            tempActiveComboSkill.DoPassiveBuff(data.Type, passiveSkillValue);

            m_playerController.DoPassiveBuff(data.Type, passiveSkillValue);
        }
        else if (data.Type == PlayerSkillCategory.PASSIVE_NINJASCROLL || data.Type == PlayerSkillCategory.PASSIVE_OILBONDS)
        {
            m_scoreManager.DoPassiveBuff(data.Type, passiveSkillValue);
        }
    }

    void UpdateActiveSkill(ComboSkillData data)
    {
        if (data.Type >= PlayerSkillCategory.TOP_WEAPON_BAZOOKA)
        {
            m_playerController.LevellingUpWeapon((int)data.level);
        }
        else
        {
            if (activeSkillControllerList[(int)data.Type].Count > 0)
            {
                activeSkillControllerList[(int)data.Type][0].GetComponent<ActiveComboSkill>().LevelUp(data.level);
            }
            else //새로 추가된 경우(레벨 0인경우)
            {
                int index = (int)data.Type - (int)PlayerSkillCategory.TOP_ACTIVE_BOOMERANG;
                GameObject _object = Instantiate(activeSkillControllerPrefabs[index]);
                _object.SetActive(true);
                _object.transform.SetParent(transform);
                _object.transform.localPosition = Vector3.zero;
                activeSkillControllerList[(int)data.Type].Add(_object);

                activeSkillControllerList[(int)data.Type][0].GetComponent<ActiveComboSkill>().LevelUp(data.level);
            }
        }
    }

    void UpdateSkillCondition(ComboSkillData skillData)
    {
        //조합스킬 Type Index 가져오기
        Queue<int> requireSkillNumberQueue = new Queue<int>();
        requireSkillNumberQueue = GetReqireSkillTyeNumber((int)skillData.Type);

        bool isPassiveSkill = (int)skillData.Type < (int)PlayerSkillCategory.TOP_ACTIVE_BOOMERANG;

        //스킬 레벨 업데이트
        currentSkillConditionDictionary[(int)skillData.Type].level = skillData.level;

        //스킬 습득 카운트 업데이트(0레벨일때 한번만)
        if (skillData.level == 0)
            AddAcquiredSkillCount(isPassiveSkill);

        if (isPassiveSkill) //패시브
        {
            // 패시브 최대 레벨도달 시 Lock
            if (skillData.level == 4)
                currentSkillConditionDictionary[(int)skillData.Type].isLock = true;

            foreach(int reqireSkillNumber in requireSkillNumberQueue)
            {
                if (currentSkillConditionDictionary[reqireSkillNumber].level > -1 && currentSkillConditionDictionary[reqireSkillNumber].level < 5)
                {
                    currentSkillConditionDictionary[reqireSkillNumber].isConditionMet = true;

                    currentSkillConditionDictionary[reqireSkillNumber].isLock = false;
                }
            }
        }
        else //액티브
        {
            // 액티브 최대 레벨도달 시 Lock
            if (skillData.level == 5)
            {
                currentSkillConditionDictionary[(int)skillData.Type].isLock = true;
                currentSkillConditionDictionary[(int)skillData.Type].weight = 0;
            }
            else if (skillData.level == 4)
            {
                currentSkillConditionDictionary[(int)skillData.Type].isLock = !currentSkillConditionDictionary[(int)skillData.Type].isConditionMet;
                currentSkillConditionDictionary[(int)skillData.Type].weight = 200;
            }

            bool isDroneSkill = ((int)skillData.Type == (int)PlayerSkillCategory.ACTIVE_DRONE_A || (int)skillData.Type == (int)PlayerSkillCategory.ACTIVE_DRONE_B) ? true : false;

            foreach (int reqireSkillNumber in requireSkillNumberQueue)
            {
                //드론 제외 나머지 스킬 해금 조건 : 습득여부 , 드론 스킬 해금 조건 : 요구스킬의 레벨 4
                bool isConditionMet = (!isDroneSkill && currentSkillConditionDictionary[reqireSkillNumber].level > -1 ) ||
                    (isDroneSkill && currentSkillConditionDictionary[reqireSkillNumber].level == 4);
               
                if (isConditionMet)
                    currentSkillConditionDictionary[(int)skillData.Type].isConditionMet = true;

                //습득한 드론스킬이 최고 등급 도달 시, 요구 드론스킬은 리셋(다시 선택 가능)
                if (isDroneSkill && skillData.level == 5)
                    DoResetSkill(reqireSkillNumber);
            }
        }
    }

    public void CheckStagePlayState(bool isTimeToReady) 
    {
        // isTimeToReady(true): 보스전 시작전 레디 돌입 || 노멀전 시작 전 레디 돌입
        // isTimeToReady(false): 레디상태에서 보스전 돌입 || 레디 상태에서 노멀전 돌입

        m_playerController.CheckSkillActiveTime(isTimeToReady);

        foreach (KeyValuePair<int, SkillInfo> kvp in currentSkillConditionDictionary)
        {
            if (kvp.Key >= (int)PlayerSkillCategory.TOP_ACTIVE_BOOMERANG  && kvp.Key < (int)PlayerSkillCategory.TOP_WEAPON_BAZOOKA)
            {
                if(kvp.Value.level >= 0)
                {
                    ActiveComboSkill activeComboSkill = activeSkillControllerList[kvp.Key][0].GetComponent<ActiveComboSkill>();
                    activeComboSkill.SetToReady(isTimeToReady);
                }
            }
            else
                continue;
        }
    }


    //  습득스킬 카운트 Add
    void AddAcquiredSkillCount(bool isPassive)
    {
        // 습득가능 스킬 카운트 감소 (=습득한 스킬 카운트 증가)
        if (isPassive && curentPassiveSkillAcquiredCount < numberOfMaxAcquirableSkillCount)
            curentPassiveSkillAcquiredCount++;
        else if (!isPassive && curentActiveSkillAcquiredCount < numberOfMaxAcquirableSkillCount)
            curentActiveSkillAcquiredCount++;

        // 최대 스킬선택 수량 도달시, 선택되지 않은 스킬은 모두 Lock(T)
        for (int i = 0; i < (int)PlayerSkillCategory.MAX_SIZE; ++i)
        {
            int acquiredCount = (i < 12) ? curentPassiveSkillAcquiredCount : curentActiveSkillAcquiredCount;

            if ((acquiredCount >= numberOfMaxAcquirableSkillCount) && currentSkillConditionDictionary[i].level == -1)
                currentSkillConditionDictionary[i].isLock = true;
        }
    }

    //  습득스킬 카운트 Remove
    public void RemoveAcquiredSkillCount()
    {
        //액티브 스킬 간의 조합시, 습득가능 스킬 카운트 증가

        curentActiveSkillAcquiredCount--;

        for (int i = 12; i < (int)PlayerSkillCategory.MAX_SIZE; ++i)
        {
            bool isSelectedSkill = currentSkillConditionDictionary[i].level != -1;
            bool comboSkillCategory = i < (int)PlayerSkillCategory.TOP_WEAPON_BAZOOKA;

            if (!isSelectedSkill && (comboSkillCategory || i == PlayerWeaponController.Instance.PlayerWeaponType))
                currentSkillConditionDictionary[i].isLock = false;
        }
    }

    Queue<int> GetReqireSkillTyeNumber(int _gainedSkillNumber)
    {
        IReadOnlyList<bool>[] tempLinkSkillCheckDataDictionary = DataManager.Instance.LinkSkillCheckDataDictionary;

        Queue<int> reqiureSkillNumber = new Queue<int>();

        for (int i = 0; i < tempLinkSkillCheckDataDictionary.Length; i++)
        {
            if (tempLinkSkillCheckDataDictionary[_gainedSkillNumber][i])
            {
                reqiureSkillNumber.Enqueue(i);
            }
        }

        return reqiureSkillNumber;
    }

    void DoResetSkill(int _skillType)
    {
        currentSkillConditionDictionary[_skillType].level = -1;
        currentSkillConditionDictionary[_skillType].isLock = false;
        currentSkillConditionDictionary[_skillType].isConditionMet = false;
        currentSkillConditionDictionary[_skillType].weight = 100 / (int)PlayerSkillCategory.MAX_SIZE;

        foreach (var obj in activeSkillControllerList[_skillType])
        {
            if (obj.gameObject)
                Destroy(obj.gameObject);
        }

        activeSkillControllerList[_skillType].Clear();
    }
}
