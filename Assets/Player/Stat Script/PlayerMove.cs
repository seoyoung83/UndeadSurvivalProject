using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerMovementState
{
    Moveable,
    NonMoveable,
}

public class PlayerMove : MonoBehaviour
{
    PlayerMovementState m_playerMovementState;

    [SerializeField] FloatingJoystick joy;

    Animator animator;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;

    Vector2 joysticVect;

    [Header("Slow Debuff")]
    float slowDebuffCount = 0;
    float debuff_slowSpeed = 1;
    float checkingTime_abnormalStat = 0;
    Color[] playerStateColor = { Color.white, new Color(1, 0.3f, 0.5f, 1f) };

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator= GetComponent<Animator>();

        joysticVect = Vector2.left;
    }

    public Vector2 JoysticVector
    {
        get { return joysticVect; }
    }

    private void FixedUpdate()
    {
        if (m_playerMovementState != PlayerMovementState.Moveable)
            return;

        Move();

        //Sprite Direction
        spriteRenderer.flipX = joysticVect.x > 0 ? false: true;
    }

    void Move()
    {
        //Move
        //float x = Input.GetAxis("Horizontal");
        //float y = Input.GetAxis("Vertical");

        float x = joy.Horizontal;
        float y = joy.Vertical;

        Vector2 _moveVect = new Vector2(x, y) * PlayerStat.BuffSpeed * debuff_slowSpeed * Time.fixedDeltaTime;

        rigid.MovePosition(rigid.position + _moveVect);

        if (joy.transform.GetChild(0).gameObject.activeInHierarchy == true)
            joysticVect = new Vector2(x, y);

        if (x != 0 || y != 0)
            animator.SetBool("isWalk", true);
        else
            animator.SetBool("isWalk", false);
    }

    public void AbnormalStat()
    {
        debuff_slowSpeed = 0.3f;
        slowDebuffCount += 1.5f;

        StartCoroutine(StartAbnormalStatEffect());

        Invoke("NormalStat",slowDebuffCount);
    }

    void NormalStat()
    {
        if (slowDebuffCount >= 1.5f)
            slowDebuffCount -= 1.5f;

        if(slowDebuffCount <= 0)
        {
            debuff_slowSpeed = 1;
            checkingTime_abnormalStat = 0;
            GetComponent<SpriteRenderer>().material.color = Color.white;
        }
    }

    IEnumerator StartAbnormalStatEffect()
    {
        while (slowDebuffCount > 0)
        {
            checkingTime_abnormalStat += Time.deltaTime;

            if (checkingTime_abnormalStat > 1)
            {
                checkingTime_abnormalStat = 0f;
            }

            float _value;
            _value = Mathf.PingPong(checkingTime_abnormalStat * 10f, playerStateColor.Length);
            spriteRenderer.material.SetColor("_Color", playerStateColor[(int)_value]);

            yield return null;
        }
    }

    public void SetPlayerMovementType(PlayerMovementState _state)
    {
        m_playerMovementState = _state;
    }
}
