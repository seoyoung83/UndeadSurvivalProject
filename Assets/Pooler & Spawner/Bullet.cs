using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

}

public interface IEnemyBullet
{
    float damageValue { get; }

    void Fire(Vector3 shootVect);
    void SetMuzzleTransform(Transform muzzleTransform);

}

public interface IPlayerBullet
{
    void UpdateSkillLevel(int level);

    void Fire(Vector3 shootVect);

    void SetMuzzleTransform(Transform muzzleTransform);

}
