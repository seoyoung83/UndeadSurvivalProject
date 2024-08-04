using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

abstract public class SkillMenu : MonoBehaviour
{
    protected PlayerStat m_playerStat;

    protected PlayerSkillCategory skillType;

    protected IReadOnlyDictionary<int, JSkillUIData> m_SkillUIDataDictionary;

    protected int level;

    [SerializeField] Button skillSelectButton;

    [SerializeField] protected Image skillIcon;
    [SerializeField] protected TextMeshProUGUI skillName;
    [SerializeField] protected TextMeshProUGUI skillDescription;


    [SerializeField] protected GameObject starObjectPrefab;
    [SerializeField] protected Transform starIconLayoutGroup;
    protected GameObject starIcon;


    private void Awake()
    {
        m_SkillUIDataDictionary = DataManager.Instance.SkillUIDataDictionary;

        m_playerStat = GameObject.FindObjectOfType<PlayerStat>();

        skillSelectButton.onClick.AddListener(OnSelected);
    }

    public abstract void SkillMenuInitialize(PlayerSkillCategory _skillType, int _level);
    
    protected void OnSelected()
    {
        AudioManager.Instance.OnClickButtonAudioEvent();

        switch (skillType)
        {
            case PlayerSkillCategory.TOP_BONUS_GOLD:
                m_playerStat.DoImmediateHeal(PlayerStat.BuffMaxHp * 0.30f);
                break;
            case PlayerSkillCategory.BONUS_MEAT:
                ScoreManager.Instance.UpdateCoinCount(50);
                break;
            default:

                ComboSkillData _data;
                _data.Type = skillType;
                _data.level = level;

                ComboSkillManager.AddComboSkill(_data);
                SkillSelectUIMenuManager.Instance.RegisterAcquiredSkillIcon((int)skillType);

                break;
        }

        StageUIManager.Instance.CloseSkillSelectUI();
    }


}
