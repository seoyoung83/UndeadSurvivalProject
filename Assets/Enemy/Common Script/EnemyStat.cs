using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStat : MonoBehaviour
{
    public EnemyType enemyType { get; set; }

    public float maxHp { get; set; }

    [SerializeField] float currentHp;

    [HideInInspector] public bool isDeath = false;
    float chekcingTimer = 0;

    private void OnEnable()
    {
        //Reset
        isDeath = false;

        currentHp = maxHp;

        GetComponent<Collider2D>().enabled = true;
    }

    public void AddDamage(float _damage)
    {
        currentHp -= _damage;

        if (currentHp <= 0f)
        {
            GetComponent<Collider2D>().enabled = false;

            GetComponent<Enemy>().EnemyDeath();

            chekcingTimer = 0;
            isDeath = true;
        }
        else
        {
            StartCoroutine(StartDamageEffect());
        }

        GameObject _damageTextFx = EffectPooler.Instance.GetEffect(EffectType.EFFECTTYPE_DAMAGETEXT);
        if (_damageTextFx != null)
        {
            _damageTextFx.SetActive(true);
            _damageTextFx.GetComponent<WarldspaceDamageTextUI>().DisplayDamageTextEffect(transform, _damage);
        }
    }

    private void Update()
    {
        if (!isDeath)
            return;
        chekcingTimer += Time.deltaTime;

        if (chekcingTimer > 1f)
        {
            chekcingTimer = 0;
            EnemySpawner.Instance.DespawnEnemy(this.gameObject);
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
