using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int damage = 20;
    private IPool enemyPool;
    private IDamageable lifeSystem;
    private IPoolWithParams coinPool;

    private void Awake()
    {
        SetInterfaces();
    }

    private void OnEnable()
    {
        lifeSystem.OnDeath.AddListener(enemyPool.SpawnObject);
        lifeSystem.OnDeath.AddListener(Dead);
    }

    private void SetInterfaces()
    {
        enemyPool = GameObject.FindGameObjectWithTag("EnemyPool").GetComponent<IPool>();
        coinPool = GameObject.FindGameObjectWithTag("CoinPool").GetComponent<IPoolWithParams>();
        lifeSystem = GetComponent<IDamageable>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        if (collision.gameObject.TryGetComponent(out IDamageable player))
        {
            player.ReceiveDamage(damage);
        }
    }

    private void Dead()
    {
        coinPool.SpawnObject(transform.position);
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        lifeSystem.OnDeath.RemoveListener(enemyPool.SpawnObject);
        lifeSystem.OnDeath.RemoveListener(Dead);
        lifeSystem.SetLifeToMaxLife();
    }
}
