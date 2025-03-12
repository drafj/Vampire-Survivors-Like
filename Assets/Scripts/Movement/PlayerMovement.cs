using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] private float velocity;
    [SerializeField] Animator anim;

    private Vector2 movementInput;
    private Vector3 actualDirection;

    private void Move()
    {
        movementInput = playerInput.actions["Move"].ReadValue<Vector2>();

        actualDirection = new Vector3(movementInput.x, movementInput.y, 0);

        rb.MovePosition(transform.position + actualDirection * velocity * Time.fixedDeltaTime);
    }

    private void AnimationsUpdater()
    {
        if (actualDirection.x > 0)
            sprite.flipX = false;
        else if (actualDirection.x < 0)
            sprite.flipX = true;

        anim.SetBool("Walk", actualDirection.magnitude > 0);
    }

    private void Update()
    {
        AnimationsUpdater();
    }

    void FixedUpdate()
    {
        Move();
    }
}
