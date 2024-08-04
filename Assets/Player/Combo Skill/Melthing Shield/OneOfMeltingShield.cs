using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OneOfMeltingShield : MonoBehaviour, IPlayerBullet
{
    Transform playerTransform;

    SpriteRenderer spriteRenderer;

    int skillLevel;

    float checkingTimer = 0;
    float maxSize;
    float sizeSpeed;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void UpdateSkillLevel(int _level)
    {
        skillLevel = _level;
    }

    public void UpdateSkillInfo(float _maxSize, float _sizeSpeed)
    {
        transform.localScale = new Vector3(0.1f, 0.1f, 0);

        maxSize = _maxSize;

        sizeSpeed = _sizeSpeed;
    }

    private void Update()
    {
        transform.position = playerTransform.position;

        checkingTimer += Time.deltaTime * sizeSpeed;
        float scale = 0.1f + checkingTimer;
        transform.localScale = new Vector3(scale, scale, 0);

        float alpha = 1 - checkingTimer * (1/maxSize);
        Color newColor = spriteRenderer.color.WithAlpha(alpha);
        spriteRenderer.color = newColor;

        if (scale > maxSize)
        {
            gameObject.SetActive(false);
            checkingTimer = 0f;
            spriteRenderer.color.WithAlpha(1);
            transform.localScale = new Vector3(0.1f, 0.1f, 0);
        }
    }

    public void Fire(Vector3 shootVect)
    {
        transform.rotation = Quaternion.Euler(0, 0, shootVect.z);
    }

    public void SetMuzzleTransform(Transform muzzleTransform) 
    {
        playerTransform = muzzleTransform;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyStat _enemyStat = collision.gameObject.GetComponent<EnemyStat>();
            if (_enemyStat && !_enemyStat.isDeath)
            {
                GameObject _shieldDamage = SkillDamagePooler.Instance.GetPlayerSkill(SkillType.SKILL_MELTINGSHIELD);

                if (_shieldDamage != null)
                {
                    _shieldDamage.transform.position = collision.transform.position;
                    _shieldDamage.GetComponent<DamageMeltingShield>().GetSkillLevel(skillLevel);
                    _shieldDamage.SetActive(true);
                }
            }
        }

        if (collision.gameObject.CompareTag("Boss"))
        {
            BossStat _enemyStat = collision.gameObject.GetComponent<BossStat>();

            if (_enemyStat)
            {
                GameObject _shieldDamage = SkillDamagePooler.Instance.GetPlayerSkill(SkillType.SKILL_MELTINGSHIELD);

                if (_shieldDamage != null)
                {
                    _shieldDamage.transform.position = collision.transform.position;
                    _shieldDamage.GetComponent<DamageMeltingShield>().GetSkillLevel(skillLevel);
                    _shieldDamage.SetActive(true);
                }
            }
        }

        if (collision.gameObject.CompareTag("InfiniteHPObject"))
        {
            InfiniteStat _objectStat = collision.gameObject.GetComponent<InfiniteStat>();
            if (_objectStat)
            {
                GameObject _shieldDamage = SkillDamagePooler.Instance.GetPlayerSkill(SkillType.SKILL_MELTINGSHIELD);

                if (_shieldDamage != null)
                {
                    _shieldDamage.transform.position = collision.transform.position;
                    _shieldDamage.GetComponent<DamageMeltingShield>().GetSkillLevel(skillLevel);
                    _shieldDamage.SetActive(true);
                }
            }
        }

        if (collision.gameObject.CompareTag("PickupBox"))
        {
            PickupBox _box = collision.gameObject.GetComponent<PickupBox>();
            if (_box)
            {
                _box.GetComponent<PickupBox>().SetBoxState(false);
            }
        }
    }
}
