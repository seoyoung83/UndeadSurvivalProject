using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public class GetLuckySkillInfo
{
    public int skillType;
    public int level;
    public bool doReward;
}

public class LuckySkillRouletteUI : Roulette
{
    [Header("About ItemBox Datas")]
    [SerializeField] GameObject[] itemBoxPrefab; //패시브, 액티브 ,레드

    List<GetLuckySkillInfo> getLuckySkill = new List<GetLuckySkillInfo>(); //선택된 아이템에 대한 데이터

    [Header("About Coin")]
    [SerializeField] TextMeshProUGUI coinText;
    Coroutine coinTextCoroutine;
    float textEffectDuration;
    int rewardCoinValue;

    public override void CreatRouletteItemBox(int _rouletteWinningCount)
    {
        selectedRouletteBoxIndexList = new List<int>();

        rouletteItemsList = new List<GameObject>();

        rouletteWinningCount = _rouletteWinningCount;

        coinText.text = "" + 0;

        textEffectDuration = rouletteWinningCount == 1 ? 3.25f : (rouletteWinningCount != 5 ? 9.25f : 9.5f);

        if (rouletteWinningCount == 1)
            rewardCoinValue = Random.Range(15, 26) * 10; //15-25
        else if (rouletteWinningCount == 3)
            rewardCoinValue = Random.Range(15, 41) * 10; //25~40
        else
            rewardCoinValue = Random.Range(40, 76) * 10;// 40~75 

        //룰렛 아이템들 초기 생성
        GetLuckySkillRouletteItem();

        if (getLuckySkill.Count != rouletteWinningCount || selectedRouletteBoxIndexList.Count == rouletteWinningCount)
            return;

        //생성한 박스 세팅
        SetRouletteItemBox();
    }

    // 룰렛 아이템(스킬)
    void GetLuckySkillRouletteItem()
    {
        getLuckySkill = new List<GetLuckySkillInfo>();

        while (getLuckySkill.Count < rouletteWinningCount)
        {
            int randomType = RandomWeightedRouletteItem.GetWeightedLuckySkillRouletteItem(rouletteWinningCount, ComboSkillManager.Instance.currentSkillConditionDictionary);

            if (randomType == -1)
            {
                //모든 오브젝트들 삭제
                foreach (var item in rouletteItemsList)
                    Destroy(item);

                rouletteItemsList.Clear();
                selectedRouletteBoxIndexList.Clear();
                getLuckySkill.Clear();

                // RouletteWinningCount 재설정
                RouletteUIManager.Instance.ResetWinningCount();
                break;
            }

            int thisSkillCurrentLevel = ComboSkillManager.Instance.currentSkillConditionDictionary[randomType].level;

            bool isPassiveSkill = randomType < (int)PlayerSkillCategory.TOP_ACTIVE_BOOMERANG;
            int limitLevel = isPassiveSkill || (!isPassiveSkill && !ComboSkillManager.Instance.currentSkillConditionDictionary[randomType].isConditionMet) ? 4 : 5;

            if (thisSkillCurrentLevel >= limitLevel)
                continue; //다시 랜더밍


            GetLuckySkillInfo foundTypeNumber = getLuckySkill.Find(Info => (int)Info.skillType == randomType);
            int existingIndex = getLuckySkill.FindIndex(Info => (int)Info.skillType == randomType); 
            int duplicateCount = getLuckySkill.Count(Info => (int)Info.skillType == randomType);

            if (foundTypeNumber != null && (int)foundTypeNumber.skillType == randomType) 
            {
                //기존에 있는 스킬 타입 과 추가 되는 타입 두개 합쳐도 초과 레벨을 넘지 않는지 확인
                if (limitLevel < thisSkillCurrentLevel + duplicateCount + 1)
                    continue;  //다시 랜더밍
                else
                {
                    getLuckySkill[existingIndex].doReward = false;

                    GetLuckySkillInfo getLuckySkillInfo = new GetLuckySkillInfo();
                    getLuckySkillInfo.skillType = randomType;
                    getLuckySkillInfo.level = thisSkillCurrentLevel + duplicateCount + 1;
                    getLuckySkillInfo.doReward = true;

                    getLuckySkill.Add(getLuckySkillInfo);
                }
            }
            else
            {
                GetLuckySkillInfo getLuckySkillInfo = new GetLuckySkillInfo();
                getLuckySkillInfo.skillType = randomType;
                getLuckySkillInfo.level = thisSkillCurrentLevel + 1;
                getLuckySkillInfo.doReward = true;

                getLuckySkill.Add(getLuckySkillInfo);
            }
        }

        if (getLuckySkill.Count != rouletteWinningCount || rouletteItemsList.Count == 16)
            return;

        foreach (var skillInfo in getLuckySkill)
        {
            int _skillType = (int)skillInfo.skillType;
            bool isPassive = _skillType < (int)PlayerSkillCategory.TOP_ACTIVE_BOOMERANG;
            bool isNomalSkill = skillInfo.level < 5;
            int boxType = isPassive ? 0 : isNomalSkill ? 1 : 2;

            Sprite _sprite;
            string _itemTitle;
            string _itemDescription;

            int index = isPassive ? 0 : (isNomalSkill ? 0 : 1);

            _sprite = m_SkillUIDataDictionary[_skillType].skillSprite[index];
            _itemTitle = "" + m_SkillUIDataDictionary[_skillType].skillName[index];
            _itemDescription = "" + m_SkillUIDataDictionary[_skillType].skillDescription[skillInfo.level];

            int starCount = skillInfo.level !=5 ? skillInfo.level + 1 : 0;
            RouletteResultUI.Instance.CreatDescriptionLayout(boxType, _sprite, _itemTitle, _itemDescription, starCount);

            GameObject _newSkillBox = Instantiate(itemBoxPrefab[boxType]);
            _newSkillBox.GetComponent<RouletteItem>().InitializedDataForRouletteItem(_sprite);
            _newSkillBox.SetActive(false);

            rouletteItemsList.Add(_newSkillBox); //선택된 아이템 먼저 넣기
        }

        int needCount = itemSpawnLayoutTransform.Length * 4 - rouletteItemsList.Count; 

        for (int i = 0; i < needCount; ++i)
        {
            int randomType = RandomWeightedRouletteItem.GetWeightedLuckySkillRouletteItem(rouletteWinningCount, ComboSkillManager.Instance.currentSkillConditionDictionary);

            bool isPassive = randomType < (int)PlayerSkillCategory.TOP_ACTIVE_BOOMERANG;
            int currentThisSkillLevel = ComboSkillManager.Instance.currentSkillConditionDictionary[randomType].level;
            bool isNomalSkill = currentThisSkillLevel < 5;
            int boxType = isPassive ? 0 : currentThisSkillLevel < 5 ? 1 : 2;

            Sprite _sprite;
            string _itemTitle;
            string _itemDescription;

            int index = isPassive ? 0 : (isNomalSkill ? 0 : 1);

            _sprite = m_SkillUIDataDictionary[randomType].skillSprite[index];
            _itemTitle = "" + m_SkillUIDataDictionary[randomType].skillName[index];
            _itemDescription = "" + m_SkillUIDataDictionary[randomType].skillDescription[0];

            GameObject _newSkillBox = Instantiate(itemBoxPrefab[boxType]);
            _newSkillBox.GetComponent<RouletteItem>().InitializedDataForRouletteItem(_sprite);
            _newSkillBox.SetActive(false);

            rouletteItemsList.Add(_newSkillBox); //미선택된 아이템 넣기
        }
    }

