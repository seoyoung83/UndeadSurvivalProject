using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class WeightedCombatAssistanceCoin 
{
    public int coinType;
    public int weight;
}

public class CombatAssistanceRouletteUI : Roulette 
{
    [Header("About ItemBox Datas")]
    [SerializeField] GameObject itemBoxPrefab;
    [SerializeField] CombatAssistanceItemDataScriptableObject combatAssistanceItemDataScriptableObject;

    [SerializeField] List<WeightedCombatAssistanceCoin> weightedCombatAssistanceCoins;

    Queue<int> selectedItemValueQueue = new Queue<int>(); //���õ� ���ξ����� Value

    public override void CreatRouletteItemBox(int _rouletteWinningCount)
    {
        selectedRouletteBoxIndexList = new List<int>();

        rouletteItemsList = new List<GameObject>();

        rouletteWinningCount = _rouletteWinningCount;

        //�귿 �����۵� �ʱ� ����
        GetCombatAssistanceRouletteItem();
    }

    void GetCombatAssistanceRouletteItem()
    {
        selectedItemValueQueue = new Queue<int>();

        for (int i = 0; i < itemSpawnLayoutTransform.Length * 4; ++i)
        {
            int randomType = (int)RandomWeightedRouletteItem.GetWeightedCombatAssistanceCoin(weightedCombatAssistanceCoins);

            GameObject _newItem = Instantiate(itemBoxPrefab);

            Sprite _sprite = combatAssistanceItemDataScriptableObject.combatAssistanceItemDatasSet[randomType].skillIcon;
            string _itemTitle = combatAssistanceItemDataScriptableObject.combatAssistanceItemDatasSet[randomType].skillName;
            string _itemDescription = combatAssistanceItemDataScriptableObject.combatAssistanceItemDatasSet[randomType].skillDescription;
            int _itemValue = combatAssistanceItemDataScriptableObject.combatAssistanceItemDatasSet[randomType].itemValue;

            if (i < rouletteWinningCount)
            {
                selectedItemValueQueue.Enqueue(_itemValue);
                RouletteResultUI.Instance.CreatDescriptionLayout(0, _sprite, _itemTitle, _itemDescription, 1);
            }
               
            _newItem.GetComponent<RouletteItem>().InitializedDataForRouletteItem(_sprite);
            _newItem.SetActive(false);

            rouletteItemsList.Add(_newItem);
        }

        SetRouletteItemBox();
    }

    public override void UpdateReward()
    {
        //���� ������Ʈ 
        for (int i = 0; i < selectedItemValueQueue.Count; ++i)
        {
            int ItemValue = selectedItemValueQueue.Dequeue();
            ScoreManager.Instance.UpdateCoinCount(ItemValue);
        }
        selectedItemValueQueue.Clear();
    }
}
