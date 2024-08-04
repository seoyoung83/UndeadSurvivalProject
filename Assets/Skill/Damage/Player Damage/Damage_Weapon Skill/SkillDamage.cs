using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDamage : MonoBehaviour
{
    protected IReadOnlyDictionary<string, JSkillAbilityData> m_SkillAbilityDataDictionary;

    public int skillCategoryTypeNumber { get; set; }
    public float damageDuration { get; set; }

    protected int level;

    private void Start()
    {
        m_SkillAbilityDataDictionary = DataManager.Instance.SkillAbilityDataDictionary;
    }

    private void OnEnable()
    {
        Invoke("StopDamage", damageDuration);
    }

    void StopDamage()
    {
        gameObject.SetActive(false);
    }
}
