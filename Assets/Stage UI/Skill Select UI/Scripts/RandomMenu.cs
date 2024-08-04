using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomMenu 
{
    public static int CheckExportMenueCount(Dictionary<int, SkillInfo> randomMenuInfo)
    {
        int exportSkillMaxCount = 0;

        //�������� �ִ� ��ų(Lock(F)�� �͵�) ī��Ʈ ��Ȯ��
        foreach (KeyValuePair<int, SkillInfo> kvp in randomMenuInfo)
        {
            if (kvp.Value.isLock == false)
                exportSkillMaxCount++;
        }

        if (exportSkillMaxCount == 0)  // ���ʽ� ��ų ����������
            return 0;
        else
        {
            if (exportSkillMaxCount < 3)  // ��÷ �� ����
                return exportSkillMaxCount;
            else //������� 3��
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
