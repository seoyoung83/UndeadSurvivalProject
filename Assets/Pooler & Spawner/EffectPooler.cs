using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectType
{
    //Player_Comboskill
    EFFECTTYPE_ROCKET_EXPLOSION,
    EFFECTTYPE_SHARK_ROCKET_EXPLOSION,
    EFFECTTYPE_DRONE_ROCKET_EXPLOSION,
    EFFECTTYPE_DRONE_ROCKET_TARGETING,
    //Boss
    EFFECTTYPE_DANGEROUSGROUND,
    EFFECTTYPE_PRESSEDCIRCLE,
    //else
    EFFECTTYPE_DAMAGETEXT,
    EFFECTTYPE_MAX_SIZE,
}

public class EffectPooler : MonoBehaviour
{
    public static EffectPooler Instance;

    [Header("Player Effect")]
    [SerializeField] GameObject rocketExposionFx;
    [SerializeField] int countRocketExposionPool;
    [SerializeField] GameObject sharkRocketExposionFx; 
    [SerializeField] int countSharkRocketExposionPool; 
    [SerializeField] GameObject droneAttackFx;
    [SerializeField] int countDroneAttackPool;
    [SerializeField] GameObject droneTargetingFx;
    [SerializeField] int countDroneTargetingPool;

    [Header("Boss Effect")]
    [SerializeField] GameObject dangerousGroudFx; //Boss Bummy 뛰는 공격에 사용
    [SerializeField] int countDangerousGroudPool;
    [SerializeField] GameObject pressedCircleFx; //닌자 허수아비 생성시 사용중 ***더 생기면 이름 바꾸자!
    [SerializeField] int countPressedCirclePool; 

    [Header("Something Effect")]
    [SerializeField] GameObject damageTextFx; 
    [SerializeField] int countDamageTextPool;

    List<GameObject>[] effectPool = new List<GameObject>[(int)EffectType.EFFECTTYPE_MAX_SIZE];

    private void Awake()
    {
        Instance = this;

        //Player 
        effectPool[(int)EffectType.EFFECTTYPE_ROCKET_EXPLOSION] = new List<GameObject>();

        for (int i = 0; i < countRocketExposionPool; ++i)
        {
            GameObject _fx = (GameObject)Instantiate(rocketExposionFx);
            _fx.SetActive(false);
            _fx.transform.SetParent(transform);
            effectPool[(int)EffectType.EFFECTTYPE_ROCKET_EXPLOSION].Add(_fx);
        }

        effectPool[(int)EffectType.EFFECTTYPE_SHARK_ROCKET_EXPLOSION] = new List<GameObject>();

        for (int i = 0; i < countSharkRocketExposionPool; ++i)
        {
            GameObject _fx = (GameObject)Instantiate(sharkRocketExposionFx);
            _fx.SetActive(false);
            _fx.transform.SetParent(transform);
            effectPool[(int)EffectType.EFFECTTYPE_SHARK_ROCKET_EXPLOSION].Add(_fx);
        } 

        effectPool[(int)EffectType.EFFECTTYPE_DRONE_ROCKET_EXPLOSION] = new List<GameObject>();

        for (int i = 0; i < countDroneAttackPool; ++i)
        {
            GameObject _fx = (GameObject)Instantiate(droneAttackFx);
            _fx.SetActive(false);
            _fx.transform.SetParent(transform);
            effectPool[(int)EffectType.EFFECTTYPE_DRONE_ROCKET_EXPLOSION].Add(_fx);
        }   

        effectPool[(int)EffectType.EFFECTTYPE_DRONE_ROCKET_TARGETING] = new List<GameObject>();

        for (int i = 0; i < countDroneTargetingPool; ++i)
        {
            GameObject _fx = (GameObject)Instantiate(droneTargetingFx);
            _fx.SetActive(false);
            _fx.transform.SetParent(transform);
            effectPool[(int)EffectType.EFFECTTYPE_DRONE_ROCKET_TARGETING].Add(_fx);
        }

        //Boss
        effectPool[(int)EffectType.EFFECTTYPE_DANGEROUSGROUND] = new List<GameObject>();

        for (int i = 0; i < countDangerousGroudPool; ++i)
        {
            GameObject _fx = (GameObject)Instantiate(dangerousGroudFx);
            _fx.SetActive(false);
            _fx.transform.SetParent(transform);
            effectPool[(int)EffectType.EFFECTTYPE_DANGEROUSGROUND].Add(_fx);
        }

        effectPool[(int)EffectType.EFFECTTYPE_PRESSEDCIRCLE] = new List<GameObject>();

        for (int i = 0; i < countPressedCirclePool; ++i)
        {
            GameObject _fx = (GameObject)Instantiate(pressedCircleFx);
            _fx.SetActive(false);
            _fx.transform.SetParent(transform);
            effectPool[(int)EffectType.EFFECTTYPE_PRESSEDCIRCLE].Add(_fx);
        }

        //else
        effectPool[(int)EffectType.EFFECTTYPE_DAMAGETEXT] = new List<GameObject>();

        for (int i = 0; i < countDamageTextPool; ++i)
        {
            GameObject _fx = (GameObject)Instantiate(damageTextFx);
            _fx.SetActive(false);
            _fx.transform.SetParent(transform);
            effectPool[(int)EffectType.EFFECTTYPE_DAMAGETEXT].Add(_fx);
        }
    }

