using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillIcon : MonoBehaviour
{
    [SerializeField] Image skillIcon;

    int skillType;

    public void InitializedSkillIconImage(int _skillType)
    {
        skillType = _skillType;

        skillIcon.sprite = DataManager.Instance.SkillUIDataDictionary[_skillType].skillSprite[0];
    }

    public void UpdateSkillIconImage()
    {
        bool isPassiveSkill = skillType < (int)PlayerSkillCategory.TOP_ACTIVE_BOOMERANG;

        if (!isPassiveSkill)
            skillIcon.sprite = DataManager.Instance.SkillUIDataDictionary[skillType].skillSprite[1];
    }
}
