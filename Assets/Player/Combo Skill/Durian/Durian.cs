using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Durian : ActiveComboSkill
{
    PlayerSkillCategory thisSkillType = PlayerSkillCategory.ACTIVE_DURIAN;

    GameObject m_currentDurianBullet = null;

    Vector3[] durianSpawnDirectVectList = { new Vector3(0.5f, 0.5f, 0), new Vector3(-0.5f, 0.5f, 0),
        new Vector3(0.5f, -0.5f, 0), new Vector3(-0.5f, -0.5f, 0) };

    bool isAttacking = false;

    Queue<Vector2> currentDurianVelocity = new Queue<Vector2>();
    float checkingTimer_velocity = 0;

    private void Awake()
    {
        curruntActiveSkillTypeNumber = (int)thisSkillType;
    }

    public override void LevelUp(int _currentWeaponSkillLevel)
    {
        isSettingUpData = false;
        
        isAttacking = false;

        if (m_currentDurianBullet != null)
            m_currentDurianBullet.SetActive(false);

        skillLevel = _currentWeaponSkillLevel;

        SetData((int)thisSkillType, skillLevel);

        if (skillLevel == 5)
            m_currentDurianBullet = null;

        isSettingUpData = true;
    }

    public override void SetToReady(bool _isTimeToReady)
    {
        isSettingUpData = !_isTimeToReady;

        if (_isTimeToReady)
        {
            isAttacking = false;
            m_currentDurianBullet.SetActive(false);

            if (skillLevel == 5)
                PlayerBulletPooler.Instance.ExpiredPlayerBullet(PlayerBulletType.DURIAN_SPECIAL_BULLET);
        }

    }

    private void Update()
    {
        if (isSettingUpData)
        {
            if (m_currentDurianBullet != null)
                UpdatInfo();

            if (!isAttacking)
                Skill_Release(durianSpawnDirectVectList[Random.Range(0, 4)]);
        }
    }

    void Skill_Release(Vector3 _direct)
    {
        isAttacking = true;

        float _attackRang = attackRang + (attackRang * buffValue_AttackRang / 100);

        float _attackSpeed = attackSpeed + (attackSpeed * buffValue_AttackSpeed / 100);

        if (skillLevel != 5)
        {
            if (m_currentDurianBullet == null)
                m_currentDurianBullet = PlayerBulletPooler.Instance.GetPlayerBullet(PlayerBulletType.DURIAN_NORMAL);

            if (m_currentDurianBullet)
            {
                m_currentDurianBullet.GetComponent<OneDurian>().skillAudio = skillAudio;
                m_currentDurianBullet.GetComponent<OneDurian>().UpdateSkillLevel(skillLevel);
                m_currentDurianBullet.GetComponent<OneDurian>().UpdateSkillInfo(_attackSpeed, _attackRang);
                m_currentDurianBullet.GetComponent<OneDurian>().SetMuzzleTransform(playerTransform.transform);
                m_currentDurianBullet.gameObject.SetActive(true);
                m_currentDurianBullet.GetComponent<OneDurian>().Fire(_direct);
            }
        }
        else
        {
            if (m_currentDurianBullet == null)
                m_currentDurianBullet = PlayerBulletPooler.Instance.GetPlayerBullet(PlayerBulletType.DURIAN_SPECIAL);

            if (m_currentDurianBullet)
            {
                m_currentDurianBullet.GetComponent<OneCaltrops>().skillAudio = skillAudio;
                m_currentDurianBullet.GetComponent<OneCaltrops>().UpdateSkillInfo(_attackSpeed, _attackRang);
                m_currentDurianBullet.GetComponent<OneCaltrops>().SetMuzzleTransform(playerTransform.transform);
                m_currentDurianBullet.gameObject.SetActive(true);
                m_currentDurianBullet.GetComponent<OneCaltrops>().Fire(_direct);

            }
        }
    }

    //패시브 스킬쓸때 쓸거!
    void UpdatInfo()
    {
        float _attackRang = attackRang + (attackRang * buffValue_AttackRang / 100);

        float _attackSpeed = attackSpeed + (attackSpeed * buffValue_AttackSpeed / 100);

        if (skillLevel != 5)
            m_currentDurianBullet.GetComponent<OneDurian>().UpdateSkillInfo(_attackSpeed, _attackRang);
        else if (skillLevel == 5)
        {
            m_currentDurianBullet.GetComponent<OneCaltrops>().UpdateSkillInfo(_attackSpeed, _attackRang);
            m_currentDurianBullet.GetComponent<OneCaltrops>().UpdateBuffInfo(buffValue_AttackSpeed, buffValue_AttackRang, buffValue_SkillDuration);
        }
            
    }

    void CheckingVelocity(Rigidbody2D rigid)
    {
        checkingTimer_velocity += Time.deltaTime;

        if (checkingTimer_velocity > 1f)
        {
            checkingTimer_velocity = 0;

            currentDurianVelocity.Enqueue(rigid.velocity);
        }

        if (currentDurianVelocity.Count < 5)
            return;

        Queue<Vector2> checkingVelocity = new Queue<Vector2>();

    }
}