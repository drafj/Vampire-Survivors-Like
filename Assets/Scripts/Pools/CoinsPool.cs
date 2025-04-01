using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsPool : MonoBehaviour, IPool
{
    [SerializeField] private GameObject coinPrefab;
    private ObjectPool poolObject;

    private void Awake()
    {
        poolObject = new ObjectPool(coinPrefab.GetComponent<RecyclableObject>());
        poolObject.Init(40);
    }

    public void SpawnObject(Transform spawnPosition = null)
    {
        poolObject.Spawn<GameObject>(spawnPosition.position);
    }

    public void DespawnObjects()
    {
        poolObject.RecycleAllObjects();
    }
}
