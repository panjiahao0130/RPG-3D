using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T:Singleton<T>
{
    private static T instance;

    public static T Instance
    {
        get
        {
            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (instance==null)
        {
            instance = this as T;
        }
        else
        {
            Destroy(gameObject);
        }
        //DontDestroyOnLoad(this);
    }

    public static bool IsInitialized
    {
        get
        {
            return instance != null;
        }
    }

    protected void OnDestroy()
    {
        if (instance==this)
        {
            instance = null;
        }
    }
}
