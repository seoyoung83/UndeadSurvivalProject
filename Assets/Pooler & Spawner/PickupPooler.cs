using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PickupType
{
    BOMB,
    MEAT,
    MAGNET,
    ONECOIN,
    THREECOIN,
    COINSPOCKET,
    SKILLFUEL_GREEN_SMALL,
    SKILLFUEL_GREEN,
    SKILLFUEL_BLUE,
    SKILLFUEL_GOLD,
    LUCKYBOX_COMBATASSISTANCE,
    LUCKYBOX_SKILLUPGRADE,
    PICKUPTYPE_MAX_SIZE,
}

public class PickupPooler : MonoBehaviour
{
    public static PickupPooler Instance;

    [Header("Packaging Pickup")]
    [SerializeField] GameObject pickupBox;
    [SerializeField] int countPickupBoxPool;
    
    [Header("Pickup Bomb")]
    [SerializeField] GameObject pickupBomb;
    [SerializeField] int countPickupBombPool;

    [Header("Pickup Health Buff")]
    [SerializeField] GameObject pickupMeat;
    [SerializeField] int countPickupMeatPool;

    [Header("Pickup Magnet")]
    [SerializeField] GameObject pickupMagnet;
    [SerializeField] int countPickupMagnetPool;

    [Header("Pickup One Coin")]
    [SerializeField] GameObject pickupOneCoin;
    [SerializeField] int countPickupOneCoinPool;

    [Header("Pickup Three Coin")]
    [SerializeField] GameObject pickupThreeCoin;
    [SerializeField] int countPickupThreeCoinPool;

    [Header("Pickup Coins Pocket")]
    [SerializeField] GameObject pickupCoinsPocket;
    [SerializeField] int countPickupCoinsPocketPool;

    [Header("Pickup Skill Fuel_Green_Small ")]
    [SerializeField] GameObject pickupFuelGreenSmall;
    [SerializeField] int countPickupFuelGreenSmallPool;

    [Header("Pickup Skill Fuel_Green ")]
    [SerializeField] GameObject pickupFuelGreen;
    [SerializeField] int countPickupFuelGreenPool;

    [Header("Pickup Skill Fuel_Blue ")]
    [SerializeField] GameObject pickupFuelBlue;
    [SerializeField] int countPickupFuelBluePool;

    [Header("Pickup Skill Fuel_Gold")]
    [SerializeField] GameObject pickupFuelGold;
    [SerializeField] int countPickupFuelGoldPool;

    [Header("Pickup Lucky Box_Combat Assistance")]
    [SerializeField] GameObject pickupCombatAssistanceLuckyBox; 
    [SerializeField] int countPickupCombatAssistanceLuckyBoxPool; 

    [Header("Pickup Lucky Box_Skill Upgrade")]
    [SerializeField] GameObject pickupSkillLuckyBox; 
    [SerializeField] int countPickupSkillLuckyBoxPool;

    List<GameObject> pickupBoxPool = new List<GameObject>();
    List<GameObject>[] pickupPool= new List<GameObject>[(int)PickupType.PICKUPTYPE_MAX_SIZE];


