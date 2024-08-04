using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerBulletType 
{
    BOOMERANG_NORMAL,
    BOOMERANG_SPECIAL,
    BRICK_NORMAL,
    BRICK_SPECIAL,
    DRILLSHOT_NORMAL,
    DRILLSHOT_SPECIAL,
    DRONEA_ROCKET_NORMAL,
    DRONEB_ROCKET_NORMAL,
    DURIAN_NORMAL,
    DURIAN_SPECIAL,
    DURIAN_SPECIAL_BULLET,
    GARDIAN_NORMAL, 
    GARDIAN_SPECIAL,
    LIGHTNINGEMITTER_NORMAL,
    LIGHTNINGEMITTER_SPECIAL,
    MELTHINGSHIELD_NORMAL,
    MELTHINGSHIELD_SPECIAL,
    MOLOTOVCOCKTAIL_NORMAL,
    MOLOTOVCOCKTAIL_SPECIAL,
    ROCKETLAUNCHER_NORMAL,
    ROCKETLAUNCHER_SPECIAL,
    SOCCERBALL_NORMAL,
    SOCCERBALL_SPECIAL,
    SOCCERBALL_SPECIAL_BULLET,
    //Weapon
    BAZOOKA_BLACKHOLE_BULLET,
    BAZOOKA_BLACKHOLE,
    BAZOOKA_BARRIER,
    KUNAI_BULLE_NORMAL,
    KUNAI_BULLE_SPECIAL,
    REVOLVER_BULLE_NORMAL,
    SWORD_MAINBLADE_NORMAL,
    SWORD_SIDEBLADE_NORMAL,
    SWORD_MAINBLADE_SPECIAL,
    SWORD_SIDEBLADE_SPECIAL,
    MAX_SIZE,
}

public class PlayerBulletPooler : MonoBehaviour
{
    public static PlayerBulletPooler Instance;

    [Header("Skill Boomerang Bullet")]
    [SerializeField] GameObject skillBoomerangNormal;
    [SerializeField] int countBoomerangNormalPool;

    [SerializeField] GameObject skillBoomerangSpecial;
    [SerializeField] int countBoomerangSpecialPool;

    [Header("Skill Brick Bullet")]
    [SerializeField] GameObject skillBrickNormal;
    [SerializeField] int countBrickNormalPool;

    [SerializeField] GameObject skillBrickSpecial;
    [SerializeField] int countBrickSpecialPool;

    [Header("Skill DrillShot Bullet")]
    [SerializeField] GameObject skillDrillShotNormal;
    [SerializeField] int countDrillShotNormalPool;

    [SerializeField] GameObject skillDrillShotSpecial;
    [SerializeField] int countDrillShotSpecialPool;

    [Header("Skill Drone Bullet")]
    [SerializeField] GameObject skillDroneRocketNormal;
    [SerializeField] int countDroneRocketNormalPool;

    [Header("Skill Durian Bullet")]
    [SerializeField] GameObject skillDurianNormal;
    [SerializeField] int countDurianNormalPool;

    [SerializeField] GameObject skillDurianSpecial;
    [SerializeField] int countDurianSpecialPool;

    [SerializeField] GameObject skillDurianSpecialBullet;
    [SerializeField] int countDurianSpecialBulletPool;

    [Header("Skill Gardian Bullet")] 
    [SerializeField] GameObject skillGardianNormal;
    [SerializeField] int countGardianNormalPool;

    [SerializeField] GameObject skillGardianSpecial;
    [SerializeField] int countGardianSpecialPool;

    [Header("Skill Lighningmitter's Bullet")] 
    [SerializeField] GameObject skillLightningNormal;
    [SerializeField] int countLightningNormalPool;

    [SerializeField] GameObject skillightningSpecial;
    [SerializeField] int countLightningSpecialPool;

    [Header("Skill MelthingShield Bullet")]
    [SerializeField] GameObject skillMelthingShieldNormal;
    [SerializeField] int countMelthingShieldNormalPool;

    [SerializeField] GameObject skillMelthingShieldSpecial;
    [SerializeField] int countMelthingShieldSpecialPool;

    [Header("Skill MolotoveCocktail Bullet")]
    [SerializeField] GameObject skillMolotoveCocktailNormal;
    [SerializeField] int countMolotoveCocktailNormalPool;

    [SerializeField] GameObject skillMolotoveCocktailSpecial;
    [SerializeField] int countMolotoveCocktailSpecialPool;

