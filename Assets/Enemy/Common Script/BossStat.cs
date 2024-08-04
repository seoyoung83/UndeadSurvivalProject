using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStat : MonoBehaviour
{
    BossManager m_bossManager;

    public BossType bossType { get; set; }

    public float maxHp { get; set; }

    [SerializeField] float currentHp;

    [HideInInspector] public bool isDeath = false;
    float chekcingTimer = 0;

    private void Awake()
    {
        m_bossManager = GameObject.FindObjectOfType<BossManager>();
    }

    private void OnEnable()
    {
        currentHp = maxHp;

        isDeath = false;
    }

    public void AddDamage(float _damage)
    {
        currentHp -= _damage;

        if (currentHp <= 0f)
        {
            if (!isDeath)
            {
                isDeath = true;

                if (this.gameObject.activeInHierarchy)
                    GetComponent<Boss>().BossDeath();

                chekcingTimer = 0;                
            }
        }
        else
        {
            StartCoroutine(StartDamageEffect());
        }

        StageUIManager.Instance.UpdateBossHealthBar(currentHp, maxHp);
    }


    private void Update()
    {
        if (!isDeath)
            return;

        chekcingTimer += Time.deltaTime;

        if (chekcingTimer > 1f)
        {
            chekcingTimer = 0;
            m_bossManager.DespawnBoss(gameObject);
        }
    }

    IEnumerator StartDamageEffect()
    {
        float pingpongCheckingTime = 0;

        int count = 0;
        while (count < 2)
        {
            //PingPong Alhpa
            pingpongCheckingTime += Time.deltaTime * 1.8f;

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
}
