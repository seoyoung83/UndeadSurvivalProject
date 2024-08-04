using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{ 
    public static PlayerUIManager Instance;

    PlayerMove m_playerMove;

    PlayerStat m_playerStat;

    RevolverSkillController m_revolverSkillController;
    int currentWeaponTypeNumber;

    [Header("Player Direction Icon UI")]
    [SerializeField] GameObject playerDirectionIcon;
    [SerializeField] GameObject targetingDirectionIcon_wid; //총 공격 범위 
    [SerializeField] GameObject targetingDirectionIcon_narrow; //총 공격 범위 

    [Header("For Gauge Bar UI")]
    [SerializeField] GameObject forGaugeObject;
    [SerializeField] Image fillmountHealthBar;
    [SerializeField] Image fillmountAttackCooltimeBar;
    float playerAttackCoolTime;
    float maxplayerAttackCoolTime;

    [Header("Revolver Bullet UI")]
    [SerializeField] Transform revolverBulletCountRect; //레볼버 아이콘 UI
    [SerializeField] GameObject[] revolverBulletLayoutPrefab;
    int max_RevolverBulletUICount = 5;
    int bulletPrefabType = 0;
    bool isRevolerBulletUILoading = false;
    Queue<GameObject> revolverBulletUIQueue = new Queue<GameObject>();

    private void Start()
    {
        Instance = this;

        m_playerMove = GetComponentInParent<PlayerMove>();

        m_playerStat = GetComponentInParent<PlayerStat>();

        currentWeaponTypeNumber = PlayerWeaponController.Instance.PlayerWeaponType;

        InitializedPlayerDirectionIcon();
    }


    void InitializedPlayerDirectionIcon()
    {
        switch (currentWeaponTypeNumber)
        {
            case (int)PlayerSkillCategory.TOP_WEAPON_BAZOOKA:
                targetingDirectionIcon_wid.SetActive(true);
                targetingDirectionIcon_narrow.SetActive(false);
                playerDirectionIcon.SetActive(false);
                forGaugeObject.transform.GetChild(1).gameObject.SetActive(true);//shoot gauge bar
                break;
            case (int)PlayerSkillCategory.WEAPON_REVOLVER:
                ResetRevolverBulletIcon();
                targetingDirectionIcon_wid.SetActive(false);
                targetingDirectionIcon_narrow.SetActive(true);
                playerDirectionIcon.SetActive(false);
                forGaugeObject.transform.GetChild(1).gameObject.SetActive(false);//shoot gauge bar
                break;
            case (int)PlayerSkillCategory.WEAPON_KUNAI:
                targetingDirectionIcon_wid.SetActive(false);
                targetingDirectionIcon_narrow.SetActive(false);
                playerDirectionIcon.SetActive(true);
                forGaugeObject.transform.GetChild(1).gameObject.SetActive(true); //shoot gauge bar
                break;
            case (int)PlayerSkillCategory.WEAPON_SWORD:
                targetingDirectionIcon_wid.SetActive(false);
                targetingDirectionIcon_narrow.SetActive(false);
                playerDirectionIcon.SetActive(true);
                forGaugeObject.transform.GetChild(1).gameObject.SetActive(true); //shoot gauge bar
                break;
        }
    }

    public void GetRevolverSkillController(RevolverSkillController revolverSkillController)
    {
        m_revolverSkillController = revolverSkillController;
    }

    private void Update()
    {
        SetPlayerDirectionIconTransform();

        //플레이어 Shoot Gauge Bar & Hp bar Set Position
        forGaugeObject.transform.position = Camera.main.WorldToScreenPoint(m_playerMove.transform.position + new Vector3(0, -17f, 0) * Time.fixedDeltaTime);

        UpdateHealthBar(PlayerStat.m_currentHp, PlayerStat.BuffMaxHp);
    }

    void UpdateHealthBar(float _changeValue, float _maxHp)
    {
        // Set HealthBar Color
        if (_changeValue / _maxHp >= 0.75f)
        {
            fillmountHealthBar.color = new Color(1, 0.7f, 0, 1);
        }
        else if (_changeValue / _maxHp < 0.75f && _changeValue / _maxHp >= 0.5f)
        {
            fillmountHealthBar.color = new Color(1, 0.6f, 0, 1);
        }
        else if (_changeValue / _maxHp < 0.5f && _changeValue / _maxHp >= 0.25f)
        {
            fillmountHealthBar.color = new Color(1, 0.5f, 0, 1);
        }
        else if (_changeValue / _maxHp < 0.25f)
        {
            fillmountHealthBar.color = new Color(1, 0.2f, 0, 1);
        }

        // Set Gauge Value
        fillmountHealthBar.fillAmount = Mathf.Lerp(fillmountHealthBar.fillAmount, _changeValue / _maxHp, 70f * Time.deltaTime);
    }

    public void UpdateShootGaugeBar(bool shootGaugeBarActive, float _changeValue, float _attackInterval)
    {
        playerAttackCoolTime = _changeValue;
        maxplayerAttackCoolTime = _attackInterval;

        fillmountAttackCooltimeBar.fillAmount = playerAttackCoolTime / maxplayerAttackCoolTime;

        forGaugeObject.transform.GetChild(1).gameObject.SetActive(shootGaugeBarActive); //shoot gauge bar
    }

    void SetPlayerDirectionIconTransform()
    {
        float radValue = Mathf.Atan2(m_playerMove.JoysticVector.normalized.y, m_playerMove.JoysticVector.normalized.x);
        float shootAngle = radValue * (180 / Mathf.PI);

        if (currentWeaponTypeNumber == (int)PlayerSkillCategory.TOP_WEAPON_BAZOOKA)
        {
            //타겟팅 이미지 Move
            targetingDirectionIcon_wid.transform.position = Camera.main.WorldToScreenPoint(m_playerMove.transform.position + new Vector3(0, 0, 1) * Time.fixedDeltaTime);
            targetingDirectionIcon_wid.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, shootAngle + 180);
        }
        else if(currentWeaponTypeNumber == (int)PlayerSkillCategory.WEAPON_REVOLVER)
        {
            //타겟팅 이미지 Move
            targetingDirectionIcon_narrow.transform.position = Camera.main.WorldToScreenPoint(m_playerMove.transform.position + new Vector3(0, 0, 1) * Time.fixedDeltaTime);
            targetingDirectionIcon_narrow.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, shootAngle + 180);
        }
        else
        {
            //플레이어 방향 아이콘 MOVE
            playerDirectionIcon.transform.position = Camera.main.WorldToScreenPoint(m_playerMove.transform.position + new Vector3(0, 0, 1) * Time.fixedDeltaTime);
            playerDirectionIcon.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, shootAngle + 180);
        }
    }

    void ResetRevolverBulletIcon()
    {
        for (int i = 0; i < max_RevolverBulletUICount; ++i)
        {
            GameObject oneRevolverBulletUI = Instantiate(revolverBulletLayoutPrefab[bulletPrefabType]);

            oneRevolverBulletUI.transform.SetParent(revolverBulletCountRect);
            oneRevolverBulletUI.transform.gameObject.SetActive(true);

            revolverBulletUIQueue.Enqueue(oneRevolverBulletUI);
        }
    }

    // About Revolver Bullet UI
    public void CreateOneRevolverBulletIcon(int bulletPrefabType)
    {
        GameObject oneRevolverBulletUI = Instantiate(revolverBulletLayoutPrefab[bulletPrefabType]);

        oneRevolverBulletUI.transform.SetParent(revolverBulletCountRect);
        oneRevolverBulletUI.transform.gameObject.SetActive(true);

        revolverBulletUIQueue.Enqueue(oneRevolverBulletUI);
    }

    public void DeleteRevolverBulletIconUI()
    {
        if (revolverBulletUIQueue.Count > 0)
        {
            GameObject bulletUI = revolverBulletUIQueue.Dequeue();
            Destroy(bulletUI);
        }
        else
        {
            revolverBulletUIQueue.Clear();

            StartCoroutine(RevolerBulletUILoading());
        }
    }

    IEnumerator RevolerBulletUILoading() 
    {
        isRevolerBulletUILoading = true;

        int bulletLoadingTime = 3;

         forGaugeObject.transform.GetChild(1).gameObject.SetActive(true);
        
        float time = 0;
        while (time <= bulletLoadingTime)
        {
            time += Time.deltaTime;
            fillmountAttackCooltimeBar.fillAmount = time / bulletLoadingTime;
            yield return null;
        }

        if (revolverBulletUIQueue.Count <= 0)
            ResetRevolverBulletIcon();

        forGaugeObject.transform.GetChild(1).gameObject.SetActive(false);

        m_revolverSkillController.RevolerLoading(false);
        isRevolerBulletUILoading = false;

    }

    public void ChangeRevolverBulletUIObject()
    {
        bulletPrefabType = 1;

        for (int i=0; i < revolverBulletCountRect.childCount; ++i)
        {
            Destroy(revolverBulletCountRect.GetChild(i).gameObject);
        }
        revolverBulletUIQueue.Clear();

        if (!isRevolerBulletUILoading)
            ResetRevolverBulletIcon();
    }
}
