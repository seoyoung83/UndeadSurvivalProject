using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillType
{
    //Player_Weapon
    SKILL_BAZOOKABULLET, 
    SKILL_BAZOOKABLACKHOLETRAP, 
    SKILL_REVOLVER,
    SKILL_KUNAI,
    SKILL_SWORD_BLADE, 
    SKILL_SWORD_BLADEWIND, 
    //Player_Skill
    SKILL_MELTINGSHIELD, 
    SKILL_SOCCERBALL,
    SKILL_DURIAN,
    SKILL_GUARDIAN,
    SKILL_ROCKET_EXPLOSION,
    SKILL_BRICK,
    SKILL_DRILLSHOT,
    SKILL_LIGHTNINGENITTER,
    SKILL_MOLOTOVCOCKTAIL,
    SKILL_BOOMERANG,
    SKILL_TYPE_A_DRONE,
    SKILL_TYPE_B_DRONE,
    SKILL_MAX_SIZ,
}


public class SkillDamagePooler : MonoBehaviour
{
    public static SkillDamagePooler Instance;

    [Header("[ Player Skill Damage Object_Weapon ]")]
    [Space(5f)]
    [SerializeField] GameObject skillBazookaBullet;
    [SerializeField] int countBazookaBullet;
    [SerializeField] GameObject skillBazookaBlackholeTrap;
    [SerializeField] int countBazookaBlackholeTrap;
    [Space(10f)]
    [SerializeField] GameObject skillRevolver;
    [SerializeField] int countRevolverBullet;
    [Space(10f)]
    [SerializeField] GameObject skillKunai;
    [SerializeField] int countKunai;
    [Space(10f)]
    [SerializeField] GameObject skillSwordBlade;
    [SerializeField] int countSwordBlade;
    [SerializeField] GameObject skillSwordBladeWind;
    [SerializeField] int countSwordBladeWind;

    [Header("[ Player Skill Damage Object_Combo Skill]")]
    [Space(5f)]
    [SerializeField] GameObject skillMeltingShield;
    [SerializeField] int countMeltingShield;
    [Space(10f)]
    [SerializeField] GameObject skillSoccerBall;
    [SerializeField] int countSoccerBall;
    [Space(10f)]
    [SerializeField] GameObject skillDurian;
    [SerializeField] int countDurian;
    [Space(10f)]
    [SerializeField] GameObject skillGuardian;
    [SerializeField] int countGuardian;
    [Space(10f)]
    [SerializeField] GameObject skillRocketExplostion;
    [SerializeField] int countRocketExplostion;
    [Space(10f)]
    [SerializeField] GameObject skillBrick;
    [SerializeField] int countBrick;
    [Space(10f)]
    [SerializeField] GameObject skillDrillshot;
    [SerializeField] int countDrillshot;
    [Space(10f)]
    [SerializeField] GameObject skillLightning;
    [SerializeField] int countLightning;
    [Space(10f)]
    [SerializeField] GameObject skillMolotovCocktail;
    [SerializeField] int countMolotovCocktail;
    [Space(10f)]
    [SerializeField] GameObject skillBoomerang;
    [SerializeField] int countBoomerang;
    [Space(10f)]
    [SerializeField] GameObject skillDroneTypeA;
    [SerializeField] int countDroneTypeA;
    [Space(10f)]
    [SerializeField] GameObject skillDroneTypeB;
    [SerializeField] int countDroneTypeB;

    List<GameObject>[] skillPool = new List<GameObject>[(int)SkillType.SKILL_MAX_SIZ];

    private void Awake()
    {
        Instance = this;
    }