    public GameObject GetEffect(EffectType _effectType) 
    {
        GameObject returnedGameObject = null;

        if (effectPool[(int)_effectType] == null)
            return null;

        for (int i = 0; i < effectPool[(int)_effectType].Count; ++i)
        {
            if (effectPool[(int)_effectType][i].activeInHierarchy == false)
            {
                returnedGameObject =  effectPool[(int)_effectType][i];
                return returnedGameObject;
            }
        }

        if (returnedGameObject == null)
        {
            return CreatEffect(_effectType);
        }

        return null;
    }

    GameObject CreatEffect(EffectType _effectType)
    {
        GameObject _creatEffect = null;
        switch (_effectType)
        {
            case EffectType.EFFECTTYPE_ROCKET_EXPLOSION:
                _creatEffect = (GameObject)Instantiate(rocketExposionFx);
                break;
            case EffectType.EFFECTTYPE_SHARK_ROCKET_EXPLOSION:
                _creatEffect = (GameObject)Instantiate(sharkRocketExposionFx);
                break;
            case EffectType.EFFECTTYPE_DRONE_ROCKET_EXPLOSION: 
                _creatEffect = (GameObject)Instantiate(droneAttackFx);
                break;
            case EffectType.EFFECTTYPE_DRONE_ROCKET_TARGETING:
                _creatEffect = (GameObject)Instantiate(droneTargetingFx);
                break;
            case EffectType.EFFECTTYPE_DANGEROUSGROUND:
                _creatEffect = (GameObject)Instantiate(dangerousGroudFx);
                break;
            case EffectType.EFFECTTYPE_PRESSEDCIRCLE:
                _creatEffect = (GameObject)Instantiate(pressedCircleFx);
                break;
            case EffectType.EFFECTTYPE_DAMAGETEXT:
                _creatEffect = (GameObject)Instantiate(damageTextFx);
                break;
        }

        if (_creatEffect != null)
        {
            _creatEffect.SetActive(false);
            _creatEffect.transform.SetParent(transform);
            effectPool[(int)_effectType].Add(_creatEffect);
        }

        for (int i = 0; i < effectPool[(int)_effectType].Count; ++i)
        {
            if (!effectPool[(int)_effectType][i].activeInHierarchy)
            {
                return effectPool[(int)_effectType][i];
            }
        }

        return null;
    }
}