    [Header("Skill RocketLauncher's Bullet")]
    [SerializeField] GameObject skillRocketNormal;
    [SerializeField] int countRocketNormalPool;

    [SerializeField] GameObject skillRocketSpecial;
    [SerializeField] int countRocketSpecialPool;

    [Header("Skill Soccerball Bullet")]
    [SerializeField] GameObject skillSoccerballNormal;
    [SerializeField] int countSoccerballNormalPool;

    [SerializeField] GameObject skillSoccerballSpecial;
    [SerializeField] int countSoccerballSpecialPool;

    [SerializeField] GameObject skillSoccerballSpecialBullet;
    [SerializeField] int countSoccerballSpecialBulletPool;

    [Header("Weapon Bazooka")]
    [SerializeField] GameObject weaponBazookaBlackholeBullet; 
    [SerializeField] int countBazookaBlackholeBulletPool;

    [SerializeField] GameObject weaponBazookaBlackhole; 
    [SerializeField] int countBazookaBlackholePool;

    [SerializeField] GameObject weaponBazookaBarrier;
    [SerializeField] int countBazookaBarrierPool;

    [Header("Weapon Kunai")]
    [SerializeField] GameObject weaponKunaiBulletNormal; 
    [SerializeField] int countKunaiBulletNormalPool;

    [SerializeField] GameObject weaponKunaiBulletSpecial; 
    [SerializeField] int countKunaiBulletSpecialPlool;

    [Header("Weapon Revolver")]
    [SerializeField] GameObject weaponRevolverBulletNormal; 
    [SerializeField] int countRevolverBulletNormalPool;

    [Header("Weapon Sword")]
    [SerializeField] GameObject weaponSwordMainBladeNormal;
    [SerializeField] int countSwordMainBladeNormalPool;

    [SerializeField] GameObject weaponSwordSideBladeNormal;
    [SerializeField] int countSwordSideBladeNormalPool;

    [SerializeField] GameObject weaponSwordMainBladeSpecial;
    [SerializeField] int countSwordMainBladeSpecialPool;

    [SerializeField] GameObject weaponSwordSideBladeSpecial;
    [SerializeField] int countSwordSideBladeSpecialPool;

    List<GameObject>[] playerBulletPool = new List<GameObject>[(int)PlayerBulletType.MAX_SIZE];

    private void Awake()
    {
        Instance = this;
    }

