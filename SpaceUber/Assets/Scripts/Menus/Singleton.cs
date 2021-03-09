/* PauseMenu.cs
 * Frank Calabrese 
 * 2/4/21
 * Turns subclasses into single instance global use classes
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T instance;

    public static T Instance
    {
        get { return instance; }
    }

    public static bool isInitialized
    {
        get { return instance != null; }
    }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("[Singleton] trying to instantiate second instance of a singleton class. Instance destroyed. ");
            Destroy(gameObject);
        }
        else
        {
            instance = (T)this;
        }
    }

    protected virtual void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
}