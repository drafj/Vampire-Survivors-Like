using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Installer : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPool;
    [SerializeField] private GameObject _coinPool;
    [SerializeField] private GameCycle _gameCycle;
    [SerializeField] private StatsManager _statsManager;
    [SerializeField] private GameObject _player;
    [SerializeField] private List<GameObject> _saws;

    private void Awake()
    {
        SetStatsDependences();
        SetStatsManagerDependences();
        SetSawsDependences();
        SetPlayerDependences();
    }

    public void SetEnemyDependences(Enemy enemy)
    {
        int actualLevelEnemyLife = _gameCycle.actualLevelEnemyLife;
        IGameCycle gameCycle = _gameCycle;
        enemy.SetEnemyPool(_enemyPool.GetComponent<IPool>());
        enemy.SetCoinsPool(_coinPool.GetComponent<IPool>());
        enemy.GetComponent<LifeSystem>().ConfigureEnemy(gameCycle, enemy, actualLevelEnemyLife);
    }

    public void SetStatsDependences()
    {
        _gameCycle.SetEnemyPool(_enemyPool.GetComponent<IPool>());
        _gameCycle.SetCoinsPool(_coinPool.GetComponent<IPool>());
    }

    public void SetStatsManagerDependences()
    {
        _statsManager.SetPlayer(_player.GetComponent<IDamageable>());
    }

    public void SetSawsDependences()
    {
        for (int i = 0; i < _saws.Count; i++)
        {
            _saws[i].GetComponent<SawDamage>().SetStatsManager(_statsManager.GetComponent<IStats>());
            _saws[i].GetComponent<SawMovement>().SetStatsManager(_statsManager.GetComponent<IStats>());
            _saws[i].GetComponent<SawDamage>().SetGameCycle(_gameCycle.GetComponent<IGameCycle>());
            _saws[i].GetComponent<SawMovement>().SetGameCycle(_gameCycle.GetComponent<IGameCycle>());
        }
    }

    public void SetPlayerDependences()
    {
        _player.GetComponent<LifeSystem>().ConfigurePlayer(_gameCycle.GetComponent<IGameCycle>());
    }
}