    private void Awake()
    {
        Instance = this;

        pickupBoxPool = new List<GameObject>();
        for (int i = 0; i < countPickupBoxPool; ++i)
        {
            GameObject _pickupBox = (GameObject)Instantiate(pickupBox);
            _pickupBox.SetActive(false);
            _pickupBox.transform.SetParent(transform);
            pickupBoxPool.Add(_pickupBox);
        }


        pickupPool[(int)PickupType.BOMB] = new List<GameObject>();
        for (int i = 0; i < countPickupBombPool; ++i)
        {
            GameObject _pickup = (GameObject)Instantiate(pickupBomb);
            _pickup.SetActive(false);
            _pickup.transform.SetParent(transform);
            pickupPool[(int)PickupType.BOMB].Add(_pickup);
        }

        pickupPool[(int)PickupType.MEAT] = new List<GameObject>();
        for (int i = 0; i < countPickupMeatPool; ++i)
        {
            GameObject _pickup = (GameObject)Instantiate(pickupMeat);
            _pickup.SetActive(false);
            _pickup.transform.SetParent(transform);
            pickupPool[(int)PickupType.MEAT].Add(_pickup);
        }

        pickupPool[(int)PickupType.MAGNET] = new List<GameObject>();
        for (int i = 0; i < countPickupMagnetPool; ++i)
        {
            GameObject _pickup = (GameObject)Instantiate(pickupMagnet);
            _pickup.SetActive(false);
            _pickup.transform.SetParent(transform);
            pickupPool[(int)PickupType.MAGNET].Add(_pickup);
        }

        pickupPool[(int)PickupType.ONECOIN] = new List<GameObject>();
        for (int i = 0; i < countPickupOneCoinPool; ++i)
        {
            GameObject _pickup = (GameObject)Instantiate(pickupOneCoin);
            _pickup.SetActive(false);
            _pickup.transform.SetParent(transform);
            pickupPool[(int)PickupType.ONECOIN].Add(_pickup);
        }

        pickupPool[(int)PickupType.THREECOIN] = new List<GameObject>();
        for (int i = 0; i < countPickupThreeCoinPool; ++i)
        {
            GameObject _pickup = (GameObject)Instantiate(pickupThreeCoin);
            _pickup.SetActive(false);
            _pickup.transform.SetParent(transform);
            pickupPool[(int)PickupType.THREECOIN].Add(_pickup);
        }

        pickupPool[(int)PickupType.COINSPOCKET] = new List<GameObject>();
        for (int i = 0; i < countPickupCoinsPocketPool; ++i)
        {
            GameObject _pickup = (GameObject)Instantiate(pickupCoinsPocket);
            _pickup.SetActive(false);
            _pickup.transform.SetParent(transform);
            pickupPool[(int)PickupType.COINSPOCKET].Add(_pickup);
        }

        pickupPool[(int)PickupType.SKILLFUEL_GREEN_SMALL] = new List<GameObject>();
        for (int i = 0; i < countPickupFuelGreenSmallPool; ++i)
        {
            GameObject _pickup = (GameObject)Instantiate(pickupFuelGreenSmall);
            _pickup.SetActive(false);
            _pickup.transform.SetParent(transform);
            pickupPool[(int)PickupType.SKILLFUEL_GREEN_SMALL].Add(_pickup);
        }

        pickupPool[(int)PickupType.SKILLFUEL_GREEN] = new List<GameObject>();
        for (int i = 0; i < countPickupFuelGreenPool; ++i)
        {
            GameObject _pickup = (GameObject)Instantiate(pickupFuelGreen);
            _pickup.SetActive(false);
            _pickup.transform.SetParent(transform);
            pickupPool[(int)PickupType.SKILLFUEL_GREEN].Add(_pickup);
        }

        pickupPool[(int)PickupType.SKILLFUEL_BLUE] = new List<GameObject>();
        for (int i = 0; i < countPickupFuelBluePool; ++i)
        {
            GameObject _pickup = (GameObject)Instantiate(pickupFuelBlue);
            _pickup.SetActive(false);
            _pickup.transform.SetParent(transform);
            pickupPool[(int)PickupType.SKILLFUEL_BLUE].Add(_pickup);
        }

        pickupPool[(int)PickupType.SKILLFUEL_GOLD] = new List<GameObject>();
        for (int i = 0; i < countPickupFuelGoldPool; ++i)
        {
            GameObject _pickup = (GameObject)Instantiate(pickupFuelGold);
            _pickup.SetActive(false);
            _pickup.transform.SetParent(transform);
            pickupPool[(int)PickupType.SKILLFUEL_GOLD].Add(_pickup);
        }

        pickupPool[(int)PickupType.LUCKYBOX_COMBATASSISTANCE] = new List<GameObject>();
        for (int i = 0; i < countPickupCombatAssistanceLuckyBoxPool; ++i)
        {
            GameObject _pickup = (GameObject)Instantiate(pickupCombatAssistanceLuckyBox);
            _pickup.SetActive(false);
            _pickup.transform.SetParent(transform);
            pickupPool[(int)PickupType.LUCKYBOX_COMBATASSISTANCE].Add(_pickup);
        }

        pickupPool[(int)PickupType.LUCKYBOX_SKILLUPGRADE] = new List<GameObject>();
        for (int i = 0; i < countPickupSkillLuckyBoxPool; ++i)
        {
            GameObject _pickup = (GameObject)Instantiate(pickupSkillLuckyBox);
            _pickup.SetActive(false);
            _pickup.transform.SetParent(transform);
            pickupPool[(int)PickupType.LUCKYBOX_SKILLUPGRADE].Add(_pickup);
        }
    }

