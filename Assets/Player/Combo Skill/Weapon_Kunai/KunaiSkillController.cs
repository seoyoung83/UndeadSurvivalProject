using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class KunaiSkillController : WeaponSkill
{
    PlayerSkillCategory thisControllerType = PlayerSkillCategory.WEAPON_KUNAI;

    Transform nearestEnemyTransform;

    float attackSpeed;
    float attackRang;

    int numberOfKunaiFiredOnce; //한번 공격에 발사되는 쿠나이 수

    private void Start()
    {
        curruntWeaponTypeNumber = (int)thisControllerType;

        PlayerBulletPooler.Instance.PlayerBulletInitialized(PlayerBulletType.KUNAI_BULLE_NORMAL);
        PlayerBulletPooler.Instance.PlayerBulletInitialized(PlayerBulletType.KUNAI_BULLE_SPECIAL);
    }

    public override void LevelUp(int _currentWeaponSkillLevel)
    {
        skillLevel = _currentWeaponSkillLevel;

        SetData();
    }

    public override void SetData() 
    {
        string index = curruntWeaponTypeNumber + "";

        attackInterval = m_SkillAbilityDataDictionary[index].attackIntervalOfLevel[skillLevel];

        attackSpeed = m_SkillAbilityDataDictionary[index].speedOfLevel[skillLevel];

        attackRang = m_SkillAbilityDataDictionary[index].attackRangOfLevel[skillLevel];

        numberOfKunaiFiredOnce = m_SkillAbilityDataDictionary[index].attackCountOfOneTimeOfLevel[skillLevel];
    }

    public override void SetToReady(bool _isTimeToReady)
    {
        isTimeToReady = _isTimeToReady;
     
        if (isTimeToReady)
        {
            StopAllCoroutines();
        }
        isShooting = false;
    }

    public override void Shoot()
    {
        nearestEnemyTransform = GetNearestEnemyPosition();

        if (nearestEnemyTransform != null)
            StartCoroutine(ShootKunai(nearestEnemyTransform.position));
        else if (nearestEnemyTransform == null)
            isShooting = false;
    }
    
    IEnumerator ShootKunai(Vector3 _shootVect)
    {
        weaponAudio.Play();

        int count = 0;
        while (count < numberOfKunaiFiredOnce)
        {
            Kunai(_shootVect);
            count++;
            yield return new WaitForSeconds(0.1f);
        }
        isShooting = false;
    }

    void Kunai(Vector3 _shootVect)
    {
        float _attackRang = attackRang + (attackRang * buffValue_AttackRang / 100);
        float _bulletSpeed = attackSpeed + (attackSpeed * buffValue_AttackSpeed / 100);
        
        GameObject _bullet;

        if(skillLevel!=5)
            _bullet = PlayerBulletPooler.Instance.GetPlayerBullet(PlayerBulletType.KUNAI_BULLE_NORMAL);
        else
            _bullet = PlayerBulletPooler.Instance.GetPlayerBullet(PlayerBulletType.KUNAI_BULLE_SPECIAL);

        if (_bullet)
        {
            _bullet.gameObject.SetActive(true);
            _bullet.gameObject.transform.localScale = new Vector3(_attackRang, _attackRang, _attackRang);
            _bullet.GetComponent<KunaiBullet>().UpdateSkillLevel(skillLevel);
            _bullet.GetComponent<KunaiBullet>().SetMuzzleTransform(m_weaponsMuzzleTransform.transform);
            _bullet.GetComponent<KunaiBullet>().UpdateSkillInfo(_bulletSpeed);
            _bullet.GetComponent<KunaiBullet>().Fire(_shootVect);
        }
    }

    /*
private void OnDrawGizmos()
{
    Gizmos.color = Color.red;

    Gizmos.DrawWireSphere(new Vector2(transform.position.x, transform.position.y), 6f);
}
*/
    Transform GetNearestEnemyPosition()
    {
        Transform tempNearestEnemyTransform = null;
     
        //Detectting Enemy In Attackable Area
        Collider2D[] enemyInAttackableArea =
        Physics2D.OverlapCircleAll(new Vector2(m_playerMove.transform.position.x, m_playerMove.transform.position.y), 6f, LayerMask.GetMask("Enemy"));

        if (enemyInAttackableArea.Length > 0)
        {
            float shortDistance = Mathf.Infinity;

            foreach (Collider2D enemyCollider in enemyInAttackableArea)
            {
                float distanceFromEnemy = (m_playerMove.transform.position - enemyCollider.gameObject.transform.position).magnitude;

                if (shortDistance > distanceFromEnemy)
                {
                    shortDistance = distanceFromEnemy;
                    tempNearestEnemyTransform = enemyCollider.transform;
                }
            }
        }
        return tempNearestEnemyTransform;
    }
}
