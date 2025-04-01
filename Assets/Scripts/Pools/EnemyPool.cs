using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyPool : MonoBehaviour, IPool
{
    [SerializeField] private List<Transform> spawnPoints = new List<Transform>();
    [SerializeField] private GameObject enemyPrefab;
    private ObjectPool newPool;
    private IGameCycle gameCycle;
    private bool inGame = true;

    private void Awake()
    {
        gameCycle = FindObjectsOfType<MonoBehaviour>().OfType<IGameCycle>().FirstOrDefault();
        gameCycle.OnGameStarted.AddListener(ActivateSpawn);

        newPool = new ObjectPool(enemyPrefab.GetComponent<RecyclableObject>());
        newPool.Init(40);
    }

    public void SpawnObject(Transform spawnPositiion = null)
    {
        if (!inGame) return;
        newPool.Spawn<GameObject>(spawnPoints[Random.Range(0, spawnPoints.Count)].position);
    }

    private void ActivateSpawn()
    {
        inGame = true;
    }

    public void DespawnObjects()
    {
        inGame = false;
        newPool.RecycleAllObjects();
    }

    /*public void RotateOrders()
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
    }*/
}
