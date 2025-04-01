using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RecyclableObject : MonoBehaviour
{
    public event Action OnRelease;
    private ObjectPool _pool;

    public void Configure(ObjectPool pool)
    {
        _pool = pool;
    }

    public void Recycle()
    {
        _pool.RecycleGameObject(this);
    }

    public abstract void Init();
    public virtual void Release()
    {
        OnRelease?.Invoke();
    }
}
