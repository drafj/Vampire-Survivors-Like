using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class ObjectPool
{
    private RecyclableObject _prefab;
    private HashSet<RecyclableObject> _instantiatedObjects;
    private Queue<RecyclableObject> _recycledObjects;

    public ObjectPool(RecyclableObject prefab)
    {
        _prefab = prefab;
        _instantiatedObjects = new HashSet<RecyclableObject>();
    }

    public void Init(int numberOfInitialObjects)
    {
        _recycledObjects = new Queue<RecyclableObject>(numberOfInitialObjects);

        for (int i = 0; i < numberOfInitialObjects; i++)
        {
            var instance = InstantiateNewObject();
            instance.gameObject.SetActive(false);
            _recycledObjects.Enqueue(instance);
        }
    }

    private RecyclableObject InstantiateNewObject()
    {
        var instance = MonoBehaviour.Instantiate(_prefab);
        instance.Configure(this);
        return instance;
    }

    public T Spawn<T>(Vector3 position)
    {
        var recyclableObject = GetInstance();
        _instantiatedObjects.Add(recyclableObject);
        recyclableObject.transform.position = position;
        recyclableObject.gameObject.SetActive(true);
        recyclableObject.Init();
        return recyclableObject.GetComponent<T>();
    }

    private RecyclableObject GetInstance()
    {
        if (_instantiatedObjects.Count > 0)
            return _recycledObjects.Dequeue();

        var instance = InstantiateNewObject();
        return instance;
    }

    public void RecycleGameObject(RecyclableObject gameObjectToRecycle)
    {
        var wasInstantiated = _instantiatedObjects.Remove(gameObjectToRecycle);
        Assert.IsTrue(wasInstantiated, $"{gameObjectToRecycle.name} was not instantiated in this pool");

        gameObjectToRecycle.gameObject.SetActive(false);
        gameObjectToRecycle.Release();
        _recycledObjects.Enqueue(gameObjectToRecycle);
    }

    public void RecycleAllObjects()
    {
        _instantiatedObjects.ToList().ForEach(recyclableObject => recyclableObject.Recycle());
    }
}
