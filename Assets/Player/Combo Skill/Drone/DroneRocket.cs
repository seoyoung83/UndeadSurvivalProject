using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneRocket : MonoBehaviour, IPlayerBullet
{
    GameObject trailRenderer;

    SkillType m_skillType;

    Vector3 targetVect;

    int skillLevel;

    bool isReached = false;

    float rocketExplosionSize; //0.6f

    float buffValue_AttackRang;

    private void Awake()
    {
        rocketExplosionSize = transform.localScale.x;

        trailRenderer = transform.GetChild(0).gameObject;
    }

    public void SetShootTransform(Transform _muzzleTransform, Vector3 _targetVect)
    {
        isReached = false;

        if (_muzzleTransform)
        {
            transform.position = _muzzleTransform.position;
           
        }

        trailRenderer.SetActive(true);
    }

    public void UpdateSkillLevel(int _level)
    {
        skillLevel = _level;
    }

    public void UpdateSkillInfo(SkillType _skillType, float _buffValue_AttackRang)
    {
        m_skillType = _skillType;

        buffValue_AttackRang = _buffValue_AttackRang;
    }

    public void Fire(Vector3 _targetVect)
    {
        targetVect = _targetVect;
    }

    private void Update()
    {
        if (!isReached)
            ShootToTargeting(targetVect);
    }

    void ShootToTargeting(Vector3 _targetVect)
    {
        Vector2 directVect = (_targetVect - transform.position).normalized;
        float radValue = Mathf.Atan2(directVect.y, directVect.x);
        float shootAngle = radValue * (180 / Mathf.PI);

        transform.localRotation = Quaternion.Euler(0, 0, shootAngle);

        transform.position = Vector3.Lerp(transform.position, _targetVect, 0.15f);

        float distanceToTarget = (_targetVect - transform.position).magnitude;

        if (distanceToTarget <= 0.05f)
        {
            isReached = true;

            Fire_RocketExplosion();

            trailRenderer.SetActive(false);
            gameObject.SetActive(false);
        }
    }

    void Fire_RocketExplosion()
    {
        GameObject _explosionFx = EffectPooler.Instance.GetEffect(EffectType.EFFECTTYPE_DRONE_ROCKET_EXPLOSION);
        if (_explosionFx != null)
        {
            float _attackRang = rocketExplosionSize + (rocketExplosionSize * buffValue_AttackRang / 100);

            _explosionFx.transform.position = targetVect;
            _explosionFx.transform.localScale = new Vector3(_attackRang, _attackRang, _attackRang);
            _explosionFx.GetComponent<DroneRocketExplosion>().GetInfo(m_skillType, skillLevel);
            _explosionFx.SetActive(true);
        }
    }

    public void SetMuzzleTransform(Transform muzzleTransform)
    {
        isReached = false;

        if (muzzleTransform)
            transform.position = muzzleTransform.position;

        trailRenderer.SetActive(true);

    }
}
