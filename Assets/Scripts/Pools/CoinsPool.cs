using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsPool : MonoBehaviour, IPoolWithParams
{
    [SerializeField] private List<GameObject> pool = new List<GameObject>();
    [SerializeField] private GameObject coinPrefab;

    public void SpawnObject(Vector3 position)
    {
        if (pool[0].activeSelf)
        {
            Instantiate(coinPrefab, pool[pool.Count - 1].transform.position, Quaternion.identity);
        }
        else
        {
            pool[0].transform.position = position;
            pool[0].SetActive(true);
            RotateOrders();
        }
    }

    public void DespawnObjects()
    {
        for (int i = 0; i < pool.Count; i++)
        {
            pool[i].SetActive(false);
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