    public void PlayerSkillDamagePoolInitialized(int _skillType)
    {
        switch (_skillType)
        {
            case (int)PlayerSkillCategory.TOP_ACTIVE_BOOMERANG:
                skillPool[(int)SkillType.SKILL_BOOMERANG] = new List<GameObject>();

                for (int i = 0; i < countBoomerang; ++i)
                {
                    GameObject _skill = (GameObject)Instantiate(skillBoomerang);
                    _skill.GetComponent<SkillDamage>().skillCategoryTypeNumber = _skillType;
                    _skill.GetComponent<SkillDamage>().damageDuration = 0.1f;
                    _skill.SetActive(false);
                    _skill.transform.SetParent(transform);
                    skillPool[(int)SkillType.SKILL_BOOMERANG].Add(_skill);
                }
                break;
            case (int)PlayerSkillCategory.ACTIVE_BRICK:
                skillPool[(int)SkillType.SKILL_BRICK] = new List<GameObject>();

                for (int i = 0; i < countBrick; ++i)
                {
                    GameObject _skill = (GameObject)Instantiate(skillBrick);
                    _skill.GetComponent<SkillDamage>().skillCategoryTypeNumber = _skillType;
                    _skill.GetComponent<SkillDamage>().damageDuration = 0.1f;
                    _skill.SetActive(false);
                    _skill.transform.SetParent(transform);
                    skillPool[(int)SkillType.SKILL_BRICK].Add(_skill);
                }
                break;

            case (int)PlayerSkillCategory.ACTIVE_DRILLSHOT:
                skillPool[(int)SkillType.SKILL_DRILLSHOT] = new List<GameObject>();

                for (int i = 0; i < countDrillshot; ++i)
                {
                    GameObject _skill = (GameObject)Instantiate(skillDrillshot);
                    _skill.GetComponent<SkillDamage>().skillCategoryTypeNumber = _skillType;
                    _skill.GetComponent<SkillDamage>().damageDuration = 0.1f;
                    _skill.SetActive(false);
                    _skill.transform.SetParent(transform);
                    skillPool[(int)SkillType.SKILL_DRILLSHOT].Add(_skill);
                }
                break;
            case (int)PlayerSkillCategory.ACTIVE_DRONE_A:
                skillPool[(int)SkillType.SKILL_TYPE_A_DRONE] = new List<GameObject>();

                for (int i = 0; i < countDroneTypeA; ++i)
                {
                    GameObject _skill = (GameObject)Instantiate(skillDroneTypeA);
                    _skill.GetComponent<SkillDamage>().skillCategoryTypeNumber = _skillType;
                    _skill.GetComponent<SkillDamage>().damageDuration = 0.1f;
                    _skill.SetActive(false);
                    _skill.transform.SetParent(transform);
                    skillPool[(int)SkillType.SKILL_TYPE_A_DRONE].Add(_skill);
                }
                break;
            case (int)PlayerSkillCategory.ACTIVE_DRONE_B:
                skillPool[(int)SkillType.SKILL_TYPE_B_DRONE] = new List<GameObject>();

                for (int i = 0; i < countDroneTypeB; ++i)
                {
                    GameObject _skill = (GameObject)Instantiate(skillDroneTypeB);
                    _skill.GetComponent<SkillDamage>().skillCategoryTypeNumber = _skillType;
                    _skill.GetComponent<SkillDamage>().damageDuration = 0.1f;
                    _skill.SetActive(false);
                    _skill.transform.SetParent(transform);
                    skillPool[(int)SkillType.SKILL_TYPE_B_DRONE].Add(_skill);
                }
                break;
            case (int)PlayerSkillCategory.ACTIVE_DURIAN:
                skillPool[(int)SkillType.SKILL_DURIAN] = new List<GameObject>();

                for (int i = 0; i < countDurian; ++i)
                {
                    GameObject _skill = (GameObject)Instantiate(skillDurian);
                    _skill.GetComponent<SkillDamage>().skillCategoryTypeNumber = _skillType;
                    _skill.GetComponent<SkillDamage>().damageDuration = 0.05f;
                    _skill.SetActive(false);
                    _skill.transform.SetParent(transform);
                    skillPool[(int)SkillType.SKILL_DURIAN].Add(_skill);
                }
                break;
            case (int)PlayerSkillCategory.ACTIVE_GARDIAN:
                skillPool[(int)SkillType.SKILL_GUARDIAN] = new List<GameObject>();

                for (int i = 0; i < countGuardian; ++i)
                {
                    GameObject _skill = (GameObject)Instantiate(skillGuardian);
                    _skill.GetComponent<SkillDamage>().skillCategoryTypeNumber = (int)PlayerSkillCategory.ACTIVE_GARDIAN;
                    _skill.GetComponent<SkillDamage>().damageDuration = 0.05f;
                    _skill.SetActive(false);
                    _skill.transform.SetParent(transform);
                    skillPool[(int)SkillType.SKILL_GUARDIAN].Add(_skill);
                }
                break;
            case (int)PlayerSkillCategory.ACTIVE_LIGHTNINGEMITTER:
                skillPool[(int)SkillType.SKILL_LIGHTNINGENITTER] = new List<GameObject>();

                for (int i = 0; i < countLightning; ++i)
                {
                    GameObject _skill = (GameObject)Instantiate(skillLightning);
                    _skill.GetComponent<SkillDamage>().skillCategoryTypeNumber = (int)PlayerSkillCategory.ACTIVE_LIGHTNINGEMITTER;
                    _skill.GetComponent<SkillDamage>().damageDuration = 0.1f;
                    _skill.SetActive(false);
                    _skill.transform.SetParent(transform);
                    skillPool[(int)SkillType.SKILL_LIGHTNINGENITTER].Add(_skill);
                }
                break;
            case (int)PlayerSkillCategory.ACTIVE_MELTHINGSHIELD:
                skillPool[(int)SkillType.SKILL_MELTINGSHIELD] = new List<GameObject>();

                for (int i = 0; i < countMeltingShield; ++i)
                {
                    GameObject _skill = (GameObject)Instantiate(skillMeltingShield);
                    _skill.GetComponent<SkillDamage>().skillCategoryTypeNumber = (int)PlayerSkillCategory.ACTIVE_MELTHINGSHIELD;
                    _skill.GetComponent<SkillDamage>().damageDuration = 0.1f;
                    _skill.SetActive(false);
                    _skill.transform.SetParent(transform);
                    skillPool[(int)SkillType.SKILL_MELTINGSHIELD].Add(_skill);
                }
                break;
            case (int)PlayerSkillCategory.ACTIVE_MOLOTOVCOCKTAIL:
                skillPool[(int)SkillType.SKILL_MOLOTOVCOCKTAIL] = new List<GameObject>();

                for (int i = 0; i < countMolotovCocktail; ++i)
                {
                    GameObject _skill = (GameObject)Instantiate(skillMolotovCocktail);
                    _skill.GetComponent<SkillDamage>().skillCategoryTypeNumber = (int)PlayerSkillCategory.ACTIVE_MOLOTOVCOCKTAIL;
                    _skill.GetComponent<SkillDamage>().damageDuration = 0.1f;
                    _skill.SetActive(false);
                    _skill.transform.SetParent(transform);
                    skillPool[(int)SkillType.SKILL_MOLOTOVCOCKTAIL].Add(_skill);
                }
                break;
            case (int)PlayerSkillCategory.ACTIVE_ROCKETLAUNCHER:
                skillPool[(int)SkillType.SKILL_ROCKET_EXPLOSION] = new List<GameObject>();

                for (int i = 0; i < countRocketExplostion; ++i)
                {
                    GameObject _skill = (GameObject)Instantiate(skillRocketExplostion);
                    _skill.GetComponent<SkillDamage>().skillCategoryTypeNumber = (int)PlayerSkillCategory.ACTIVE_ROCKETLAUNCHER;
                    _skill.GetComponent<SkillDamage>().damageDuration = 0.15f;
                    _skill.SetActive(false);
                    _skill.transform.SetParent(transform);
                    skillPool[(int)SkillType.SKILL_ROCKET_EXPLOSION].Add(_skill);
                }
                break;
            case (int)PlayerSkillCategory.ACTIVE_SOCCERBALL:
                skillPool[(int)SkillType.SKILL_SOCCERBALL] = new List<GameObject>();

                for (int i = 0; i < countSoccerBall; ++i)
                {
                    GameObject _skill = (GameObject)Instantiate(skillSoccerBall);
                    _skill.GetComponent<SkillDamage>().skillCategoryTypeNumber = (int)PlayerSkillCategory.ACTIVE_SOCCERBALL;
                    _skill.GetComponent<SkillDamage>().damageDuration = 0.05f;
                    _skill.SetActive(false);
                    _skill.transform.SetParent(transform);
                    skillPool[(int)SkillType.SKILL_SOCCERBALL].Add(_skill);
                }
                break;
            default: 
                break;
        }
    }

