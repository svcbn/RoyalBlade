using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]

public class Player : MonoBehaviour
{
    public static Player instance;

    int _jumpSkillCount = 0;
    [SerializeField] int maxJumpSkillCount = 3;
    public int JumpSkillCount
    {
        get
        {
            return _jumpSkillCount;
        }
        set
        {
            if (_jumpSkillCount < maxJumpSkillCount)
            {
                _jumpSkillCount = value;
            }
        }
    }

    int _attackSkillCount = 0;
    [SerializeField] int maxAttackSkillCount = 10;
    public int AttackSkillCount
    {
        get
        {
            return _attackSkillCount;
        }
        set
        {
            if (_attackSkillCount < maxAttackSkillCount)
            {
                _attackSkillCount = value;
            }
        }
    }

    public enum PlayerState
    {
        Idle,
        Air,
        Crush
    };
    
    public PlayerState playerstate;

    [SerializeField] float jumpPower = 20f;
    Rigidbody2D rb;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        playerstate = PlayerState.Idle;
        rb = GetComponent<Rigidbody2D>();
        attackArea.SetActive(false);
        guardArea.SetActive(false);
        isGuardCoolDown = false;
    }

    void ChangeState(PlayerState state)
    {
        playerstate = state;
        switch (playerstate)
        {
            case PlayerState.Idle:
                PlayerIdle();
                break;
            case PlayerState.Air:
                PlayerAir();
                break;
            case PlayerState.Crush:
                PlayerCrush();
                break;
        }
    }

    void PlayerIdle()
    {
        isGround = true;
        rb.simulated = true;
    }

    void PlayerAir()
    {
        isGround = false;
    }

    void PlayerCrush()
    {
        rb.simulated = false;
        GameManager.instance.Combo = 0;
        GameManager.instance.playerHP--;
    }

    [SerializeField]bool isGround = false;
    public void Jump()
    {
        if (playerstate != PlayerState.Idle) return;

        rb.velocity += Vector2.up * jumpPower;
        JumpSkillCount++;
        ChangeState(PlayerState.Air);
    }

    bool isAttack = false;
    public void Attack()
    {
        if(isAttack) return;
        
        StartCoroutine(CoAttack());
        if(playerstate == PlayerState.Air)
        {
            onCollisionEnemy = false;
        }
        if (playerstate == PlayerState.Crush)
        {
            rb.simulated = true;
        }
    }

    [SerializeField] float attackDelayTime = 0.15f;
    [HideInInspector] public GameObject attackArea;
    IEnumerator CoAttack()
    {
        isAttack = true;
        float currentTime = 0;
        attackArea.SetActive(true);

        while(currentTime < attackDelayTime)
        {
            currentTime += Time.deltaTime;
            if (currentTime > 0.05f) attackArea.SetActive(false);
            yield return null;
        }
        isAttack = false;
    }

    [HideInInspector] public bool isGuard = false;
    [HideInInspector] public GameObject guardArea;
    public void Guard()
    {
        if (isGuardCoolDown) return;
        isGuard = true;
        guardArea.SetActive(true);
    }

    [SerializeField] float defensePower = 15f;
    public void Defense(Collider2D collision)
    {
        collision.gameObject.GetComponent<Rigidbody2D>().velocity += (Vector2.up * defensePower);
        rb.velocity += (Vector2.down * defensePower);
        StartCoroutine(GuardCoolDown());
        if(playerstate == PlayerState.Crush)
        {
            ChangeState(PlayerState.Idle);
        }
    }

    [HideInInspector] public bool isGuardCoolDown = false;
    [SerializeField] float guardCoolDownTime = 2f;

    public IEnumerator GuardCoolDown()
    {
        isGuard = false;
        isGuardCoolDown = true;
        guardArea.SetActive(false);
        float currentTime = 0;

        while (currentTime < guardCoolDownTime)
        {
            currentTime += Time.deltaTime;
            yield return null;
        }
        isGuardCoolDown = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.layer)
        {
            case 7:     // 지면충돌
                if (onCollisionEnemy)   // 적충돌 동시 지면충돌
                {
                    ChangeState(PlayerState.Crush);
                }
                else                    // 지면하고만
                {
                    ChangeState(PlayerState.Idle);
                }
                break;
            case 8:     // 적충돌
                if (playerstate == PlayerState.Idle)    // 지상에서 깔림
                {
                    ChangeState(PlayerState.Crush);
                }
                break;
        }
    }

    bool onCollisionEnemy = false;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 8)
        {
            onCollisionEnemy = true;
        }
        
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            onCollisionEnemy = false;
        }
    }
}
