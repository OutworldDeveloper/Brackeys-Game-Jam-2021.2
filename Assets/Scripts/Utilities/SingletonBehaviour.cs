using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SingletonBehaviour<T> : MonoBehaviour where T : SingletonBehaviour<T>
{
    public static T Instance { get; private set; }

    protected abstract bool UseDontDestroyOnLoad { get; }

    private void Awake()
    {
        if (Instance && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this as T;
        if (UseDontDestroyOnLoad)
            DontDestroyOnLoad(gameObject);
        OnSingletonAwake();
    }

    protected virtual void OnSingletonAwake() { }

}