    //Player_Weapon
    public void PlayerWeaponDamagePoolInitialized(int _weaponType)
    {
        switch (_weaponType)
        {
            case (int)PlayerSkillCategory.TOP_WEAPON_BAZOOKA:
                skillPool[(int)SkillType.SKILL_BAZOOKABULLET] = new List<GameObject>();

                for (int i = 0; i < countBazookaBullet; ++i)
                {
                    GameObject _skill = (GameObject)Instantiate(skillBazookaBullet);
                    _skill.GetComponent<SkillDamage>().skillCategoryTypeNumber = (int)_weaponType;
                    _skill.GetComponent<SkillDamage>().damageDuration = 0.1f;
                    _skill.SetActive(false);
                    _skill.transform.SetParent(transform);
                    skillPool[(int)SkillType.SKILL_BAZOOKABULLET].Add(_skill);
                }
                skillPool[(int)SkillType.SKILL_BAZOOKABLACKHOLETRAP] = new List<GameObject>();

                for (int i = 0; i < countBazookaBlackholeTrap; ++i)
                {
                    GameObject _skill = (GameObject)Instantiate(skillBazookaBlackholeTrap);
                    _skill.GetComponent<SkillDamage>().skillCategoryTypeNumber = (int)_weaponType;
                    _skill.GetComponent<SkillDamage>().damageDuration = 3f;
                    _skill.SetActive(false);
                    _skill.transform.SetParent(transform);
                    skillPool[(int)SkillType.SKILL_BAZOOKABLACKHOLETRAP].Add(_skill);
                }
                break;
            case (int)PlayerSkillCategory.WEAPON_REVOLVER:
                skillPool[(int)SkillType.SKILL_REVOLVER] = new List<GameObject>();

                for (int i = 0; i < countRevolverBullet; ++i)
                {
                    GameObject _skill = (GameObject)Instantiate(skillRevolver);
                    _skill.GetComponent<SkillDamage>().skillCategoryTypeNumber = (int)_weaponType;
                    _skill.GetComponent<SkillDamage>().damageDuration = 0.1f;
                    _skill.SetActive(false);
                    _skill.transform.SetParent(transform);
                    skillPool[(int)SkillType.SKILL_REVOLVER].Add(_skill);
                }
                break;
            case (int)PlayerSkillCategory.WEAPON_KUNAI:
                skillPool[(int)SkillType.SKILL_KUNAI] = new List<GameObject>();

                for (int i = 0; i < countKunai; ++i)
                {
                    GameObject _skill = (GameObject)Instantiate(skillKunai);
                    _skill.GetComponent<SkillDamage>().skillCategoryTypeNumber = (int)_weaponType;
                    _skill.GetComponent<SkillDamage>().damageDuration = 0.1f;
                    _skill.SetActive(false);
                    _skill.transform.SetParent(transform);
                    skillPool[(int)SkillType.SKILL_KUNAI].Add(_skill);
                }
                break;
            case (int)PlayerSkillCategory.WEAPON_SWORD:
                skillPool[(int)SkillType.SKILL_SWORD_BLADE] = new List<GameObject>();

                for (int i = 0; i < countSwordBlade; ++i)
                {
                    GameObject _skill = (GameObject)Instantiate(skillSwordBlade);
                    _skill.GetComponent<SkillDamage>().skillCategoryTypeNumber = (int)_weaponType;
                    _skill.GetComponent<SkillDamage>().damageDuration = 0.1f;
                    _skill.SetActive(false);
                    _skill.transform.SetParent(transform);
                    skillPool[(int)SkillType.SKILL_SWORD_BLADE].Add(_skill);
                }

                skillPool[(int)SkillType.SKILL_SWORD_BLADEWIND] = new List<GameObject>();

                for (int i = 0; i < countSwordBladeWind; ++i)
                {
                    GameObject _skill = (GameObject)Instantiate(skillSwordBladeWind);
                    _skill.GetComponent<SkillDamage>().skillCategoryTypeNumber = (int)_weaponType;
                    _skill.GetComponent<SkillDamage>().damageDuration = 0.1f;
                    _skill.SetActive(false);
                    _skill.transform.SetParent(transform);
                    skillPool[(int)SkillType.SKILL_SWORD_BLADEWIND].Add(_skill);
                }
                break;
        }
    }

