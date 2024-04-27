using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;


public class BehaviourPool<T> where T : Behaviour
{
    private List<T> _pool;
    private int _defaultSize = 10;
    private float _incrementalRate = 1.5f;
    private bool _isInitialized = false;
    private GameObject _owner;
    
    
    public struct Initializer
    {
        public GameObject owner;
        public int defaultSize;
        public float incrementalRate;
    }
    
    public BehaviourPool(Initializer initializer)
    {
        Initialize(initializer);
    }

    private void Initialize(Initializer initializer)
    {
        if (initializer.owner == null)
        {
            // useless;
            return;
        }

        _owner = initializer.owner;
        _defaultSize = math.max(1, initializer.defaultSize);
        _incrementalRate = math.max(1.0f, initializer.incrementalRate);
        _isInitialized = true;
        Increase(_defaultSize);
    }

    private T Increase(int incSize)
    {
        if (_owner == null)
        {
            return null;
        }

        _pool ??= new();

        int nextIdx = _pool.Count;
        
        for (int i = 0; i < incSize; ++i)
        {
            var obj = _owner.AddComponent<T>();
            obj.enabled = false;
            _pool.Add(obj);
        }

        return _pool[nextIdx];
    }

    private T Increase(float rate)
        => Increase(math.max(1, (int)(_pool.Count * rate - _pool.Count)));

    public virtual T Get(bool asEnabled = true)
    {
        if (IsValid() == false)
        {
            return null;
        }
        
        T obj = _pool.FirstOrDefault(c => c.enabled == false) ?? Increase(_incrementalRate);
        obj.enabled = asEnabled;
        return obj;
    }

    public virtual void Release(T toRelease)
    {
        if (toRelease == null)
        {
            return;
        }
        
        if (IsValid() == false)
        {
            return;
        }

        toRelease.enabled = false;
    }
    
    public bool IsValid()
        => _isInitialized && _pool is { Count: > 0 };
}