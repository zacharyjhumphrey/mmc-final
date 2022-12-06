using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    Animator m_animator;
    SpriteRenderer m_spriteRenderer;
    Rigidbody2D rb;

    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        float dir = Input.GetAxisRaw("Horizontal");

        if (Mathf.Abs(dir) < 0.01f) {
            return;
        }

        m_spriteRenderer.flipX = dir < 0;

        // TODO Animate run
    }
}
