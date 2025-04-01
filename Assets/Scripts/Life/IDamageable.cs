using System;
using UnityEngine.Events;

public interface IDamageable
{
    public UnityEvent OnHitted { get; }
    public event Action onDeath;
    public int life { get; }
    public void ReceiveDamage(int amount);
    public void Heal();
}
