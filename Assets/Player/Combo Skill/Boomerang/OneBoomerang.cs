using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OneBoomerang : MonoBehaviour, IPlayerBullet
{
    Boomerang m_boomerang;

    Rigidbody2D rigid;

    Transform reloadTransform;

    Vector2 shootDirection;

    int skillLevel;

    bool isReturn = false;

    float returnDistance;
    float distanceToMuzzle;

    float moveForce; //= 11.5f;
    float initialMoveForce;
    float eulerZ;
    float spineSpeed = 650f;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();

        m_boomerang = GameObject.FindObjectOfType<Boomerang>();
    }
    public void UpdateSkillLevel(int _level)
    {
        skillLevel = _level;

        if (skillLevel == 0 || skillLevel == 1 || skillLevel == 2)
            returnDistance = 2.5f;
        else if (skillLevel == 3)
            returnDistance = 2.75f;
        else if (skillLevel == 4)
            returnDistance = 2.85f;
    }
    public void UpdateSkillInfo(float _moveForce)
    {
        distanceToMuzzle = 0f;

        isReturn = false;

        initialMoveForce = _moveForce;

        moveForce = _moveForce;
    }


    public void Fire(Vector3 _direct)
    {
        shootDirection = _direct;
    }

    private void Update()
    {
        eulerZ += Time.deltaTime * spineSpeed;
        transform.localRotation = Quaternion.Euler(0, 0, eulerZ);

        if (shootDirection == null)
            return;

        distanceToMuzzle = (reloadTransform.transform.position - transform.position).magnitude;

        rigid.AddForce(shootDirection * moveForce, ForceMode2D.Force);

        if (!isReturn)
        {
            if (distanceToMuzzle > returnDistance)
            {
                isReturn = true;

                rigid.velocity = Vector3.zero;
                shootDirection *= -1f;
                moveForce *= 0.2f;
            }
        }
        else if (isReturn)
        {
            moveForce += (initialMoveForce * 0.005f);

            if (distanceToMuzzle >= 7)
            {
                m_boomerang.ExpiredBoomerang(this,false);
            }
        }
    }

    public void SetMuzzleTransform(Transform _reloadTransform)
    {
        if (_reloadTransform)
        {
            rigid.velocity = Vector2.zero;

            transform.localRotation = Quaternion.identity;

            reloadTransform = _reloadTransform;

            transform.position = reloadTransform.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isReturn)
        {
            if (collision.gameObject.CompareTag("Player"))
                m_boomerang.ExpiredBoomerang(this, true);
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyStat _enemyStat = collision.gameObject.GetComponent<EnemyStat>();
            if (_enemyStat && !_enemyStat.isDeath)
            {
                GameObject _damage = SkillDamagePooler.Instance.GetPlayerSkill(SkillType.SKILL_BOOMERANG);

                if (_damage != null)
                {
                    _damage.GetComponent<DamageBoomerang>().GetSkillLevel(skillLevel);
                    _damage.transform.position = collision.transform.position;
                    _damage.SetActive(true);
                }
            }
        }

        if (collision.gameObject.CompareTag("Boss"))
        {
            BossStat _enemyStat = collision.gameObject.GetComponent<BossStat>();

            if (_enemyStat && !_enemyStat.isDeath)
            {
                GameObject _damage = SkillDamagePooler.Instance.GetPlayerSkill(SkillType.SKILL_BOOMERANG);

                if (_damage != null)
                {
                    _damage.GetComponent<DamageBoomerang>().GetSkillLevel(skillLevel);
                    _damage.transform.position = collision.transform.position;
                    _damage.SetActive(true);
                }
            }
        }

        if (collision.gameObject.CompareTag("InfiniteHPObject"))
        {
            InfiniteStat _objectStat = collision.gameObject.GetComponent<InfiniteStat>();
            if (_objectStat)
            {
                GameObject _damage = SkillDamagePooler.Instance.GetPlayerSkill(SkillType.SKILL_BOOMERANG);

                if (_damage != null)
                {
                    _damage.GetComponent<DamageBoomerang>().GetSkillLevel(skillLevel);
                    _damage.transform.position = collision.transform.position;
                    _damage.SetActive(true);
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
