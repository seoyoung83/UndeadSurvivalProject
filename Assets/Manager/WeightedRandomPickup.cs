using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WeightedRandomPickup 
{
    public static PickupType? GetWeightedRandomBoxPickup(List<WeightedBoxPickup> weightedItems) 
    {
        int totalWeight = 0;
        foreach (WeightedBoxPickup weightedItem in weightedItems)
        {
            totalWeight += weightedItem.weight;
        }

        int randomValue = Random.Range(0, totalWeight);

        int cumulativeWeight = 0;
        foreach (WeightedBoxPickup weightedItem in weightedItems)
        {
            cumulativeWeight += weightedItem.weight;
            if (randomValue < cumulativeWeight)
            {
                return weightedItem.pickupType;
            }
        }
        return null;
    }

    public static PickupType? GetWeightedRandomSkillFuelPickup(List<WeightedSkillFuelPickup> weightedItems)
    {
        int totalWeight = 0;
        foreach (WeightedSkillFuelPickup weightedItem in weightedItems)
        {
            totalWeight += weightedItem.weight[StageManager.m_currentRound - 1];
        }

        int randomValue = Random.Range(0, totalWeight);

        int cumulativeWeight = 0;
        foreach (WeightedSkillFuelPickup weightedItem in weightedItems)
        {
            cumulativeWeight += weightedItem.weight[StageManager.m_currentRound - 1];
            if (randomValue < cumulativeWeight)
            {
                return weightedItem.pickupType;
            }
        }
        return null;
    }
}
