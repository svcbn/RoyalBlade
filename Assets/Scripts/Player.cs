using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]

public class Player : MonoBehaviour
{
    int _hp = 0;

    public int HP
    {
        get
        {
            return _hp;
        }
        set
        {
            _hp = value;
        }
    }

    public enum PlayerState
    {
        Idle,
        Air,
        Crush
    };
    
    public PlayerState state;

    public float gravityAccel = 0;
    public float jumpPower = 10f;
    Rigidbody2D rb;



    void Start()
    {
        state = PlayerState.Idle;
        rb = GetComponent<Rigidbody2D>();

    }

    void Update()
    {
        switch (state)
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

    bool isJump = false;
    void PlayerIdle()
    {

    }

    void PlayerAir()
    {

    }

    void PlayerCrush()
    {

    }

    public void Jump()
    {
        if (state != PlayerState.Idle) return;

        rb.velocity += Vector2.up * jumpPower;
        state = PlayerState.Air;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            state = PlayerState.Idle;
        }
    }
}