    public void PlayerBulletInitialized(PlayerBulletType _PlayerBulletType)
    {
        switch (_PlayerBulletType)
        {
            case PlayerBulletType.BOOMERANG_NORMAL:
                playerBulletPool[(int)PlayerBulletType.BOOMERANG_NORMAL] = new List<GameObject>();
                for (int i = 0; i < countBoomerangNormalPool; ++i)
                {
                    GameObject _bullet = (GameObject)Instantiate(skillBoomerangNormal);
                    _bullet.SetActive(false);
                    _bullet.transform.SetParent(transform);
                    playerBulletPool[(int)PlayerBulletType.BOOMERANG_NORMAL].Add(_bullet);
                }
                break;
            case PlayerBulletType.BOOMERANG_SPECIAL:
                playerBulletPool[(int)PlayerBulletType.BOOMERANG_SPECIAL] = new List<GameObject>();
                for (int i = 0; i < countBoomerangSpecialPool; ++i)
                {
                    GameObject _bullet = (GameObject)Instantiate(skillBoomerangSpecial);
                    _bullet.SetActive(false);
                    _bullet.transform.SetParent(transform);
                    playerBulletPool[(int)PlayerBulletType.BOOMERANG_SPECIAL].Add(_bullet);
                }
                break;
            case PlayerBulletType.BRICK_NORMAL:
                playerBulletPool[(int)PlayerBulletType.BRICK_NORMAL] = new List<GameObject>();
                for (int i = 0; i < countBrickNormalPool; ++i)
                {
                    GameObject _bullet = (GameObject)Instantiate(skillBrickNormal);
                    _bullet.SetActive(false);
                    _bullet.transform.SetParent(transform);
                    playerBulletPool[(int)PlayerBulletType.BRICK_NORMAL].Add(_bullet);
                }
                break;
            case PlayerBulletType.BRICK_SPECIAL:
                playerBulletPool[(int)PlayerBulletType.BRICK_SPECIAL] = new List<GameObject>();
                for (int i = 0; i < countBrickSpecialPool; ++i)
                {
                    GameObject _bullet = (GameObject)Instantiate(skillBrickSpecial);
                    _bullet.SetActive(false);
                    _bullet.transform.SetParent(transform);
                    playerBulletPool[(int)PlayerBulletType.BRICK_SPECIAL].Add(_bullet);
                }
                break;
                case PlayerBulletType.DRILLSHOT_NORMAL:
                playerBulletPool[(int)PlayerBulletType.DRILLSHOT_NORMAL] = new List<GameObject>();
                for (int i = 0; i < countDrillShotNormalPool; ++i)
                {
                    GameObject _bullet = (GameObject)Instantiate(skillDrillShotNormal);
                    _bullet.SetActive(false);
                    _bullet.transform.SetParent(transform);
                    playerBulletPool[(int)PlayerBulletType.DRILLSHOT_NORMAL].Add(_bullet);
                }
                break;
            case PlayerBulletType.DRILLSHOT_SPECIAL:
                playerBulletPool[(int)PlayerBulletType.DRILLSHOT_SPECIAL] = new List<GameObject>();
                for (int i = 0; i < countDrillShotSpecialPool; ++i)
                {
                    GameObject _bullet = (GameObject)Instantiate(skillDrillShotSpecial);
                    _bullet.SetActive(false);
                    _bullet.transform.SetParent(transform);
                    playerBulletPool[(int)PlayerBulletType.DRILLSHOT_SPECIAL].Add(_bullet);
                }
                break;
            case PlayerBulletType.DRONEA_ROCKET_NORMAL:
                playerBulletPool[(int)PlayerBulletType.DRONEA_ROCKET_NORMAL] = new List<GameObject>();
                for (int i = 0; i < countDroneRocketNormalPool; ++i)
                {
                    GameObject _bullet = (GameObject)Instantiate(skillDroneRocketNormal);
                    _bullet.SetActive(false);
                    _bullet.transform.SetParent(transform);
                    playerBulletPool[(int)PlayerBulletType.DRONEA_ROCKET_NORMAL].Add(_bullet);
                }
                break;
            case PlayerBulletType.DRONEB_ROCKET_NORMAL:
                playerBulletPool[(int)PlayerBulletType.DRONEB_ROCKET_NORMAL] = new List<GameObject>();
                for (int i = 0; i < countDroneRocketNormalPool; ++i)
                {
                    GameObject _bullet = (GameObject)Instantiate(skillDroneRocketNormal);
                    _bullet.SetActive(false);
                    _bullet.transform.SetParent(transform);
                    playerBulletPool[(int)PlayerBulletType.DRONEB_ROCKET_NORMAL].Add(_bullet);
                }
                break;
            case PlayerBulletType.DURIAN_NORMAL:
                playerBulletPool[(int)PlayerBulletType.DURIAN_NORMAL] = new List<GameObject>();
                for (int i = 0; i < countDurianNormalPool; ++i)
                {
                    GameObject _bullet = (GameObject)Instantiate(skillDurianNormal);
                    _bullet.SetActive(false);
                    _bullet.transform.SetParent(transform);
                    playerBulletPool[(int)PlayerBulletType.DURIAN_NORMAL].Add(_bullet);
                }
                break;
            case PlayerBulletType.DURIAN_SPECIAL:
                playerBulletPool[(int)PlayerBulletType.DURIAN_SPECIAL] = new List<GameObject>();
                for (int i = 0; i < countDurianSpecialPool; ++i)
                {
                    GameObject _bullet = (GameObject)Instantiate(skillDurianSpecial);
                    _bullet.SetActive(false);
                    _bullet.transform.SetParent(transform);
                    playerBulletPool[(int)PlayerBulletType.DURIAN_SPECIAL].Add(_bullet);
                }
                break; 
            case PlayerBulletType.DURIAN_SPECIAL_BULLET:
                playerBulletPool[(int)PlayerBulletType.DURIAN_SPECIAL_BULLET] = new List<GameObject>();
                for (int i = 0; i < countDurianSpecialBulletPool; ++i)
                {
                    GameObject _bullet = (GameObject)Instantiate(skillDurianSpecialBullet);
                    _bullet.SetActive(false);
                    _bullet.transform.SetParent(transform);
                    playerBulletPool[(int)PlayerBulletType.DURIAN_SPECIAL_BULLET].Add(_bullet);
                }
                break;
            case PlayerBulletType.GARDIAN_NORMAL:
                playerBulletPool[(int)PlayerBulletType.GARDIAN_NORMAL] = new List<GameObject>();
                for (int i = 0; i < countGardianNormalPool; ++i)
                {
                    GameObject _bullet = (GameObject)Instantiate(skillGardianNormal);
                    _bullet.SetActive(false);
                    _bullet.transform.SetParent(transform);
                    playerBulletPool[(int)PlayerBulletType.GARDIAN_NORMAL].Add(_bullet);
                }
                break;
            case PlayerBulletType.GARDIAN_SPECIAL:
                playerBulletPool[(int)PlayerBulletType.GARDIAN_SPECIAL] = new List<GameObject>();
                for (int i = 0; i < countGardianSpecialPool; ++i)
                {
                    GameObject _bullet = (GameObject)Instantiate(skillGardianSpecial);
                    _bullet.SetActive(false);
                    _bullet.transform.SetParent(transform);
                    playerBulletPool[(int)PlayerBulletType.GARDIAN_SPECIAL].Add(_bullet);
                }
                break;
            case PlayerBulletType.LIGHTNINGEMITTER_NORMAL:
                playerBulletPool[(int)PlayerBulletType.LIGHTNINGEMITTER_NORMAL] = new List<GameObject>();
                for (int i = 0; i < countLightningNormalPool; ++i)
                {
                    GameObject _bullet = (GameObject)Instantiate(skillLightningNormal);
                    _bullet.SetActive(false);
                    _bullet.transform.SetParent(transform);
                    playerBulletPool[(int)PlayerBulletType.LIGHTNINGEMITTER_NORMAL].Add(_bullet);
                }
                break;
            case PlayerBulletType.LIGHTNINGEMITTER_SPECIAL:
                playerBulletPool[(int)PlayerBulletType.LIGHTNINGEMITTER_SPECIAL] = new List<GameObject>();
                for (int i = 0; i < countLightningSpecialPool; ++i)
                {
                    GameObject _bullet = (GameObject)Instantiate(skillightningSpecial);
                    _bullet.SetActive(false);
                    _bullet.transform.SetParent(transform);
                    playerBulletPool[(int)PlayerBulletType.LIGHTNINGEMITTER_SPECIAL].Add(_bullet);
                }
                break;
            case PlayerBulletType.MELTHINGSHIELD_NORMAL:
                playerBulletPool[(int)PlayerBulletType.MELTHINGSHIELD_NORMAL] = new List<GameObject>();
                for (int i = 0; i < countMelthingShieldNormalPool; ++i)
                {
                    GameObject _bullet = (GameObject)Instantiate(skillMelthingShieldNormal);
                    _bullet.SetActive(false);
                    _bullet.transform.SetParent(transform);
                    playerBulletPool[(int)PlayerBulletType.MELTHINGSHIELD_NORMAL].Add(_bullet);
                }
                break;
            case PlayerBulletType.MELTHINGSHIELD_SPECIAL:
                playerBulletPool[(int)PlayerBulletType.MELTHINGSHIELD_SPECIAL] = new List<GameObject>();
                for (int i = 0; i < countMelthingShieldSpecialPool; ++i)
                {
                    GameObject _bullet = (GameObject)Instantiate(skillMelthingShieldSpecial);
                    _bullet.SetActive(false);
                    _bullet.transform.SetParent(transform);
                    playerBulletPool[(int)PlayerBulletType.MELTHINGSHIELD_SPECIAL].Add(_bullet);
                }
                break;
            case PlayerBulletType.MOLOTOVCOCKTAIL_NORMAL:
                playerBulletPool[(int)PlayerBulletType.MOLOTOVCOCKTAIL_NORMAL] = new List<GameObject>();
                for (int i = 0; i < countMolotoveCocktailNormalPool; ++i)
                {
                    GameObject _bullet = (GameObject)Instantiate(skillMolotoveCocktailNormal);
                    _bullet.SetActive(false);
                    _bullet.transform.SetParent(transform);
                    playerBulletPool[(int)PlayerBulletType.MOLOTOVCOCKTAIL_NORMAL].Add(_bullet);
                }
                break;
            case PlayerBulletType.MOLOTOVCOCKTAIL_SPECIAL:
                playerBulletPool[(int)PlayerBulletType.MOLOTOVCOCKTAIL_SPECIAL] = new List<GameObject>();
                for (int i = 0; i < countMolotoveCocktailSpecialPool; ++i)
                {
                    GameObject _bullet = (GameObject)Instantiate(skillMolotoveCocktailSpecial);
                    _bullet.SetActive(false);
                    _bullet.transform.SetParent(transform);
                    playerBulletPool[(int)PlayerBulletType.MOLOTOVCOCKTAIL_SPECIAL].Add(_bullet);
                }
                break;
            case PlayerBulletType.ROCKETLAUNCHER_NORMAL:
                playerBulletPool[(int)PlayerBulletType.ROCKETLAUNCHER_NORMAL] = new List<GameObject>();
                for (int i = 0; i < countRocketNormalPool; ++i)
                {
                    GameObject _bullet = (GameObject)Instantiate(skillRocketNormal);
                    _bullet.SetActive(false);
                    _bullet.transform.SetParent(transform);
                    playerBulletPool[(int)PlayerBulletType.ROCKETLAUNCHER_NORMAL].Add(_bullet);
                }
                break;
            case PlayerBulletType.ROCKETLAUNCHER_SPECIAL:
                playerBulletPool[(int)PlayerBulletType.ROCKETLAUNCHER_SPECIAL] = new List<GameObject>();
                for (int i = 0; i < countRocketSpecialPool; ++i)
                {
                    GameObject _bullet = (GameObject)Instantiate(skillRocketSpecial);
                    _bullet.SetActive(false);
                    _bullet.transform.SetParent(transform);
                    playerBulletPool[(int)PlayerBulletType.ROCKETLAUNCHER_SPECIAL].Add(_bullet);
                }
                break;
            case PlayerBulletType.SOCCERBALL_NORMAL:
                playerBulletPool[(int)PlayerBulletType.SOCCERBALL_NORMAL] = new List<GameObject>();
                for (int i = 0; i < countSoccerballNormalPool; ++i)
                {
                    GameObject _bullet = (GameObject)Instantiate(skillSoccerballNormal);
                    _bullet.SetActive(false);
                    _bullet.transform.SetParent(transform);
                    playerBulletPool[(int)PlayerBulletType.SOCCERBALL_NORMAL].Add(_bullet);
                }
                break;
            case PlayerBulletType.SOCCERBALL_SPECIAL:
                playerBulletPool[(int)PlayerBulletType.SOCCERBALL_SPECIAL] = new List<GameObject>();
                for (int i = 0; i < countSoccerballSpecialPool; ++i)
                {
                    GameObject _bullet = (GameObject)Instantiate(skillSoccerballSpecial);
                    _bullet.SetActive(false);
                    _bullet.transform.SetParent(transform);
                    playerBulletPool[(int)PlayerBulletType.SOCCERBALL_SPECIAL].Add(_bullet);
                }
                break;
            case PlayerBulletType.SOCCERBALL_SPECIAL_BULLET: 
                playerBulletPool[(int)PlayerBulletType.SOCCERBALL_SPECIAL_BULLET] = new List<GameObject>();
                for (int i = 0; i < countSoccerballSpecialBulletPool; ++i)
                {
                    GameObject _bullet = (GameObject)Instantiate(skillSoccerballSpecialBullet);
                    _bullet.SetActive(false);
                    _bullet.transform.SetParent(transform);
                    playerBulletPool[(int)PlayerBulletType.SOCCERBALL_SPECIAL_BULLET].Add(_bullet);
                }
                break; 
            case PlayerBulletType.BAZOOKA_BLACKHOLE_BULLET:
                playerBulletPool[(int)PlayerBulletType.BAZOOKA_BLACKHOLE_BULLET] = new List<GameObject>();
                for (int i = 0; i < countBazookaBlackholeBulletPool; ++i)
                {
                    GameObject _bullet = (GameObject)Instantiate(weaponBazookaBlackholeBullet); 
                    _bullet.SetActive(false);
                    _bullet.transform.SetParent(transform);
                    playerBulletPool[(int)PlayerBulletType.BAZOOKA_BLACKHOLE_BULLET].Add(_bullet);
                }
                break;
            case PlayerBulletType.BAZOOKA_BLACKHOLE:
                playerBulletPool[(int)PlayerBulletType.BAZOOKA_BLACKHOLE] = new List<GameObject>();
                for (int i = 0; i < countBazookaBlackholePool; ++i)
                {
                    GameObject _bullet = (GameObject)Instantiate(weaponBazookaBlackhole);
                    _bullet.SetActive(false);
                    _bullet.transform.SetParent(transform);
                    playerBulletPool[(int)PlayerBulletType.BAZOOKA_BLACKHOLE].Add(_bullet);
                }
                break;
            case PlayerBulletType.BAZOOKA_BARRIER:
                playerBulletPool[(int)PlayerBulletType.BAZOOKA_BARRIER] = new List<GameObject>();
                for (int i = 0; i < countBazookaBarrierPool; ++i)
                {
                    GameObject _bullet = (GameObject)Instantiate(weaponBazookaBarrier);
                    _bullet.SetActive(false);
                    _bullet.transform.SetParent(transform);
                    playerBulletPool[(int)PlayerBulletType.BAZOOKA_BARRIER].Add(_bullet);
                }
                break;
            case PlayerBulletType.KUNAI_BULLE_NORMAL:
                playerBulletPool[(int)PlayerBulletType.KUNAI_BULLE_NORMAL] = new List<GameObject>();
                for (int i = 0; i < countKunaiBulletNormalPool; ++i)
                {
                    GameObject _bullet = (GameObject)Instantiate(weaponKunaiBulletNormal);
                    _bullet.SetActive(false);
                    _bullet.transform.SetParent(transform);
                    playerBulletPool[(int)PlayerBulletType.KUNAI_BULLE_NORMAL].Add(_bullet);
                }
                break;
            case PlayerBulletType.KUNAI_BULLE_SPECIAL:
                playerBulletPool[(int)PlayerBulletType.KUNAI_BULLE_SPECIAL] = new List<GameObject>();
                for (int i = 0; i < countKunaiBulletSpecialPlool; ++i)
                {
                    GameObject _bullet = (GameObject)Instantiate(weaponKunaiBulletSpecial);
                    _bullet.SetActive(false);
                    _bullet.transform.SetParent(transform);
                    playerBulletPool[(int)PlayerBulletType.KUNAI_BULLE_SPECIAL].Add(_bullet);
                }
                break;
            case PlayerBulletType.REVOLVER_BULLE_NORMAL:
                playerBulletPool[(int)PlayerBulletType.REVOLVER_BULLE_NORMAL] = new List<GameObject>();
                for (int i = 0; i < countRevolverBulletNormalPool; ++i)
                {
                    GameObject _bullet = (GameObject)Instantiate(weaponRevolverBulletNormal);
                    _bullet.SetActive(false);
                    _bullet.transform.SetParent(transform);
                    playerBulletPool[(int)PlayerBulletType.REVOLVER_BULLE_NORMAL].Add(_bullet);
                }
                break;
            case PlayerBulletType.SWORD_MAINBLADE_NORMAL:
                playerBulletPool[(int)PlayerBulletType.SWORD_MAINBLADE_NORMAL] = new List<GameObject>();
                for (int i = 0; i < countSwordMainBladeNormalPool; ++i)
                {
                    GameObject _bullet = (GameObject)Instantiate(weaponSwordMainBladeNormal);
                    _bullet.SetActive(false);
                    _bullet.transform.SetParent(transform);
                    playerBulletPool[(int)PlayerBulletType.SWORD_MAINBLADE_NORMAL].Add(_bullet);
                }
                break;
            case PlayerBulletType.SWORD_SIDEBLADE_NORMAL:
                playerBulletPool[(int)PlayerBulletType.SWORD_SIDEBLADE_NORMAL] = new List<GameObject>();
                for (int i = 0; i < countSwordSideBladeNormalPool; ++i)
                {
                    GameObject _bullet = (GameObject)Instantiate(weaponSwordSideBladeNormal);
                    _bullet.SetActive(false);
                    _bullet.transform.SetParent(transform);
                    playerBulletPool[(int)PlayerBulletType.SWORD_SIDEBLADE_NORMAL].Add(_bullet);
                }
                break;
            case PlayerBulletType.SWORD_MAINBLADE_SPECIAL:
                playerBulletPool[(int)PlayerBulletType.SWORD_MAINBLADE_SPECIAL] = new List<GameObject>();
                for (int i = 0; i < countSwordMainBladeSpecialPool; ++i)
                {
                    GameObject _bullet = (GameObject)Instantiate(weaponSwordMainBladeSpecial);
                    _bullet.SetActive(false);
                    _bullet.transform.SetParent(transform);
                    playerBulletPool[(int)PlayerBulletType.SWORD_MAINBLADE_SPECIAL].Add(_bullet);
                }
                break;
            case PlayerBulletType.SWORD_SIDEBLADE_SPECIAL:
                playerBulletPool[(int)PlayerBulletType.SWORD_SIDEBLADE_SPECIAL] = new List<GameObject>();
                for (int i = 0; i < countSwordSideBladeSpecialPool; ++i)
                {
                    GameObject _bullet = (GameObject)Instantiate(weaponSwordSideBladeSpecial);
                    _bullet.SetActive(false);
                    _bullet.transform.SetParent(transform);
                    playerBulletPool[(int)PlayerBulletType.SWORD_SIDEBLADE_SPECIAL].Add(_bullet);
                }
                break;
        }
    }

