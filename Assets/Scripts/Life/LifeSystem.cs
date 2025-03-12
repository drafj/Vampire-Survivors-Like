using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class LifeSystem : MonoBehaviour, IDamageable
{
    private UnityEvent _onHitted = new UnityEvent();
    private UnityEvent _onDeath = new UnityEvent();
    public UnityEvent OnHitted { get { return _onHitted; } }
    public UnityEvent OnDeath { get { return _onDeath; } }

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
            _onDeath.Invoke();
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

    public void ResetLife()
    {
        maxLife = increaseFactor;
        _life = maxLife;
    }

    public void SetLifeToMaxLife()
    {
        _life = maxLife;
    }

    public void IncreaseMaxLife()
    {
        maxLife = maxLife + increaseFactor;
        _life = maxLife;
    }
}
