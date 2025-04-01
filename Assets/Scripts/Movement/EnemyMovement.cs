using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private Transform player;

    private float deactivateTime = 1;
    private float velocity = 4.5f;
    private float normalVelocity = 4.5f;
    private float recoilVelocity = -1f;

    private void OnEnable()
    {
        gameObject.GetComponent<IDamageable>().OnHitted.AddListener(RecoilMovement);
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void OnDisable()
    {
        gameObject.GetComponent<IDamageable>().OnHitted.RemoveListener(RecoilMovement);
        velocity = normalVelocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && velocity == normalVelocity)
            RecoilMovement();
    }

    public void RecoilMovement()
    {
        if (!gameObject.activeSelf) return;
        velocity = recoilVelocity;
        Invoke(nameof(RecoilMovementCO), deactivateTime);
    }

    void RecoilMovementCO()
    {
        velocity = normalVelocity;
    }

    private void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.position, velocity * Time.deltaTime);
    }

    void Update()
    {
        Move();
    }
}
