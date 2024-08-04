using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeightedBoxPickup
{
    public PickupType pickupType;
    public int weight;
}

[System.Serializable]
public class WeightedSkillFuelPickup
{
    public PickupType pickupType;
    public int[] weight;
}

public class PickupManager : MonoBehaviour
{
    Transform targetTransform;

    [SerializeField] Vector3[] initialSkillFuelSpawnVect;

    bool isRush = false;

    float expireTimer = 0;
    float spawnInterval;

    [SerializeField] List<WeightedBoxPickup> weightedPickups; //�Ⱦ� �ڽ��� �����ϴ� �����۵�

    [SerializeField] List<WeightedSkillFuelPickup> weightedSkillFuelPickups;

    private void Awake()
    {
        targetTransform = GameObject.FindObjectOfType<PlayerMove>().transform;
    }

    private void Update()
    {
        if (StageManager.playState == PlayState.NormalBattle)
        {
            if (!isRush)
                return;

            expireTimer += Time.deltaTime;

            if (expireTimer > spawnInterval)
            {
                expireTimer = 0;

                SpawnBoxPickupItem();
            }
        }
    }

    void SpawnBoxPickupItem() 
    {
        float guidAxisX = 3;
        float guidAxisY = 5;

        PickupType selectedPickupBoxType = (PickupType)WeightedRandomPickup.GetWeightedRandomBoxPickup(weightedPickups);

        int positionNumber = Random.Range(0, 4);

        Vector3 spawnVect;

        if (positionNumber == 0) //��
            spawnVect = new Vector3(targetTransform.position.x + guidAxisX, targetTransform.position.y + Random.Range(-guidAxisY, guidAxisY), 0);
        else if (positionNumber == 1) //��
            spawnVect = new Vector3(targetTransform.position.x - guidAxisX, targetTransform.position.y + Random.Range(-guidAxisY, guidAxisY), 0);
        else if (positionNumber == 2) //��
            spawnVect = new Vector3(targetTransform.position.x + Random.Range(-guidAxisX, guidAxisX), targetTransform.position.y - guidAxisY, 0);
        else if (positionNumber == 2) //��
            spawnVect = new Vector3(targetTransform.position.x + Random.Range(-guidAxisX, guidAxisX), targetTransform.position.y + guidAxisY, 0);
        else
            spawnVect = Vector3.zero;

        AddSpawnData(true, selectedPickupBoxType, spawnVect);
    }

    public void SpawnRandomSkillFuel(Vector3 _spawnVect)
    {
        PickupType selectedSkillFuelPickupType = (PickupType)WeightedRandomPickup.GetWeightedRandomSkillFuelPickup(weightedSkillFuelPickups);

        if (selectedSkillFuelPickupType != PickupType.PICKUPTYPE_MAX_SIZE) // PICKUPTYPE_MAX_SIZE�� ���� ���� �ʴ� Ȯ�� �߰� ��Ŵ
            AddSpawnData(false, selectedSkillFuelPickupType, _spawnVect);
        else
            return;
    }

    //�ʱ� ���� ���� �ٷ���(���������� Pickup + skillFuel)
    public void InitialSupportSpawn()
    {
        AddSpawnData(false, PickupType.LUCKYBOX_COMBATASSISTANCE, Vector3.up * 3);

        for (int i=0; i< initialSkillFuelSpawnVect.Length; ++i)
            AddSpawnData(false, PickupType.SKILLFUEL_GREEN_SMALL, initialSkillFuelSpawnVect[i]);
    }

    void AddSpawnData(bool _isPickupBox, PickupType _typ, Vector3 _position)
    {
        PickupDataToSpawn data;
        data.isPickupBox = _isPickupBox;
        data.Type = _typ;
        data.position = _position;
        PickupSpawner.AddPickupToSpawn(data);
    }

    public void GetStageRoundIndex(int _index)
    {
        switch (_index)
        {
            case 0:
                spawnInterval = 50f;
                break; 
            case 1:
                spawnInterval = 40f;
                break;
            case 2:
                spawnInterval = 30f;
                break;
        }
    }

    //���� start & stop 
    public void CommandRush(bool _isRush = true) 
    {
        isRush = _isRush;
    }
}