    public GameObject GetPickupBox(PickupType _pickupBoxType)
    {
        GameObject pickup = null;

        if (pickupBoxPool.Count < 1)
            return null;

        for (int i = 0; i < pickupBoxPool.Count; ++i)
        {
            if (!pickupBoxPool[i].activeInHierarchy)
            {
                pickup = pickupBoxPool[i];
                pickup.GetComponent<PickupBox>().m_pickuptype = _pickupBoxType;
                return pickup;  
            }
        }

        if (pickup == null)
            return CreatPickupBox(_pickupBoxType);

        return null;
    }

    public GameObject GetPickup(PickupType _pickupType)
    {
        GameObject pickup = null;

        if (pickupPool[(int)_pickupType] == null)
            return null;

        for (int i = 0; i < pickupPool[(int)_pickupType].Count; ++i)
        {
            if (!pickupPool[(int)_pickupType][i].activeInHierarchy)
            {
                pickup = pickupPool[(int)_pickupType][i];
                return pickup;
            }
        }

        if (pickup == null)
            return CreatPickup(_pickupType);

        return null;
    }


    GameObject CreatPickupBox(PickupType _pickupBoxType)
    {
        GameObject _creatPickupBox = null;

        _creatPickupBox = (GameObject)Instantiate(pickupBox);

        if (_creatPickupBox != null)
        {
            _creatPickupBox.SetActive(false);
            _creatPickupBox.transform.SetParent(transform);
            _creatPickupBox.GetComponent<PickupBox>().m_pickuptype = _pickupBoxType;
            pickupBoxPool.Add(_creatPickupBox);
        }

        for (int i = 0; i < pickupBoxPool.Count; ++i)
        {
            if (!pickupBoxPool[i].activeInHierarchy)
            {
                return pickupBoxPool[i];
            }
        }

        return null;
    }

    GameObject CreatPickup(PickupType _pickupType)
    {
        GameObject _creatPickup = null;

        switch (_pickupType)
        {
            case PickupType.BOMB:
                _creatPickup = (GameObject)Instantiate(pickupBomb);
                break;
            case PickupType.MEAT:
                _creatPickup = (GameObject)Instantiate(pickupMeat);
                break;
            case PickupType.MAGNET:
                _creatPickup = (GameObject)Instantiate(pickupMagnet);
                break;
            case PickupType.ONECOIN:
                _creatPickup = (GameObject)Instantiate(pickupOneCoin);
                break;
            case PickupType.THREECOIN:
                _creatPickup = (GameObject)Instantiate(pickupThreeCoin);
                break;
            case PickupType.COINSPOCKET:
                _creatPickup = (GameObject)Instantiate(pickupCoinsPocket);
                break;
            case PickupType.SKILLFUEL_GREEN_SMALL:
                _creatPickup = (GameObject)Instantiate(pickupFuelGreenSmall);
                break;
            case PickupType.SKILLFUEL_GREEN:
                _creatPickup = (GameObject)Instantiate(pickupFuelGreen);
                break;
            case PickupType.SKILLFUEL_BLUE:
                _creatPickup = (GameObject)Instantiate(pickupFuelBlue);
                break;
            case PickupType.SKILLFUEL_GOLD:
                _creatPickup = (GameObject)Instantiate(pickupFuelGold);
                break;
        }

        if (_creatPickup != null)
        {
            _creatPickup.SetActive(false);
            _creatPickup.transform.SetParent(transform);
            pickupPool[(int)_pickupType].Add(_creatPickup);
        }

        for (int i = 0; i < pickupPool[(int)_pickupType].Count; ++i)
        {
            if (!pickupPool[(int)_pickupType][i].activeInHierarchy)
            {
                return pickupPool[(int)_pickupType][i];
            }
        }
        return null;
    }

    public void InactiveAll()
    {
        for (int i = 0; i < (int)PickupType.PICKUPTYPE_MAX_SIZE; ++i)
        {
            for (int j = 0; j < pickupPool[i].Count; ++j)
            {
                pickupPool[i][j].SetActive(false);
            }
        }

        for (int i = 0; i < pickupBoxPool.Count; ++i)
        {
            pickupBoxPool[i].SetActive(false);
        }
    }

    public void ClearUnusedPickupType()
    {

    }

    public GameObject GetPickupData(int _pickupType, int _index)
    {
        if (pickupPool[_pickupType][_index].activeInHierarchy)
        {
            return pickupPool[_pickupType][_index].gameObject;
        }
        return null;
    }

    public GameObject GetPickupBoxData()
    {
        for (int i = 0; i < pickupBoxPool.Count; ++i)
        {
            if (pickupBoxPool[i].activeInHierarchy)
                return pickupBoxPool[i];
        }

        return null;
    }
}
