using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BonusSkillMenu : SkillMenu
{
    public override void SkillMenuInitialize(PlayerSkillCategory _skillType, int _level)
    {
        skillType = _skillType;

        skillIcon.sprite = m_SkillUIDataDictionary[(int)_skillType].skillSprite[0];

        skillName.text = "" + m_SkillUIDataDictionary[(int)_skillType].skillName[0];

        skillDescription.text = "" + m_SkillUIDataDictionary[(int)_skillType].skillDescription[0];
    }
}
