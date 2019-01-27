using System;
using UnityEngine;

public abstract class SingletonComponent<T> : MonoBehaviour where T : SingletonComponent<T>{

    protected virtual void Initialize() { Debug.Log("Init " + typeof(T)); }

    protected static T instance_;
    public static T instance
    {
        get
        {
            if (instance_ == null)
            {
                instance_ = FindObjectOfType<T>();
                if (instance_ == null)
                    throw new System.Exception("Can't Find " + typeof(T));
                Init(instance_);
            }
            return instance_;
        }
    }

    private static T1 FindObjectOfType<T1>()
    {
        throw new NotImplementedException();
    }

    protected virtual void Awake()
    {
        if (instance_ == null)
            Init(GetComponent<T>());

        if (instance_.gameObject != gameObject)
            Destroy(gameObject);
    }

    protected static void Init(T t)
    {
        Debug.Log(t.gameObject.name + " : Init.. " + t.GetInstanceID());
        instance_ = t;
        instance_.Initialize();
        DontDestroyOnLoad(t);
    }

    protected static void CheckInstance()
    {
        if (instance == null) ;
    }
}
