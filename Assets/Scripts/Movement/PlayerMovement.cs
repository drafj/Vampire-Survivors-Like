using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] private float velocity;

    private void Move()
    {
        Vector2 movementInput = playerInput.actions["Move"].ReadValue<Vector2>();

        Vector3 actualDirection = new Vector3(movementInput.x, movementInput.y, 0);

        rb.MovePosition(transform.position + actualDirection * velocity * Time.fixedDeltaTime);
    }

    void FixedUpdate()
    {
        Move();
    }
}
