using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsMuzzleTransform : MonoBehaviour
{
    Transform playerTransform; 

    [SerializeField] Transform revolverBulletShootPosition;
    [SerializeField] Transform bazzokaBullerShootPosition;

    int currentWeaponTypeNumber;

    private void Start()
    {
        playerTransform = GetComponentInParent<PlayerMove>().transform;

        currentWeaponTypeNumber = PlayerWeaponController.Instance.PlayerWeaponType;

        switch (currentWeaponTypeNumber)
        {
            case (int)PlayerSkillCategory.TOP_WEAPON_BAZOOKA:
                transform.position = bazzokaBullerShootPosition.position;
                break;
            case (int)PlayerSkillCategory.WEAPON_REVOLVER:
                transform.position = revolverBulletShootPosition.position;
                break;
            case (int)PlayerSkillCategory.WEAPON_KUNAI:
                transform.position = playerTransform.position;
                break;
            case (int)PlayerSkillCategory.WEAPON_SWORD:
                transform.position = playerTransform.position;
                break;
        }
    }

    private void Update()
    {
        switch (currentWeaponTypeNumber)
        {
            case (int)PlayerSkillCategory.TOP_WEAPON_BAZOOKA:
                transform.position = bazzokaBullerShootPosition.position;
                break;
            case (int)PlayerSkillCategory.WEAPON_REVOLVER:
                transform.position = revolverBulletShootPosition.position;
                break;
            case (int)PlayerSkillCategory.WEAPON_KUNAI:
                transform.position = playerTransform.position;
                break;
            case (int)PlayerSkillCategory.WEAPON_SWORD:
                transform.position = playerTransform.position;
                break;
        }
    }
}
