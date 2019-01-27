using System.Collections;   
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Advanced Singleton Component
// 기존의 Singleton Component는 없으면 Find에서 찾았고 그래도 없으면 터졌는데
// 이건 찾아도 없으면 만든다.
public abstract class AdvSingletonComponent<T> : AdvStaticComponent<T> where T : AdvSingletonComponent<T>
{
    protected static bool isDestroyed { get; private set; }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Initialize()
    {
        base.Initialize();
        DontDestroyOnLoad(gameObject);
    }

    protected virtual void OnDestroy()
    {
        if (!Application.isPlaying)
            return;
        if (instance == this)
            isDestroyed = true;
    }

}