using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PassiveSkillMenu : SkillMenu
{
    [Header("Combination Sets")]
    [SerializeField] GameObject CombinationMenuUI;
    [SerializeField] Transform CombinationSkillImageTransform;
    [SerializeField] GameObject skillImagePrefab;


    public override void SkillMenuInitialize(PlayerSkillCategory _skillType, int nextLevel)
    {
        skillType = _skillType;

        level = nextLevel;

        skillIcon.sprite = m_SkillUIDataDictionary[(int)_skillType].skillSprite[0];

        skillName.text = "" + m_SkillUIDataDictionary[(int)_skillType].skillName[0];

        skillDescription.text = "" + m_SkillUIDataDictionary[(int)_skillType].skillDescription[level];

        for (int i = -1; i < level; ++i)
        {
            GameObject _star = Instantiate(starObjectPrefab);
            _star.transform.SetParent(starIconLayoutGroup);

            if (i + 1 == level)
                starIcon = _star;
        }

        Queue<int> requireSkillNumberQueue = new Queue<int>();
        requireSkillNumberQueue = GetReqireSkillTyeNumber((int)_skillType);
        //조합 스킬 아이콘 위치시키기
        foreach (int requireSkill in requireSkillNumberQueue)
        {
            bool weaponCategory = (requireSkill >= (int)PlayerSkillCategory.TOP_WEAPON_BAZOOKA) && (requireSkill < (int)PlayerSkillCategory.MAX_SIZE);
            if (weaponCategory && requireSkill != PlayerWeaponController.Instance.PlayerWeaponType)
                continue;

            CombinationMenuUI.SetActive(true);
            GameObject _skillImageObject = Instantiate(skillImagePrefab);
            _skillImageObject.gameObject.GetComponent<Image>().sprite = m_SkillUIDataDictionary[requireSkill].skillSprite[0]; 
            _skillImageObject.transform.SetParent(CombinationSkillImageTransform);
        }
    }

    Queue<int> GetReqireSkillTyeNumber(int _gainedSkillNumber)
    {
        IReadOnlyList<bool>[] tempLinkSkillCheckDataDictionary = DataManager.Instance.LinkSkillCheckDataDictionary;

        Queue<int> reqiureSkillNumber = new Queue<int>();

        for (int i = 0; i < tempLinkSkillCheckDataDictionary.Length; i++)
        {
            if (tempLinkSkillCheckDataDictionary[_gainedSkillNumber][i])
            {
                reqiureSkillNumber.Enqueue(i);
            }
        }

        return reqiureSkillNumber;
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
