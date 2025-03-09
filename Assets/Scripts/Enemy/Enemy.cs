using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    private int damage = 20;
    [SerializeField] GameObject enemyPoolGO;
    private IPool enemyPool;
    private IHealable healInterface;
    [SerializeField] GameObject coinPoolGO;
    private IPoolWithParams coinPool;
    private UnityEvent OnDeath;

    private void Awake()
    {
        SetInterfaces();
    }

    private void OnEnable()
    {
        OnDeath.AddListener(enemyPool.SpawnObject);
        OnDeath.AddListener(Dead);
    }

    private void SetInterfaces()
    {
        enemyPool = enemyPoolGO.GetComponent<IPool>();
        coinPool = coinPoolGO.GetComponent<IPoolWithParams>();
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
        OnDeath.RemoveListener(enemyPool.SpawnObject);
        OnDeath.RemoveListener(Dead);
        healInterface.SetLifeToMaxLife();
    }
}
