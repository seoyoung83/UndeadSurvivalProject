using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ActiveSkillMenu : SkillMenu
{
    public override void SkillMenuInitialize(PlayerSkillCategory _skillType, int _nextLevel)
    {
        skillType = _skillType;

        level = _nextLevel;

        int Index = level != 5 ? 0 : 1;

        skillIcon.sprite = m_SkillUIDataDictionary[(int)_skillType].skillSprite[Index];

        skillName.text = "" + m_SkillUIDataDictionary[(int)_skillType].skillName[Index];

        skillDescription.text = "" + m_SkillUIDataDictionary[(int)_skillType].skillDescription[level];


        if (level >= 5)
            return;

        for (int i = -1; i < level; ++i)
        {
            GameObject _star = Instantiate(starObjectPrefab);
            _star.transform.SetParent(starIconLayoutGroup);

            if (i + 1 == level)
                starIcon = _star;
        }
    }

    private void OnEnable()
    {
        StartCoroutine(SkillMenuStarIconUIEffect());
    }

    IEnumerator SkillMenuStarIconUIEffect()
    {
        float checkingTimer = 0f;

        bool isPlusAlpha = true;

        while (gameObject.activeInHierarchy && starIcon != null)
        {
            if (checkingTimer >= 0.9f)
            {
                isPlusAlpha = false;
            }
            else if (checkingTimer <= 0.1f)
            {
                isPlusAlpha = true;
            }

            if (isPlusAlpha)
            {
                checkingTimer += Time.unscaledDeltaTime * 0.5f;
            }
            else
            {
                checkingTimer -= Time.unscaledDeltaTime * 0.5f;
            }

            starIcon.GetComponent<Image>().color = new Color(1, 1, 0, checkingTimer);

            yield return null;
        }
    }
}