    public GameObject GetPlayerBullet(PlayerBulletType _playerBulletType)
    {
        GameObject returnedGameObject = null;

        if (playerBulletPool[(int)_playerBulletType] == null)
            return null;

        for (int i = 0; i < playerBulletPool[(int)_playerBulletType].Count; ++i)
        {
            if (!playerBulletPool[(int)_playerBulletType][i].activeInHierarchy)
            {
                returnedGameObject = playerBulletPool[(int)_playerBulletType][i];
                return returnedGameObject;
            }
        }

        if (returnedGameObject == null)
            return CreatPlayerBullet(_playerBulletType);

        return null;
    }

    GameObject CreatPlayerBullet(PlayerBulletType _playerBulletType)
    {
        GameObject _creatPlayerBullet = null;

        switch (_playerBulletType)
        {
            case PlayerBulletType.BOOMERANG_NORMAL:
                _creatPlayerBullet = (GameObject)Instantiate(skillBoomerangNormal);
                break;
            case PlayerBulletType.BOOMERANG_SPECIAL:
                _creatPlayerBullet = (GameObject)Instantiate(skillBoomerangSpecial);
                break;
            case PlayerBulletType.BRICK_NORMAL:
                _creatPlayerBullet = (GameObject)Instantiate(skillBrickNormal);
                break;
            case PlayerBulletType.BRICK_SPECIAL:
                _creatPlayerBullet = (GameObject)Instantiate(skillBrickSpecial);
                break;
            case PlayerBulletType.DRILLSHOT_NORMAL:
                _creatPlayerBullet = (GameObject)Instantiate(skillDrillShotNormal);
                break;
            case PlayerBulletType.DRILLSHOT_SPECIAL:
                _creatPlayerBullet = (GameObject)Instantiate(skillDrillShotSpecial);
                break;
            case PlayerBulletType.DRONEA_ROCKET_NORMAL:
                _creatPlayerBullet = (GameObject)Instantiate(skillDroneRocketNormal);
                break;
            case PlayerBulletType.DRONEB_ROCKET_NORMAL:
                _creatPlayerBullet = (GameObject)Instantiate(skillDroneRocketNormal);
                break;
            case PlayerBulletType.DURIAN_NORMAL:
                _creatPlayerBullet = (GameObject)Instantiate(skillDurianNormal);
                break;
            case PlayerBulletType.DURIAN_SPECIAL:
                _creatPlayerBullet = (GameObject)Instantiate(skillDurianSpecial);
                break;
            case PlayerBulletType.DURIAN_SPECIAL_BULLET:
                _creatPlayerBullet = (GameObject)Instantiate(skillDurianSpecialBullet);
                break;
            case PlayerBulletType.GARDIAN_NORMAL:
                _creatPlayerBullet = (GameObject)Instantiate(skillGardianNormal);
                break;
            case PlayerBulletType.GARDIAN_SPECIAL:
                _creatPlayerBullet = (GameObject)Instantiate(skillGardianSpecial);
                break;
            case PlayerBulletType.LIGHTNINGEMITTER_NORMAL:
                _creatPlayerBullet = (GameObject)Instantiate(skillLightningNormal);
                break;
            case PlayerBulletType.LIGHTNINGEMITTER_SPECIAL:
                _creatPlayerBullet = (GameObject)Instantiate(skillightningSpecial);
                break;
            case PlayerBulletType.MELTHINGSHIELD_NORMAL:
                _creatPlayerBullet = (GameObject)Instantiate(skillMelthingShieldNormal);
                break;
            case PlayerBulletType.MELTHINGSHIELD_SPECIAL:
                _creatPlayerBullet = (GameObject)Instantiate(skillMelthingShieldSpecial);
                break;
            case PlayerBulletType.MOLOTOVCOCKTAIL_NORMAL:
                _creatPlayerBullet = (GameObject)Instantiate(skillMolotoveCocktailNormal);
                break;
            case PlayerBulletType.MOLOTOVCOCKTAIL_SPECIAL:
                _creatPlayerBullet = (GameObject)Instantiate(skillMolotoveCocktailSpecial);
                break;
            case PlayerBulletType.ROCKETLAUNCHER_NORMAL:
                _creatPlayerBullet = (GameObject)Instantiate(skillRocketNormal);
                break;
            case PlayerBulletType.ROCKETLAUNCHER_SPECIAL:
                _creatPlayerBullet = (GameObject)Instantiate(skillRocketSpecial);
                break;
            case PlayerBulletType.SOCCERBALL_NORMAL:
                _creatPlayerBullet = (GameObject)Instantiate(skillSoccerballNormal);
                break;
            case PlayerBulletType.SOCCERBALL_SPECIAL:
                _creatPlayerBullet = (GameObject)Instantiate(skillSoccerballSpecial);
                break; 
            case PlayerBulletType.SOCCERBALL_SPECIAL_BULLET:
                _creatPlayerBullet = (GameObject)Instantiate(skillSoccerballSpecialBullet);
                break;
            case PlayerBulletType.BAZOOKA_BLACKHOLE_BULLET:
                _creatPlayerBullet = (GameObject)Instantiate(weaponBazookaBlackholeBullet);
                break;
            case PlayerBulletType.BAZOOKA_BLACKHOLE:
                _creatPlayerBullet = (GameObject)Instantiate(weaponBazookaBlackhole);
                break;
            case PlayerBulletType.BAZOOKA_BARRIER:
                _creatPlayerBullet = (GameObject)Instantiate(weaponBazookaBarrier);
                break;
            case PlayerBulletType.KUNAI_BULLE_NORMAL:
                _creatPlayerBullet = (GameObject)Instantiate(weaponKunaiBulletNormal);
                break;
            case PlayerBulletType.KUNAI_BULLE_SPECIAL:
                _creatPlayerBullet = (GameObject)Instantiate(weaponKunaiBulletSpecial);
                break;
            case PlayerBulletType.REVOLVER_BULLE_NORMAL:
                _creatPlayerBullet = (GameObject)Instantiate(weaponRevolverBulletNormal);
                break;
            case PlayerBulletType.SWORD_MAINBLADE_NORMAL:
                _creatPlayerBullet = (GameObject)Instantiate(weaponSwordMainBladeNormal);
                break;
            case PlayerBulletType.SWORD_SIDEBLADE_NORMAL:
                _creatPlayerBullet = (GameObject)Instantiate(weaponSwordSideBladeNormal);
                break;
            case PlayerBulletType.SWORD_MAINBLADE_SPECIAL:
                _creatPlayerBullet = (GameObject)Instantiate(weaponSwordMainBladeSpecial);
                break;
            case PlayerBulletType.SWORD_SIDEBLADE_SPECIAL:
                _creatPlayerBullet = (GameObject)Instantiate(weaponSwordSideBladeSpecial);
                break;
        }

        if (_creatPlayerBullet != null)
        {
            _creatPlayerBullet.SetActive(false);
            _creatPlayerBullet.transform.SetParent(transform);
            playerBulletPool[(int)_playerBulletType].Add(_creatPlayerBullet);
        }


        for (int i = 0; i < playerBulletPool[(int)_playerBulletType].Count; ++i)
        {
            if (!playerBulletPool[(int)_playerBulletType][i].activeInHierarchy)
            {
                return playerBulletPool[(int)_playerBulletType][i];
            }
        }

        return null;

    }

    public void ExpiredPlayerBullet(PlayerBulletType _playerBulletType)
    {
        foreach (GameObject obj in playerBulletPool[(int)_playerBulletType])
        {
            if (obj != null)
                obj.SetActive(false);
        }
    }

    public void DestroyPlayerBullet(PlayerBulletType _playerBulletType)
    {
        if (playerBulletPool[(int)_playerBulletType] != null)
        {
            for (int j = 0; j < playerBulletPool[(int)_playerBulletType].Count; ++j)
            {
                Destroy(playerBulletPool[(int)_playerBulletType][j].gameObject);
            }
        }
    }
}

