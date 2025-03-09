using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SawDamage : MonoBehaviour
{
    private int damage;
    private IStats stats;
    private IGameCycle startGame;

    private void Awake()
    {
        SetInterfaces();
    }

    private void OnEnable()
    {
        startGame.OnGameStarted.AddListener(SetDamage);
        SetDamage();
    }

    private void SetDamage()
    {
        damage = stats.Damage;
    }

    private void SetInterfaces()
    {
        stats = FindObjectsOfType<MonoBehaviour>().OfType<IStats>().FirstOrDefault();
        startGame = FindObjectsOfType<MonoBehaviour>().OfType<IGameCycle>().FirstOrDefault();
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
