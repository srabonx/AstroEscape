using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    Vector2 m_moveInput;

    Rigidbody2D m_rigidBody;

    BoxCollider2D m_boxCollider;

    Animator m_animator;

    bool IsRunning;
    bool IsJumping;
    bool IsFalling;

    bool IsClimbing;

    bool IsOnGround;

    [SerializeField] float MoveSpeed = 10.0f;
    [SerializeField] float JumpForce = 1000.0f;
    float   GravityScale = 4.0f;

    int m_groundMask = 0;


    void Start()
    {
        m_rigidBody = GetComponent<Rigidbody2D>();
        m_boxCollider = GetComponent<BoxCollider2D>();
        m_animator = GetComponent<Animator>();

        GravityScale = m_rigidBody.gravityScale;

        m_groundMask = LayerMask.GetMask("Ground");
    }

    void OnCollisionEnter2D(Collision2D other)
    {       
        if(m_boxCollider.IsTouchingLayers(m_groundMask))
        {
            IsOnGround = true;
            IsFalling = false;
            IsJumping = false;
        }
        else
        {
            IsOnGround = false;
        }   
    }

    void Update()
    {
        Run();
        ClimbLadder();
        FlipSprite();
        ChangeAnimation();
    }

    void OnMove(InputValue inputValue)
    {
        m_moveInput = inputValue.Get<Vector2>();
    }

    void OnJump(InputValue inputValue)
    {
        if(inputValue.isPressed)
        {
            if(IsOnGround)
            {
                m_rigidBody.AddForce(new(0.0f,JumpForce));
                IsJumping = true;
                IsFalling = false;
                IsOnGround = false;
            }
        }
    }

    void Run()
    {
        Vector2 playerVelocity = new(m_moveInput.x * MoveSpeed, m_rigidBody.velocity.y);
        m_rigidBody.velocity = playerVelocity;

        if(m_rigidBody.velocity.y < 0 && !IsOnGround)
        {
            IsFalling = true;
            IsJumping = false;        
        }

    }

    void FlipSprite()
    {
        if(Mathf.Abs(m_moveInput.x) > Mathf.Epsilon)
        {
            transform.localScale = new(Mathf.Sign(m_moveInput.x),1.0f);
            IsRunning = true;
        }
        else
        {
            IsRunning = false;
        }
    }

    void ChangeAnimation()
    {
        m_animator.SetBool("IsRunning", IsRunning);

        m_animator.SetBool("IsJumping", IsJumping);

        m_animator.SetBool("IsFalling", IsFalling);

        m_animator.SetBool("IsClimbing", IsClimbing);
    }

    void ClimbLadder()
    {
        if(!m_boxCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            m_rigidBody.gravityScale = GravityScale;
            return;
        }

        IsJumping = false;
        IsFalling = false;
        IsOnGround = false;

        Vector2 climbVelocity = new(m_rigidBody.velocity.x, m_moveInput.y * MoveSpeed);
        m_rigidBody.velocity = climbVelocity;

        if(m_moveInput.y != 0)
        {
            IsClimbing = true;
        }
        else
        {
            IsClimbing = false;
        }

        m_rigidBody.gravityScale = 0;
    }
}
