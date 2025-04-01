using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : RecyclableObject
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private List<Color> colors = new List<Color>();
    private int damage = 20;
    private IPool _enemyPool;
    private IPool _coinPool;
    private IDamageable _lifeSystem;

    private void Awake()
    {
        SetLifeSystem();
    }

    public override void Init()
    {
        if (_enemyPool == null || _coinPool == null)
            FindObjectOfType<Installer>().SetEnemyDependences(this);

        while (_enemyPool == null) return;

        spriteRenderer.color = colors[Random.Range(0, colors.Count)];
        _lifeSystem.onDeath += SpawnNewEnemy;
        _lifeSystem.onDeath += Dead;
    }

    private void SpawnNewEnemy()
    {
        _enemyPool.SpawnObject();
    }

    public void SetEnemyPool(IPool enemyPool)
    {
        _enemyPool = enemyPool;
    }

    public void SetCoinsPool(IPool coinPool)
    {
        _coinPool = coinPool;
    }

    private void SetLifeSystem()
    {
        _lifeSystem = GetComponent<IDamageable>();
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
        _coinPool.SpawnObject(transform);
        Recycle();
    }

    public override void Release()
    {
        base.Release();
        _lifeSystem.onDeath -= SpawnNewEnemy;
        _lifeSystem.onDeath -= Dead;
    }
}
