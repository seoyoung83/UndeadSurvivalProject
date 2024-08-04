using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{
    void InitializedData(PlayerMove _playerMove, WeaponsMuzzleTransform _weaponsMuzzleTransform);
    void LevelUp(int _currentWeaponSkillLevel);
    void SetData();
    void Shoot();
    void DoPassiveBuff(PlayerSkillCategory _passivType, float _value);

    void SetToReady(bool isTimeToReady);
}

public class PlayerWeaponController : MonoBehaviour
{
    public static PlayerWeaponController Instance;

    IWeapon currentWeapon;

    PlayerMove m_playerMove;

    [SerializeField] PlayerSkillCategory playerWeaponType;

    [SerializeField] int currentWeaponSkillLevel = 0; 

    [SerializeField] GameObject[] weaponSkillConroller;

    [SerializeField] Transform[] weaponEquipmentObjectTransform;
    Transform CurrentWeaponEquipTransform;


    private void Awake()
    {
        Instance = this;

        m_playerMove = GetComponent<PlayerMove>();

        InitializedData();
    }

    public int PlayerWeaponType
    {
        get { return (int)playerWeaponType; }
    }


    void InitializedData()
    {
        //무기 오브젝트 활성화
        int weaponTypeNumber = (int)playerWeaponType - (int)PlayerSkillCategory.TOP_WEAPON_BAZOOKA;

        for (int i = 0; i < weaponEquipmentObjectTransform.Length; ++i)
        {
            if (i == weaponTypeNumber)
                weaponEquipmentObjectTransform[i].gameObject.SetActive(true);
            else if (i != weaponTypeNumber)
                weaponEquipmentObjectTransform[i].gameObject.SetActive(false);
        }

        CurrentWeaponEquipTransform = weaponEquipmentObjectTransform[weaponTypeNumber];

        SkillDamagePooler.Instance.PlayerWeaponDamagePoolInitialized(((int)playerWeaponType));
    }

    public void SetPlayerWeaponData()
    {  
        //초기_ 선택한 WeaponController 생성
        int weaponTypeNumber = (int)playerWeaponType - (int)PlayerSkillCategory.TOP_WEAPON_BAZOOKA;

        GameObject currentWeaponControllerPrefab = Instantiate(weaponSkillConroller[weaponTypeNumber]);
        currentWeaponControllerPrefab.transform.SetParent(transform);

        currentWeapon = currentWeaponControllerPrefab.GetComponent<IWeapon>();

        //WeaponSkill_needs data update
        WeaponsMuzzleTransform weaponsMuzzleTransform = GetComponentInChildren<WeaponsMuzzleTransform>();
        currentWeapon.InitializedData(m_playerMove, weaponsMuzzleTransform);

        //ComboSkillManager _플레이어 Weapon data update
        ComboSkillData _data;
        _data.Type = PlayerSkillCategory.TOP_PASSIVE_EXO_BRACER + (int)playerWeaponType;
        _data.level = 0;
        ComboSkillManager.AddComboSkill(_data);

        //Weapon Object Image Setting
        Sprite weaponSprite = DataManager.Instance.SkillUIDataDictionary[(int)playerWeaponType].skillSprite[0];
        CurrentWeaponEquipTransform.GetComponentInChildren<SpriteRenderer>().sprite = weaponSprite;
    }

    void Update()
    {
        if (StageManager.playState == PlayState.End)
            return;

        MoveWeaponEquipPosition();
    }

    public void LevellingUpWeapon(int _level)
    {
        currentWeaponSkillLevel = _level;

        currentWeapon.LevelUp(currentWeaponSkillLevel);

        if (currentWeaponSkillLevel != 5)
            return;

        //플레이어가 들고 있는 Weapon Equip 변경
        Sprite weaponSprite = DataManager.Instance.SkillUIDataDictionary[(int)playerWeaponType].skillSprite[1];
        CurrentWeaponEquipTransform.GetComponentInChildren<SpriteRenderer>().sprite = weaponSprite;
    }

    public void CheckSkillActiveTime(bool isTimeToReady)
    {
        if ((int)playerWeaponType == (int)PlayerSkillCategory.WEAPON_KUNAI)
            currentWeapon.SetToReady(isTimeToReady);
    }

    public void DoPassiveBuff(PlayerSkillCategory _passivType, float _value)
    {
        currentWeapon.DoPassiveBuff(_passivType, _value);
    }

