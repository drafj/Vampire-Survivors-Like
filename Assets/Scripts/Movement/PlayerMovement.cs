using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private float velocity;
    [SerializeField] private Rigidbody rb;

    private void Move()
    {
        Vector2 movementInput = playerInput.actions["Move"].ReadValue<Vector2>();

        Vector3 actualDirection = new Vector3(movementInput.x, 0, movementInput.y);

        rb.MovePosition(transform.position + actualDirection * velocity * Time.fixedDeltaTime);
    }

    void FixedUpdate()
    {
        Move();
    }
}
