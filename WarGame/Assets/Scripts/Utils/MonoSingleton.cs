using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static T _instance;
    public static T instance
    {
        get
        {
            if (_instance == null)
            {
                //Debug.Log("Instance not initialized");
            }

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this as T;
        ExecuteOnAwake();
    }

    private void OnDestroy()
    {
        if (_instance == this as T)
        {
            _instance = null;
        }

        ExecuteOnDestroy();
    }

    protected virtual void ExecuteOnAwake() { }
    protected virtual void ExecuteOnDestroy() { }
}