    // Weapon Object Image Transform Setting
    void MoveWeaponEquipPosition()
    {
        float radValue = Mathf.Atan2(m_playerMove.JoysticVector.normalized.y, m_playerMove.JoysticVector.normalized.x);
        float shootAngle = radValue * (180 / Mathf.PI);

        switch ((int)playerWeaponType)
        {
            case (int)PlayerSkillCategory.TOP_WEAPON_BAZOOKA:
                if (m_playerMove.JoysticVector.x >= 0) //플립 F
                {
                    CurrentWeaponEquipTransform.GetComponentInChildren<SpriteRenderer>().flipX = true;
                    CurrentWeaponEquipTransform.transform.position = transform.position + new Vector3(-0.1f, -0.1f, 0);
                    CurrentWeaponEquipTransform.transform.rotation = Quaternion.Euler(0, 0, shootAngle);

                    CurrentWeaponEquipTransform.transform.GetChild(0).localPosition = new Vector3(0.1f, 0.045f, 0); //총 이미지 
                    CurrentWeaponEquipTransform.transform.GetChild(0).GetChild(0).localPosition = new Vector3(0.3f, 0, 0); //불렛 shoot pos
                }
                else if (m_playerMove.JoysticVector.x < 0) //플립 T (디폴트)
                {
                    CurrentWeaponEquipTransform.GetComponentInChildren<SpriteRenderer>().flipX = false;
                    CurrentWeaponEquipTransform.transform.position = transform.position + new Vector3(0.1f, -0.1f, 0);
                    CurrentWeaponEquipTransform.transform.rotation = Quaternion.Euler(0, 0, shootAngle + 180);

                    CurrentWeaponEquipTransform.transform.GetChild(0).localPosition = new Vector3(-0.1f, 0.045f, 0); //총 이미지 
                    CurrentWeaponEquipTransform.transform.GetChild(0).GetChild(0).localPosition = new Vector3(-0.3f, 0, 0); //불렛 shoot pos
                }
                break;
            case (int)PlayerSkillCategory.WEAPON_REVOLVER:
                if (m_playerMove.JoysticVector.x >= 0)
                {
                    CurrentWeaponEquipTransform.GetComponentInChildren<SpriteRenderer>().flipY = true;
                    CurrentWeaponEquipTransform.transform.position = transform.position + new Vector3(0.12f, -0.1f, 0);
                    CurrentWeaponEquipTransform.transform.rotation = Quaternion.Euler(0, 0, shootAngle + 180);

                    CurrentWeaponEquipTransform.transform.GetChild(0).localPosition = new Vector3(-0.09f, -0.03f, 0); //총 이미지 
                    CurrentWeaponEquipTransform.transform.GetChild(0).GetChild(0).localPosition = new Vector3(-0.125f, -0.033f, 0); //불렛 shoot pos
                }
                else if (m_playerMove.JoysticVector.x < 0)
                {
                    CurrentWeaponEquipTransform.GetComponentInChildren<SpriteRenderer>().flipY = false;
                    CurrentWeaponEquipTransform.transform.position = transform.position + new Vector3(-0.12f, -0.1f, 0);
                    CurrentWeaponEquipTransform.transform.rotation = Quaternion.Euler(0, 0, shootAngle + 180);

                    CurrentWeaponEquipTransform.transform.GetChild(0).localPosition = new Vector3(-0.09f, 0.03f, 0); //총 이미지 
                    CurrentWeaponEquipTransform.transform.GetChild(0).GetChild(0).localPosition = new Vector3(-0.125f, 0.033f, 0); //불렛 shoot pos
                }
                break;
            case (int)PlayerSkillCategory.WEAPON_KUNAI:
                if (GetComponent<SpriteRenderer>().flipX == true)
                {
                    CurrentWeaponEquipTransform.GetComponent<SpriteRenderer>().flipX = true;
                    CurrentWeaponEquipTransform.transform.position = transform.position + new Vector3(0.002f, -0.09f, 0);
                    CurrentWeaponEquipTransform.transform.rotation = Quaternion.Euler(0, 0, -10f);
                }
                else if (GetComponent<SpriteRenderer>().flipX == false)
                {
                    CurrentWeaponEquipTransform.GetComponent<SpriteRenderer>().flipX = false;
                    CurrentWeaponEquipTransform.transform.position = transform.position + new Vector3(-0.002f, -0.09f, 0);
                    CurrentWeaponEquipTransform.transform.rotation = Quaternion.Euler(0, 0, 10f);
                }
                break;
            case (int)PlayerSkillCategory.WEAPON_SWORD:
                if (GetComponent<SpriteRenderer>().flipX == false)
                {
                    CurrentWeaponEquipTransform.GetComponent<SpriteRenderer>().flipX = true;
                    CurrentWeaponEquipTransform.transform.position = transform.position + new Vector3(0.025f, -0.078f, 0);
                    CurrentWeaponEquipTransform.transform.rotation = Quaternion.Euler(0, 0, -40f);
                }
                else if (GetComponent<SpriteRenderer>().flipX == true)
                {
                    CurrentWeaponEquipTransform.GetComponent<SpriteRenderer>().flipX = false;
                    CurrentWeaponEquipTransform.transform.position = transform.position + new Vector3(-0.025f, -0.078f, 0);
                    CurrentWeaponEquipTransform.transform.rotation = Quaternion.Euler(0, 0, 40f);
                }
                break;
        }
    }
}
