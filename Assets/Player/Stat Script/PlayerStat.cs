using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    //Hp
    const float defaultMaxHpValue = 4000f;
    public static float BuffMaxHp = defaultMaxHpValue;

    public static float m_currentHp;

    //공격력
    const float defaultOffenseValue = 935f; //535
    public static float BuffOffense = defaultOffenseValue;

    //스피드
    const float defaultSpeedValue = 2f;
    public static float BuffSpeed = defaultSpeedValue;

    //방어력
    const float defaultDefenceValue = 0f;
    float BuffDefence = defaultDefenceValue;

    bool doDefend = false;

    //Margnet
    [SerializeField] CircleCollider2D margnetArea;
    const float defaultMagnetAreaRadius = 0.4f;
    float BuffMagnetArea = defaultMagnetAreaRadius;

    //Healing
    float BuffSelfHealing = 0;

    float checkingTimer = 0f;
    float intervalSelfHeal = 5f;

    private void OnEnable()
    {
        m_currentHp = BuffMaxHp;

        margnetArea.radius = BuffMagnetArea;
    }

    private void Update()
    {
        if (BuffSelfHealing == 0)
            return;
         
        DoContinuousHeal();
    }

    public void AddDamage(float _damage)
    {
        float _buffDefence = _damage - (_damage * BuffDefence / 100);

        if (doDefend == true)
        {
            _buffDefence = _buffDefence / 70;
        }

        if (m_currentHp > 0)
        {
            m_currentHp -= _buffDefence;

            StartCoroutine(StartDamageEffect());
        }
        else if (m_currentHp <= 0)
        {
            //********죽었을때
            //StageManager. GameFail() 
        }
    }

    IEnumerator StartDamageEffect()
    {
        float pingpongCheckingTime = 0;

        int count = 0;
        while (count < 2)
        {
            //PingPong Alhpa
            pingpongCheckingTime += Time.deltaTime * 3f;

            if (pingpongCheckingTime > 1)
            {
                pingpongCheckingTime = 0;
                count++;
            }
            float _value = Mathf.PingPong(pingpongCheckingTime, 1);

            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, _value);

            yield return null;
        }

        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
    }

    public void DoImmediateHeal(float _heal) //고기 아이템
    {
        m_currentHp += _heal;

        if (m_currentHp > BuffMaxHp)
        {
            m_currentHp = BuffMaxHp;
        }
    }

    void DoContinuousHeal()
    {
        checkingTimer += Time.deltaTime;

        if (checkingTimer > intervalSelfHeal)
        {
            checkingTimer = 0f;

            m_currentHp += (BuffMaxHp * BuffSelfHealing / 100);

            if (m_currentHp > BuffMaxHp)
                m_currentHp = BuffMaxHp;
        }
    }

    public void DoDefend(bool _defend) //실드
    {
        doDefend = _defend; //80방어
    }


    public void DoPassiveBuff(PlayerSkillCategory _type, float _value) 
    {
        switch (_type)
        {
            case PlayerSkillCategory.PASSIVE_HI_POWEREDBULLET: //공격력 +N%
                BuffOffense = defaultOffenseValue + (defaultOffenseValue * _value / 100);
                break;
            case PlayerSkillCategory.PASSIVE_IRON_ARMOR: //받는 피해량 감소 -N%
                BuffDefence = _value;
                break;
            case PlayerSkillCategory.PASSIVE_ENERGYDRINK: // 5초마다 Hp N%회복 
                BuffSelfHealing = _value;
                break;
            case PlayerSkillCategory.PASSIVE_SNEAKERS: //이동 속도 +N%
                BuffSpeed = defaultSpeedValue + (defaultSpeedValue * _value / 100);
                break;
            case PlayerSkillCategory.PASSIVE_HIPOWERMAGNET: //아이템획득범위 +N%
                BuffMagnetArea = defaultMagnetAreaRadius + (defaultMagnetAreaRadius * _value / 100);
                margnetArea.radius = BuffMagnetArea;
                break;
            case PlayerSkillCategory.PASSIVE_FITNESSGUIDE: //최대 Hp +N%
                BuffMaxHp = defaultMaxHpValue + (defaultMaxHpValue * _value / 100);
                break;

        }
    }

    public void DoMagnet()
    {
        Queue<GameObject> objectsQueue = PickupSpawner.Instance.FindOutPickupSkillFuelAndActiveMagnet();

        while (objectsQueue.Count > 0)
        {
            GameObject oneObject = objectsQueue.Dequeue();
            StartCoroutine(ActivePickupMagnetic(oneObject));
        }   
    }

    IEnumerator ActivePickupMagnetic(GameObject _fuelObject)
    {
        Rigidbody2D _fuelRigid = _fuelObject.GetComponent<Rigidbody2D>();

        Vector3 _bounceDirect = (_fuelObject.transform.position - transform.position).normalized;
        float _time = 0;

        while (_fuelObject.activeInHierarchy)
        {
            if(Time.timeScale == 0)
                yield return null;

            while (_time < 0.6f)
            {
                _time += Time.deltaTime;

                _fuelRigid.AddForce(_bounceDirect * 3f, ForceMode2D.Force);

                yield return null;
            }

            if (_time >= 0.6)
                _fuelRigid.velocity = Vector3.zero;

            _fuelObject.transform.position = Vector3.MoveTowards(_fuelObject.transform.position, transform.position, 15f * Time.deltaTime);

            yield return null;
        }
    }
}
