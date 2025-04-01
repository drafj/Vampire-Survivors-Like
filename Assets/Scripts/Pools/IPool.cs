using System.Collections.Generic;
using UnityEngine;

public interface IPool
{
    public void SpawnObject(Transform spawnPosition = null);
    public void DespawnObjects();
}
