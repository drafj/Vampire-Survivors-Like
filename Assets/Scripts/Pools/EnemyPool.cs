using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyPool : MonoBehaviour, IPool
{
    [SerializeField] private List<GameObject> pool = new List<GameObject>();
    [SerializeField] private List<Transform> spawnPoints = new List<Transform>();
    private IGameCycle gameCycle;
    private bool inGame = true;

    private void Awake()
    {
        gameCycle = FindObjectsOfType<MonoBehaviour>().OfType<IGameCycle>().FirstOrDefault();
        gameCycle.OnGameStarted.AddListener(ActivateSpawn);
    }

    public void SpawnObject()
    {
        if (!inGame) return;
        if (pool[0].activeSelf)
        {
            pool.Add(Instantiate(pool[Random.Range(0, pool.Count)], new Vector3(1000, 1000, 1000), Quaternion.identity));
        }
        else
        {
            pool[0].SetActive(true);
        }
        pool[0].transform.position = spawnPoints[Random.Range(0, spawnPoints.Count)].position;
        RotateOrders();
    }

    private void ActivateSpawn()
    {
        inGame = true;
    }

    public void DespawnObjects()
    {
        inGame = false;
        for (int i = 0; i < pool.Count; i++)
        {
            pool[i].SetActive(false);
        }
    }

    public void IncreaseEnemiesLife()
    {
        for (int i = 0; i < pool.Count; i++)
        {
            pool[i].GetComponent<IDamageable>().IncreaseMaxLife();
        }
    }

    public void ResetEnemiesLife()
    {
        for (int i = 0; i < pool.Count; i++)
        {
            pool[i].GetComponent<IDamageable>().ResetLife();
        }
    }

    public void RotateOrders()
    {
        GameObject actual = null;
        GameObject save = null;

        for (int i = 0; i < pool.Count; i++)
        {
            if (i == 0)
            {
                actual = pool[i];
                save = pool[i + 1];
                pool[i + 1] = actual;
            }
            else if (i < pool.Count - 1)
            {
                actual = save;
                save = pool[i + 1];
                pool[i + 1] = actual;
            }
            else
            {
                pool[0] = save;
            }
        }
    }
}
