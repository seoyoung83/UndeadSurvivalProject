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

                for (int i = 0; i < 2; i++) // ��� & �ڼ�
                    SpawnPickup(PickupType.MEAT + i, _boss.transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0));
            }
            else
            {
                //********************��� ��ȭ ��ũ�Ѱ� ���� ���忡 �ʿ���... 
            }
               
            Destroy(_boss);

            return true;
        }
        return false;
    }

    //���� ���� ����Ʈ 
    public GameObject GetBossSpawnEffect(Vector3 _target) //����Ʈ �ٸ��� �Ϸ��� currentRoundIndex ���� �з�
    {
        waterFenceSpapwnFx.SetActive(true);
        waterFenceSpapwnFx.transform.position = _target;

        return waterFenceSpapwnFx;
    }

    //���� ���� Active
    void GetBossFence(bool _active, Vector3 _target)//��Ÿ�� �ٸ��� �Ϸ��� BossType���� �з�
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
