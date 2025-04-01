using UnityEngine;

public class SawDamage : MonoBehaviour
{
    private int damage;
    private IStats _stats;
    private IGameCycle _startGame;

    private void OnEnable()
    {
        _startGame.OnGameStarted.AddListener(SetDamage);
        SetDamage();
    }

    private void SetDamage()
    {
        damage = _stats.Damage;
    }

    public void SetStatsManager(IStats stats)
    {
        _stats = stats;
    }

    public void SetGameCycle(IGameCycle gameCycle)
    {
        _startGame = gameCycle;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Enemy")) return;
        if (collision.gameObject.TryGetComponent(out IDamageable target))
        {
            target.ReceiveDamage(damage);
        }
    }
}