    public GameObject GetPlayerSkill(SkillType _skillType)
    {
        GameObject returnedSkillPool = null;

        if (skillPool[(int)_skillType] == null)
            return null;


        for (int i = 0; i < skillPool[(int)_skillType].Count; ++i)
        {
            if (!skillPool[(int)_skillType][i].activeInHierarchy)
            {
                returnedSkillPool = skillPool[(int)_skillType][i];
                return returnedSkillPool;
            }
        }

        if (returnedSkillPool == null)
            return CreatDamagePool(_skillType);

        return null;
    }

    GameObject CreatDamagePool(SkillType _skillType)
    {
        GameObject _creatSkillPool = null;
        switch (_skillType)
        {
            case SkillType.SKILL_BAZOOKABULLET:
                _creatSkillPool = (GameObject)Instantiate(skillBazookaBullet);
                _creatSkillPool.GetComponent<SkillDamage>().skillCategoryTypeNumber = (int)PlayerSkillCategory.TOP_WEAPON_BAZOOKA;
                _creatSkillPool.GetComponent<SkillDamage>().damageDuration = 0.1f;
                break;
            case SkillType.SKILL_BAZOOKABLACKHOLETRAP:
                _creatSkillPool = (GameObject)Instantiate(skillBazookaBlackholeTrap);
                _creatSkillPool.GetComponent<SkillDamage>().skillCategoryTypeNumber = (int)PlayerSkillCategory.TOP_WEAPON_BAZOOKA;
                _creatSkillPool.GetComponent<SkillDamage>().damageDuration = 3;
                break;
            case SkillType.SKILL_REVOLVER:
                _creatSkillPool = (GameObject)Instantiate(skillRevolver);
                _creatSkillPool.GetComponent<SkillDamage>().skillCategoryTypeNumber = (int)PlayerSkillCategory.WEAPON_REVOLVER;
                _creatSkillPool.GetComponent<SkillDamage>().damageDuration = 0.1f;
                break;
            case SkillType.SKILL_KUNAI:
                _creatSkillPool = (GameObject)Instantiate(skillKunai);
                _creatSkillPool.GetComponent<SkillDamage>().skillCategoryTypeNumber = (int)PlayerSkillCategory.WEAPON_KUNAI;
                _creatSkillPool.GetComponent<SkillDamage>().damageDuration = 0.1f;
                break;
            case SkillType.SKILL_SWORD_BLADE:
                _creatSkillPool = (GameObject)Instantiate(skillSwordBlade);
                _creatSkillPool.GetComponent<SkillDamage>().skillCategoryTypeNumber = (int)PlayerSkillCategory.WEAPON_SWORD;
                _creatSkillPool.GetComponent<SkillDamage>().damageDuration = 0.1f;
                break;
            case SkillType.SKILL_SWORD_BLADEWIND:
                _creatSkillPool = (GameObject)Instantiate(skillSwordBladeWind);
                _creatSkillPool.GetComponent<SkillDamage>().skillCategoryTypeNumber = (int)PlayerSkillCategory.WEAPON_SWORD;
                _creatSkillPool.GetComponent<SkillDamage>().damageDuration = 0.1f;
                break;
            case SkillType.SKILL_MELTINGSHIELD:
                _creatSkillPool = (GameObject)Instantiate(skillMeltingShield);
                _creatSkillPool.GetComponent<SkillDamage>().skillCategoryTypeNumber = (int)PlayerSkillCategory.ACTIVE_MELTHINGSHIELD;
                _creatSkillPool.GetComponent<SkillDamage>().damageDuration = 0.1f;
                break;
            case SkillType.SKILL_SOCCERBALL:
                _creatSkillPool = (GameObject)Instantiate(skillSoccerBall);
                _creatSkillPool.GetComponent<SkillDamage>().skillCategoryTypeNumber = (int)PlayerSkillCategory.ACTIVE_SOCCERBALL;
                _creatSkillPool.GetComponent<SkillDamage>().damageDuration = 0.05f;
                break;
            case SkillType.SKILL_DURIAN:
                _creatSkillPool = (GameObject)Instantiate(skillDurian);
                _creatSkillPool.GetComponent<SkillDamage>().skillCategoryTypeNumber = (int)PlayerSkillCategory.ACTIVE_DURIAN;
                _creatSkillPool.GetComponent<SkillDamage>().damageDuration = 0.05f;
                break;
            case SkillType.SKILL_GUARDIAN:
                _creatSkillPool = (GameObject)Instantiate(skillGuardian);
                _creatSkillPool.GetComponent<SkillDamage>().skillCategoryTypeNumber = (int)PlayerSkillCategory.ACTIVE_GARDIAN;
                _creatSkillPool.GetComponent<SkillDamage>().damageDuration = 0.05f;
                break;
            case SkillType.SKILL_ROCKET_EXPLOSION:
                _creatSkillPool = (GameObject)Instantiate(skillRocketExplostion);
                _creatSkillPool.GetComponent<SkillDamage>().skillCategoryTypeNumber = (int)PlayerSkillCategory.ACTIVE_ROCKETLAUNCHER;
                _creatSkillPool.GetComponent<SkillDamage>().damageDuration = 0.15f;
                break;
            case SkillType.SKILL_BRICK:
                _creatSkillPool = (GameObject)Instantiate(skillBrick);
                _creatSkillPool.GetComponent<SkillDamage>().skillCategoryTypeNumber = (int)PlayerSkillCategory.ACTIVE_BRICK;
                _creatSkillPool.GetComponent<SkillDamage>().damageDuration = 0.1f;
                break;
            case SkillType.SKILL_DRILLSHOT:
                _creatSkillPool = (GameObject)Instantiate(skillDrillshot);
                _creatSkillPool.GetComponent<SkillDamage>().skillCategoryTypeNumber = (int)PlayerSkillCategory.ACTIVE_DRILLSHOT;
                _creatSkillPool.GetComponent<SkillDamage>().damageDuration = 0.1f;
                break;
            case SkillType.SKILL_LIGHTNINGENITTER:
                _creatSkillPool = (GameObject)Instantiate(skillLightning);
                _creatSkillPool.GetComponent<SkillDamage>().skillCategoryTypeNumber = (int)PlayerSkillCategory.ACTIVE_LIGHTNINGEMITTER;
                _creatSkillPool.GetComponent<SkillDamage>().damageDuration = 0.1f;
                break;
            case SkillType.SKILL_MOLOTOVCOCKTAIL:
                _creatSkillPool = (GameObject)Instantiate(skillMolotovCocktail);
                _creatSkillPool.GetComponent<SkillDamage>().skillCategoryTypeNumber = (int)PlayerSkillCategory.ACTIVE_MOLOTOVCOCKTAIL;
                _creatSkillPool.GetComponent<SkillDamage>().damageDuration = 0.1f;
                break;
            case SkillType.SKILL_BOOMERANG:
                _creatSkillPool = (GameObject)Instantiate(skillBoomerang);
                _creatSkillPool.GetComponent<SkillDamage>().skillCategoryTypeNumber = (int)PlayerSkillCategory.TOP_ACTIVE_BOOMERANG;
                _creatSkillPool.GetComponent<SkillDamage>().damageDuration = 0.1f;
                break;
            case SkillType.SKILL_TYPE_A_DRONE:
                _creatSkillPool = (GameObject)Instantiate(skillDroneTypeA);
                _creatSkillPool.GetComponent<SkillDamage>().skillCategoryTypeNumber = (int)PlayerSkillCategory.ACTIVE_DRONE_A;
                _creatSkillPool.GetComponent<SkillDamage>().damageDuration = 0.1f;
                break;
            case SkillType.SKILL_TYPE_B_DRONE:
                _creatSkillPool = (GameObject)Instantiate(skillDroneTypeB);
                _creatSkillPool.GetComponent<SkillDamage>().skillCategoryTypeNumber = (int)PlayerSkillCategory.ACTIVE_DRONE_B;
                _creatSkillPool.GetComponent<SkillDamage>().damageDuration = 0.1f;
                break;

        }

        if (_creatSkillPool != null)
        {
            _creatSkillPool.SetActive(false);
            _creatSkillPool.transform.SetParent(transform);
            skillPool[(int)_skillType].Add(_creatSkillPool);
        }

        for (int i = 0; i < skillPool[(int)_skillType].Count; ++i)
        {
            if (!skillPool[(int)_skillType][i].activeInHierarchy)
                return skillPool[(int)_skillType][i];
        }

        return null;
    }
}
