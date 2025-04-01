using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LifeSystem : MonoBehaviour, IDamageable
{
    private UnityEvent _onHitted = new UnityEvent();
    public event Action onDeath;
    public UnityEvent OnHitted { get { return _onHitted; } }

    private int increaseFactor = 100;
    private int maxLife = 100;
    private int healAmount = 40;
    public int life { get { return _life; } }
    private int _life = 100;

    private void OnEnable()
    {
        _life = maxLife;
    }

    public void ReceiveDamage(int amount)
    {
        _life -= amount;
        _onHitted.Invoke();
        if (_life <= 0)
        {
            _life = 0;
            onDeath.Invoke();
        }
    }

    public void Heal()
    {
        _life += healAmount;
        if (_life > maxLife)
        {
            _life = maxLife;
        }
        _onHitted.Invoke();
    }

    private void ResetLife()
    {
        maxLife = increaseFactor;
        _life = maxLife;
    }

    public void ConfigureEnemy(IGameCycle gameCycle, RecyclableObject recyclableObject, int actualLevelEnemyLife)
    {
        _life = actualLevelEnemyLife;
        gameCycle.OnResetLevel += ResetLife;
        recyclableObject.OnRelease += SetLifeToMaxLife;
    }

    public void ConfigurePlayer(IGameCycle gameCycle)
    {
        onDeath += ResetLife;
    }

    private void SetLifeToMaxLife()
    {
        _life = maxLife;
    }
}
