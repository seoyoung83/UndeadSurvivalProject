using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PickupDataToSpawn
{
    public bool isPickupBox;
    public PickupType Type;
    public Vector3 position;
}

public struct PickupDataToDespawn
{
    public bool isPickupBox;
    public PickupType Type;
    public GameObject pickupObject;
}

public class PickupSpawner : MonoBehaviour
{
    public static PickupSpawner Instance;

    [SerializeField] bool isGettingData = false;

    [Header("Spawn PickupBox Count")]
    [SerializeField] int numberOfPickupBox = 0;

    [Header("Spawn Pickup Count")]
    [SerializeField] int numberOfPickup = 0;
    [SerializeField] int[] numberOfEachPickup = new int[(int)PickupType.PICKUPTYPE_MAX_SIZE];

    static Queue<PickupDataToSpawn>[] pickupSpawnDataQueue = new Queue<PickupDataToSpawn>[(int)PickupType.PICKUPTYPE_MAX_SIZE];
    static Queue<PickupDataToDespawn>[] pickupDespawnDataQueue = new Queue<PickupDataToDespawn>[(int)PickupType.PICKUPTYPE_MAX_SIZE];

    private void Awake()
    {
        Instance = this;

        for (int i = 0; i < (int)PickupType.PICKUPTYPE_MAX_SIZE; ++i)
        {
            pickupSpawnDataQueue[i] = new Queue<PickupDataToSpawn>();
            pickupDespawnDataQueue[i] = new Queue<PickupDataToDespawn>();
        }
    }

    private void Update()
    {
        if (isGettingData)
            return;
        
        for (int i = 0; i < (int)PickupType.PICKUPTYPE_MAX_SIZE; ++i)
        {
            if (pickupSpawnDataQueue[i].Count > 0)
            {
                PickupDataToSpawn data = pickupSpawnDataQueue[i].Dequeue();

                SpawnPickup(data, data.isPickupBox);
            }

            if (pickupDespawnDataQueue[i].Count > 0)
            {
                PickupDataToDespawn data = pickupDespawnDataQueue[i].Dequeue();

                DespawnPickup(data, data.isPickupBox);
            }
        }
    }

    static public void AddPickupToSpawn(PickupDataToSpawn _pickupData)
    {
        pickupSpawnDataQueue[(int)_pickupData.Type].Enqueue(_pickupData);
    }

    static public void AddPickupToDespawn(PickupDataToDespawn _pickupData)
    {
        pickupDespawnDataQueue[(int)_pickupData.Type].Enqueue(_pickupData);
    }

    //픽업박스 open , 보스 아이템 스폰 : 즉시 스폰
    public void SpawnPickup(PickupDataToSpawn data, bool isPickupBox)
    {
        GameObject pickup = isPickupBox ? PickupPooler.Instance.GetPickupBox(data.Type) : PickupPooler.Instance.GetPickup(data.Type);

        if (pickup)
        {
            pickup.transform.position = data.position;
            pickup.SetActive(true);

            if (isPickupBox)
            {
                ++numberOfPickupBox;
            }
            else
            {
                ++numberOfPickup;
                ++numberOfEachPickup[(int)data.Type];
            }
        }
    }

    //픽업박스 open : 즉시 스폰
    public void DespawnPickup(PickupDataToDespawn data, bool isPickupBox)
    {
        if (data.pickupObject)
        {
            if (isPickupBox)
            {
                if (numberOfPickupBox > 0)
                    --numberOfPickupBox;

                //바로 스폰
                PickupDataToSpawn addData;
                addData.isPickupBox = false;
                addData.Type = data.Type;
                addData.position = data.pickupObject.transform.position;
                SpawnPickup(addData, false);
            }
            else
            {
               // AudioManager.Instance.GetPickupAudioEvent();

                if (numberOfEachPickup[(int)data.Type] > 0)
                {
                    --numberOfPickup;
                    --numberOfEachPickup[(int)data.Type];
                }
            }

            data.pickupObject.SetActive(false);
        }
    }

    void AddOrientationIcon(int _pickupBoxType, GameObject _pickupBoxObject)
    {
        if(_pickupBoxType == (int)PickupType.LUCKYBOX_SKILLUPGRADE)
            StageUIManager.Instance.AddOrientationIconUI(false, _pickupBoxObject);
    }

    //Pickup Bomb 기능_필드상의 모든 Pickup Box 오픈
    public void DespawnAllOfPickupBox()
    {
        if (numberOfPickupBox < 1)
            return;

        StartCoroutine(StartOpenAllOfPickupBox());
    }

    IEnumerator StartOpenAllOfPickupBox()
    {
        isGettingData = true;

        Queue<GameObject> objectsQueue = new Queue<GameObject>();

        int count = 0;
        while (count < numberOfPickupBox)
        {
            GameObject _activeBox = PickupPooler.Instance.GetPickupBoxData();
            objectsQueue.Enqueue(_activeBox);
            count++;
            yield return null;
        }

        isGettingData = false;

        while (objectsQueue.Count > 0)
        {
            GameObject box = objectsQueue.Dequeue();
            box.GetComponent<PickupBox>().SetBoxState(false);
            yield return null;
        }
        
    }

    //Pickup Magnet 기능_필드상의 모든 skill Fuel 데이터 넘겨주기
    public Queue<GameObject> FindOutPickupSkillFuelAndActiveMagnet()
    {
        isGettingData = true;

        Queue<GameObject> objectsQueue = new Queue<GameObject>();

        GameObject _pickup;
        for (int i = (int)PickupType.SKILLFUEL_GREEN_SMALL; i < (int)PickupType.SKILLFUEL_GOLD + 1; ++i)
        {
            for (int j = 0; j < numberOfEachPickup[i]; ++j)
            {
                _pickup = PickupPooler.Instance.GetPickupData(i, j);

                if (_pickup != null && _pickup.activeInHierarchy)
                    objectsQueue.Enqueue(_pickup);
            }
        }

        isGettingData = false;

        return objectsQueue;
    }


    //모든 픽업 아이템 초기화
    void Reset() 
    {
        numberOfPickup = 0;
        numberOfPickupBox = 0;

        for (int i = 0; i < (int)PickupType.PICKUPTYPE_MAX_SIZE; ++i)
            numberOfEachPickup[i] = 0;

        PickupPooler.Instance.InactiveAll();
    }

}
