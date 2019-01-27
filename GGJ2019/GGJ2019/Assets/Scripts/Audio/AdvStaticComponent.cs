using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Advanced Static Component.
// 기존의 Static Component는 없으면 Find에서 찾았고 그래도 없으면 터졌는데
// 이건 찾아도 없으면 만든다.
public abstract class AdvStaticComponent<T> : MonoBehaviour where T : AdvStaticComponent<T>
{
    public static T TryLoad() => instance;

    private static T _instance;
    public static T instance
    {
        get
        {
            if (_instance != null)
                return _instance;

            string createMethod = "";
            createMethod.ToString();
            // 만약 인스턴스가 없다면 필드에서 찾아본다.
            var cands = FindObjectsOfType<T>();

            // 필드에서 인스턴스가 1개라면 그걸 사용한다.
            if (cands.Length == 1)
            {
                _instance = cands[0];
                createMethod = "필드에서 찾음";
            }

            // 필드에서 인스턴스가 2개 이상 발견된다면 잘못된 경우이다.
            // 지우라고 에러를 띄워주고 그냥둔다.
            else if (1 < cands.Length)
            {
                for (int i = 1; i < cands.Length; i++)
                    Destroy(cands[i].gameObject);
                _instance = cands[0];
                createMethod = "필드에서 여러개 찾아서 그 중에 하나를 고름";
            }

            // 필드에서 하나도 나오지 않았다면 생성해서 쓴다.
            else // == if (cands.Length == 0)
            {
                var prefab = Resources.Load<GameObject>(typeof(T).Name);
                if (prefab != null)
                {
                    _instance = Instantiate(prefab).GetComponent<T>();
                    createMethod = "Resource.Load로 불러옴";
                }

                // 프리팹도 없으면 아예 맨땅에 만든다.
                else
                {
                    _instance = new GameObject(typeof(T).Name).AddComponent<T>();
                    createMethod = "아예 새로 만듬";
                }
            }

            _instance._Initialize();
            return instance;
        }
    }

    private bool isInitialized = false;
    private void _Initialize()
    {
        if (isInitialized)
            return;
        _instance = (T)this;
        isInitialized = true;
        Initialize();
    }

    protected virtual void Initialize() { }
    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = (T)this;
            _Initialize();
            return;
        }

        if (_instance.gameObject != gameObject)
            Destroy(gameObject);
    }

}