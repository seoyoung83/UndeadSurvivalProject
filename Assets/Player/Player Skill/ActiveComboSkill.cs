using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public abstract class ActiveComboSkill : MonoBehaviour
{
    protected int curruntActiveSkillTypeNumber;

    [SerializeField] protected int skillLevel;

    protected AudioSource skillAudio;

    protected Transform playerTransform;
    protected CameraMove m_cameraMove;

    protected bool isSettingUpData = false;

    protected int maxBulletCount;

    protected float attackInterval; //���� �ð� ����
    protected float skillDuration; //���� ���ӽð�
    protected float attackSpeed; //���� �� ź�� ���ǵ�
    protected float attackRang; //���� �� ź�� ���� 

    protected static float buffValue_SkillDuration = 0;
    protected static float buffValue_AttackInterval = 0;
    protected static float buffValue_AttackSpeed = 0;
    protected static float buffValue_AttackRang = 0;

    private void Start()
    {
        playerTransform = GameObject.FindObjectOfType<PlayerMove>().transform;

        m_cameraMove = GameObject.FindObjectOfType<CameraMove>();

        skillAudio = GetComponent<AudioSource>();

        SkillDamagePooler.Instance.PlayerSkillDamagePoolInitialized(curruntActiveSkillTypeNumber);
    }

    public abstract void LevelUp(int _level); //ComboSkillManager���� ȣ���ϱ�.

    protected void SetData(int _skillTypeNumber, int _skillLevel) //���� ����
    {
        // Skill Bullet Setting
        SkillBulletSetting(_skillTypeNumber, _skillLevel);

        // Skill Ability Value ����
        IReadOnlyDictionary<string, JSkillAbilityData> skillAbilityAssets = DataManager.Instance.SkillAbilityDataDictionary;

        string index = _skillTypeNumber + "";

        maxBulletCount = skillAbilityAssets[index].attackCountOfOneTimeOfLevel[_skillLevel];

        attackInterval = skillAbilityAssets[index].attackIntervalOfLevel[_skillLevel];

        skillDuration = skillAbilityAssets[index].skillDurationOfLevel[_skillLevel];

        attackSpeed = skillAbilityAssets[index].speedOfLevel[_skillLevel];

        attackRang = skillAbilityAssets[index].attackRangOfLevel[_skillLevel];
    }

    // BulletInitialized & Destroy
    void SkillBulletSetting(int _skillTypeNumber, int _skillLevel)
    {
        if (!(_skillLevel == 0 || _skillLevel == 5))
            return;

        switch (_skillTypeNumber)
        {
            case (int)PlayerSkillCategory.TOP_ACTIVE_BOOMERANG:
                if (_skillLevel == 0)
                    PlayerBulletPooler.Instance.PlayerBulletInitialized(PlayerBulletType.BOOMERANG_NORMAL);
                else if (_skillLevel == 5)
                {
                    PlayerBulletPooler.Instance.PlayerBulletInitialized(PlayerBulletType.BOOMERANG_SPECIAL);
                    PlayerBulletPooler.Instance.DestroyPlayerBullet(PlayerBulletType.BOOMERANG_NORMAL);
                }
                break;
            case (int)PlayerSkillCategory.ACTIVE_BRICK:
                if (_skillLevel == 0)
                    PlayerBulletPooler.Instance.PlayerBulletInitialized(PlayerBulletType.BRICK_NORMAL);
                else if (_skillLevel == 5)
                {
                    PlayerBulletPooler.Instance.PlayerBulletInitialized(PlayerBulletType.BRICK_SPECIAL);
                    PlayerBulletPooler.Instance.DestroyPlayerBullet(PlayerBulletType.BRICK_NORMAL);
                }
                break;
            case (int)PlayerSkillCategory.ACTIVE_DRILLSHOT:
                if (_skillLevel == 0)
                    PlayerBulletPooler.Instance.PlayerBulletInitialized(PlayerBulletType.DRILLSHOT_NORMAL);
                else if (_skillLevel == 5)
                {
                    PlayerBulletPooler.Instance.PlayerBulletInitialized(PlayerBulletType.DRILLSHOT_SPECIAL);
                    PlayerBulletPooler.Instance.DestroyPlayerBullet(PlayerBulletType.DRILLSHOT_NORMAL);
                }
                break;
            case (int)PlayerSkillCategory.ACTIVE_DRONE_A:
                if (_skillLevel == 0)
                    PlayerBulletPooler.Instance.PlayerBulletInitialized(PlayerBulletType.DRONEA_ROCKET_NORMAL);
                break;
            case (int)PlayerSkillCategory.ACTIVE_DRONE_B:
                if (_skillLevel == 0)
                    PlayerBulletPooler.Instance.PlayerBulletInitialized(PlayerBulletType.DRONEB_ROCKET_NORMAL);
                break;
            case (int)PlayerSkillCategory.ACTIVE_DURIAN:
                if (_skillLevel == 0)
                    PlayerBulletPooler.Instance.PlayerBulletInitialized(PlayerBulletType.DURIAN_NORMAL);
                else if (_skillLevel == 5)
                {
                    PlayerBulletPooler.Instance.PlayerBulletInitialized(PlayerBulletType.DURIAN_SPECIAL);
                    PlayerBulletPooler.Instance.PlayerBulletInitialized(PlayerBulletType.DURIAN_SPECIAL_BULLET);
                    PlayerBulletPooler.Instance.DestroyPlayerBullet(PlayerBulletType.DURIAN_NORMAL);
                }
                break;
            case (int)PlayerSkillCategory.ACTIVE_GARDIAN:
                if (_skillLevel == 0)
                    PlayerBulletPooler.Instance.PlayerBulletInitialized(PlayerBulletType.GARDIAN_NORMAL);
                else if (_skillLevel == 5)
                {
                    PlayerBulletPooler.Instance.PlayerBulletInitialized(PlayerBulletType.GARDIAN_SPECIAL);
                    PlayerBulletPooler.Instance.DestroyPlayerBullet(PlayerBulletType.GARDIAN_NORMAL);
                }
                break;
            case (int)PlayerSkillCategory.ACTIVE_LIGHTNINGEMITTER:
                if (_skillLevel == 0)
                    PlayerBulletPooler.Instance.PlayerBulletInitialized(PlayerBulletType.LIGHTNINGEMITTER_NORMAL);
                else if (_skillLevel == 5)
                {
                    PlayerBulletPooler.Instance.PlayerBulletInitialized(PlayerBulletType.LIGHTNINGEMITTER_SPECIAL);
                    PlayerBulletPooler.Instance.DestroyPlayerBullet(PlayerBulletType.LIGHTNINGEMITTER_NORMAL);
                }
                break;
            case (int)PlayerSkillCategory.ACTIVE_MELTHINGSHIELD:
                if (_skillLevel == 0)
                    PlayerBulletPooler.Instance.PlayerBulletInitialized(PlayerBulletType.MELTHINGSHIELD_NORMAL);
                else if (_skillLevel == 5)
                {
                    PlayerBulletPooler.Instance.PlayerBulletInitialized(PlayerBulletType.MELTHINGSHIELD_SPECIAL);
                    PlayerBulletPooler.Instance.DestroyPlayerBullet(PlayerBulletType.MELTHINGSHIELD_NORMAL);
                }
                break;
            case (int)PlayerSkillCategory.ACTIVE_MOLOTOVCOCKTAIL:
                if (_skillLevel == 0)
                    PlayerBulletPooler.Instance.PlayerBulletInitialized(PlayerBulletType.MOLOTOVCOCKTAIL_NORMAL);
                else if (_skillLevel == 5)
                {
                    PlayerBulletPooler.Instance.PlayerBulletInitialized(PlayerBulletType.MOLOTOVCOCKTAIL_SPECIAL);
                    PlayerBulletPooler.Instance.DestroyPlayerBullet(PlayerBulletType.MOLOTOVCOCKTAIL_NORMAL);
                }
                break;
            case (int)PlayerSkillCategory.ACTIVE_ROCKETLAUNCHER:
                if (_skillLevel == 0)
                    PlayerBulletPooler.Instance.PlayerBulletInitialized(PlayerBulletType.ROCKETLAUNCHER_NORMAL);
                else if (_skillLevel == 5)
                {
                    PlayerBulletPooler.Instance.PlayerBulletInitialized(PlayerBulletType.ROCKETLAUNCHER_SPECIAL);
                    PlayerBulletPooler.Instance.DestroyPlayerBullet(PlayerBulletType.ROCKETLAUNCHER_NORMAL);
                }
                break;
            case (int)PlayerSkillCategory.ACTIVE_SOCCERBALL:
                if (_skillLevel == 0)
                    PlayerBulletPooler.Instance.PlayerBulletInitialized(PlayerBulletType.SOCCERBALL_NORMAL);
                else if (_skillLevel == 5)
                {
                    PlayerBulletPooler.Instance.PlayerBulletInitialized(PlayerBulletType.SOCCERBALL_SPECIAL);
                    PlayerBulletPooler.Instance.PlayerBulletInitialized(PlayerBulletType.SOCCERBALL_SPECIAL_BULLET);
                    PlayerBulletPooler.Instance.DestroyPlayerBullet(PlayerBulletType.SOCCERBALL_NORMAL);
                }
                break;
        }

    }

    public abstract void SetToReady(bool isTimeToReady);//�θ޶�, �帱��, �θ���, �����߻��, ����, �౸, 

    public void DoPassiveBuff(PlayerSkillCategory _passivType, float _value) 
    {
        switch (_passivType)
        {
            case PlayerSkillCategory.TOP_PASSIVE_EXO_BRACER:  //ȿ�� ���ӽð� ����
                buffValue_SkillDuration = _value;
                break;
            case PlayerSkillCategory.PASSIVE_ENERGYCUBE: //��� ���� �ð� ���� ���� 
                buffValue_AttackInterval = _value;
                break;
            case PlayerSkillCategory.PASSIVE_AMOTHURSTER: //ź��� ����ӵ� ����
                buffValue_AttackSpeed = _value;
                break;
            case PlayerSkillCategory.PASSIVE_HE_FUEL:  //��� ź�� �� ������� ����
                buffValue_AttackRang = _value;
                break;
        }
    }
}
