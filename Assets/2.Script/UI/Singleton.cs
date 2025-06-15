using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    private static T s_instance;

    public static T Instance
    {
        get
        {
            if (s_instance != null)
            {
                return s_instance;
            }

            s_instance = GameObject.FindFirstObjectByType<T>();
            if (s_instance == null)
            {
                var go = new GameObject($"{typeof(T).Name}");
                s_instance = go.AddComponent<T>();
            }

            return s_instance;
        }
    }
    public virtual void Awake()
    {
        if (s_instance == null)
        {
            s_instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}