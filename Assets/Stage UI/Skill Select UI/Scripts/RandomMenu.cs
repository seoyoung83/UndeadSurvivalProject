using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomMenu 
{
    public static int CheckExportMenueCount(Dictionary<int, SkillInfo> randomMenuInfo)
    {
        int exportSkillMaxCount = 0;

        //내보낼수 있는 스킬(Lock(F)인 것들) 카운트 재확인
        foreach (KeyValuePair<int, SkillInfo> kvp in randomMenuInfo)
        {
            if (kvp.Value.isLock == false)
                exportSkillMaxCount++;
        }

        if (exportSkillMaxCount == 0)  // 보너스 스킬 내보내야함
            return 0;
        else
        {
            if (exportSkillMaxCount < 3)  // 당첨 수 수정
                return exportSkillMaxCount;
            else //예정대로 3개
                return 3;
        }
    }

    public static int GetRandomSkillMenuInfo(Dictionary<int, SkillInfo> randomMenuInfo)
    {
        int totalWeight = 0;

        foreach (KeyValuePair<int, SkillInfo> kvp in randomMenuInfo)
        {
            if (kvp.Value.isLock == false)
                totalWeight += kvp.Value.weight;
        }

        int randomValue = Random.Range(0, totalWeight);

        int cumulativeWeight = 0;

        foreach (KeyValuePair<int, SkillInfo> kvp in randomMenuInfo)
        {
            if (kvp.Value.isLock == false)
            {
                cumulativeWeight += kvp.Value.weight;

                if (randomValue < cumulativeWeight)
                {
                    return (int)kvp.Key;
                }
            }
        }

        return -1;
    }
}