    public override void StartTextEffect(bool start)
    {
        if(start)
            coinTextCoroutine = StartCoroutine(StarEffect());
        else
        {
            StopCoroutine(coinTextCoroutine);

            coinText.gameObject.transform.localScale = Vector3.one;
            coinText.text = "" + rewardCoinValue;
        }       
    }

    IEnumerator StarEffect()
    {
        float startTime = 0;
        float progress = 0;
        float wantedValue = 0;

        float checkingTimer_size = 1;
        bool isBigger = false;

        while (wantedValue < rewardCoinValue)
        {
            progress += Time.unscaledDeltaTime / textEffectDuration;

            wantedValue = Mathf.Lerp(startTime, rewardCoinValue, progress);
            coinText.text = "" + Mathf.RoundToInt(wantedValue);

            //텍스트 사이즈
            if (checkingTimer_size >= 1.7f)
                isBigger = false;
            else if (checkingTimer_size <= 1)
                isBigger = true;

            if (isBigger)
                checkingTimer_size += Time.unscaledDeltaTime * 1.5f;
            else
                checkingTimer_size -= Time.unscaledDeltaTime * 1.5f;

            coinText.gameObject.transform.localScale = new Vector3(checkingTimer_size, checkingTimer_size, 0);

            yield return new WaitForEndOfFrame();
        }

        coinText.gameObject.transform.localScale = Vector3.one;
        coinText.text = "" + rewardCoinValue;
    }

    public override void UpdateReward()
    {
        //코인 업데이트 
        ScoreManager.Instance.UpdateCoinCount(rewardCoinValue);

        for (int i = 0; i < getLuckySkill.Count; ++i)
        {
            if (getLuckySkill[i].doReward)
            {
                if (getLuckySkill[i].level == 5)
                    SkillSelectUIMenuManager.Instance.RegisterAcquiredSkillIcon(getLuckySkill[i].skillType);

                //스킬 업데이트
                ComboSkillData _data;
                _data.Type = PlayerSkillCategory.TOP_PASSIVE_EXO_BRACER + getLuckySkill[i].skillType;
                _data.level = getLuckySkill[i].level;
                ComboSkillManager.AddComboSkill(_data);
            }
        }
    }
}
