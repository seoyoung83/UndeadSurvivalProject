using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RandomWeightedRouletteItem
{
    //당첨 카운트
    public static int GetRouletteWinningCount(int[] weightedValue)
    {
        int[] WinningCount = { 1, 3, 5 };

        int totalWeight = 0;

        for (int i = 0; i < weightedValue.Length; i++)
            totalWeight += weightedValue[i];

        int randomValue = Random.Range(0, totalWeight);

        int cumulativeWeight = 0;
        for (int i = 0; i < weightedValue.Length; i++)
        {
            cumulativeWeight += weightedValue[i];

            if (randomValue < cumulativeWeight)
                return WinningCount[i];
        }

        return -1;
    }

    //전투지원 룰렛 박스 아이템
    public static int? GetWeightedCombatAssistanceCoin(List<WeightedCombatAssistanceCoin> weightedItems)
    {
        int totalWeight = 0;
        foreach (WeightedCombatAssistanceCoin weightedItem in weightedItems)
        {
            totalWeight += weightedItem.weight;
        }

        int randomValue = Random.Range(0, totalWeight);

        int cumulativeWeight = 0;
        foreach (WeightedCombatAssistanceCoin weightedItem in weightedItems)
        {
            cumulativeWeight += weightedItem.weight;

            if (randomValue < cumulativeWeight)
                return weightedItem.coinType;
        }
        return null;
    }

    //랜덤스킬 럭키 박스
    public static int GetWeightedLuckySkillRouletteItem(int _rouletteWinningCount, Dictionary<int, SkillInfo> weightedSkills)
    {
        int exportMaxCount = 0;

        //내보낼수 있는 스킬 카운트 확인
        foreach (KeyValuePair<int, SkillInfo> kvp in weightedSkills)
        {
            bool isPassiveSkill = kvp.Key < (int)PlayerSkillCategory.TOP_ACTIVE_BOOMERANG;

            //고려사항 : 패시브or액티브 , 조건 충족 or미충족
            int limitLevel = isPassiveSkill ? 4 : (!kvp.Value.isConditionMet ? 4 : 5);

            if (kvp.Value.level > -1 && !kvp.Value.isLock)//기본 조건 : 습득 & Lock(F)
            {
                int remainingCount = limitLevel - kvp.Value.level;
                for (int j = 0; j < remainingCount; ++j)
                    exportMaxCount++;
            }
        }

        if(exportMaxCount < _rouletteWinningCount)
        {
            return -1;
        }
        else
        {
            int totalWeight = 0;
            foreach (KeyValuePair<int, SkillInfo> kvp in weightedSkills)
            {
                if (kvp.Value.level > -1 && !kvp.Value.isLock)
                    totalWeight += kvp.Value.weight;
            }

            int randomValue = Random.Range(0, totalWeight);

            int cumulativeWeight = 0;


            foreach (KeyValuePair<int, SkillInfo> kvp in weightedSkills)
            {
                if (kvp.Value.level > -1 && !kvp.Value.isLock)
                {
                    cumulativeWeight += kvp.Value.weight;

                    if (randomValue < cumulativeWeight)
                    {
                        return (int)kvp.Key;
                    }
                }
            }
        }

        return -1;
    }
}
