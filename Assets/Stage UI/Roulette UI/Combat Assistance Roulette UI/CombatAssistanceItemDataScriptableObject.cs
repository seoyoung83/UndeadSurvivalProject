using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = " Combat Assistance Item Datas", menuName = "ScriptableObjects/CombatAssistanceItemDataScriptableObject", order = 6)]
public class CombatAssistanceItemDataScriptableObject : ScriptableObject
{
    [System.Serializable]
    public class CombatAssistanceItemDatas
    {
        public uint skillId;
        public Sprite skillIcon;
        public string skillName;
        public string skillDescription;
        public int itemValue;
    }

    public CombatAssistanceItemDatas[] combatAssistanceItemDatasSet;
}
