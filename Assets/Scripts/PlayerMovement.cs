using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    Vector2 m_moveInput;

    Rigidbody2D m_rigidBody;

    Animator m_animator;

    bool IsRunning;

    [SerializeField] float MoveSpeed = 10.0f;
    [SerializeField] float JumpForce = 800.0f;


    void Start()
    {
        m_rigidBody = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();
    }

    void Update()
    {
        Run();
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
            m_rigidBody.AddForce(new(0.0f,JumpForce));
        }
    }

    void Run()
    {
        Vector2 playerVelocity = new(m_moveInput.x * MoveSpeed, m_rigidBody.velocity.y);
        m_rigidBody.velocity = playerVelocity;
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
    }
}
