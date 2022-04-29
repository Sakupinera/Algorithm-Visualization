using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

/// <summary>
/// Unity的Mono单例基类
/// </summary>
/// <typeparam name="T">对应的子类</typeparam>
public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
        public static T Instance { get; private set; }
    protected virtual void Awake(){
        if(Instance != null)
            Destroy(gameObject);
        Instance = this as T;
    }

    protected virtual void OnApplicationQuit() {
        Instance = null;
        Destroy(gameObject);
    }

    #region NotSupportException
    // private static T m_Instance = null;
    // private new static string name;
    // private static Mutex mutex;//互斥类Mutex
    // public static T Instance
    // {
    //     get
    //     {
    //         if (m_Instance == null)
    //         {
    //             if (IsSingle())
    //             {
    //                 m_Instance = new GameObject(name, typeof(T)).GetComponent<T>();
    //                 m_Instance.Init();
    //             }
    //         }
    //         return m_Instance;
    //     }
    // }

    // // NotSupportException
    // private static bool IsSingle()
    // {
    //     name = "Singleton of " + typeof(T).ToString();
    //     mutex = new Mutex(false, name, out bool createdNew);
    //     mutex.WaitOne();
    //     return createdNew;
    // }

    // private bool m_isDestroy = true;

    // protected virtual void Awake()
    // {
    //     m_isDestroy = true;
    //     if (m_Instance == null)
    //     {
    //         if (IsSingle())
    //         {
    //             m_Instance = this as T;
    //             m_Instance.Init();
    //         }
    //     }
    //     else
    //     {
    //         // Debug.LogError("[Singleton] " + typeof(T).Name + " should never be more than 1 in scene!Owner is “" + transform.name + "”.");
    //         m_isDestroy = false;
    //         Destroy(this);
    //     }
    // }
    // protected abstract void Init();
    // protected abstract void DisInit();
    // private void OnDestroy()
    // {
    //     if (m_Instance != null)
    //     {
    //         if (m_isDestroy)
    //         {
    //             if (null != mutex)
    //             {
    //                 mutex.ReleaseMutex();
    //             }
    //             m_Instance = null;
    //         }
    //         DisInit();
    //     }
    // }
    // private void OnApplicationQuit()
    // {
    //     if (null != mutex)
    //     {
    //         mutex.ReleaseMutex();
    //         mutex.Close();
    //         mutex = null;
    //     }
    // }
    #endregion
}
