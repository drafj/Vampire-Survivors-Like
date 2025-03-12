using UnityEngine.Accessibility;
using UnityEngine.Events;

public interface IDamageable
{
    public UnityEvent OnHitted { get; }
    public UnityEvent OnDeath { get; }
    public int life { get; }
    public void ReceiveDamage(int amount);
    public void Heal();
    public void ResetLife();
    public void IncreaseMaxLife();
    public void SetLifeToMaxLife();
}
