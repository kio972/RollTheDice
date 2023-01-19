using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance = null;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
                GameObject temp = new GameObject(typeof(T).Name, typeof(T));
                instance = temp.GetComponent<T>();
            }
            
            return instance;
        }
    }

    protected virtual void Awake()
    {
        T[] signletons = FindObjectsOfType<T>();
        if(signletons.Length > 1)
            Destroy(gameObject);
        else
        {
            if (transform.parent != null && transform.root != null)
                DontDestroyOnLoad(this.transform.root.gameObject);
            else
                DontDestroyOnLoad(this.gameObject);
        }
    }
}
