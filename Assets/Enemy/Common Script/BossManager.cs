using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    StageManager m_stageManager;

    [Header("Boss Fence Fx")]
    [SerializeField] GameObject waterFenceSpapwnFx;

    [Header("Boss Fence")]
    [SerializeField] Transform fencePlatformGrid;
    [SerializeField] GameObject bossWaterFence;

    private void Awake()
    {
        m_stageManager = GameObject.FindObjectOfType<StageManager>();
    }

    public bool SpawnBoss(BossType _bossType, Vector3 _position)
    {
        GetBossFence(true, _position);

        EnemyBulletPooler.Instance.BossBulletInitialized(_bossType);

        GameObject _boss = EnemyPooler.Instance.GetBoss(_bossType);
        if (_boss)
        {
            _boss.transform.position = _position;
            _boss.SetActive(true);
            return true;
        }
        return false;
    }

    public bool DespawnBoss(GameObject _boss)
    {
        GetBossFence(false, Vector3.zero);

        if (_boss)
        {
            _boss.SetActive(false);

            EnemyBulletPooler.Instance.DestroyBossBullet();

            m_stageManager.OnStageRoundCleared();

            if (StageManager.m_currentRound <= 3)
            {
                SpawnPickup(PickupType.LUCKYBOX_SKILLUPGRADE, _boss.transform.position);

                for (int i = 0; i < 2; i++) // 고기 & 자석
                    SpawnPickup(PickupType.MEAT + i, _boss.transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0));
            }
            else
            {
                //********************장비 강화 스크롤과 같이 성장에 필요한... 
            }
               
            Destroy(_boss);

            return true;
        }
        return false;
    }

    //보스 등장 이펙트 
    public GameObject GetBossSpawnEffect(Vector3 _target) //이펙트 다르게 하려면 currentRoundIndex 으로 분류
    {
        waterFenceSpapwnFx.SetActive(true);
        waterFenceSpapwnFx.transform.position = _target;

        return waterFenceSpapwnFx;
    }

    //보스 장판 Active
    void GetBossFence(bool _active, Vector3 _target)//울타리 다르게 하려면 BossType으로 분류
    {
        fencePlatformGrid.gameObject.SetActive(_active);
        fencePlatformGrid.transform.position = _target;

        bossWaterFence.SetActive(_active);
    }


    void SpawnPickup(PickupType _typ, Vector3 _position)
    {
        PickupDataToSpawn data;
        data.isPickupBox = false;
        data.Type = _typ;
        data.position = _position;
        //PickupSpawner.AddPickupToSpawn(data);
        PickupSpawner.Instance.SpawnPickup(data, false);
    }
}
