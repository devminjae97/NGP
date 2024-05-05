using UnityEngine;
using Object = UnityEngine.Object;

/**
 * 2024.04.18.
 * @조민재, 최초 작성
 */

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static bool _shutdown = false;
    private static object _lock = new Object();

    protected static bool dontDestroy = true;
    
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_shutdown)
            {
                return null;
            }

            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = (T)FindObjectOfType(typeof(T));
                    if (_instance == null)
                    {
                        var go = new GameObject();
                        _instance = go.AddComponent<T>();
                        go.name = typeof(T).ToString() + " (Singleton)";
                        
                        if (dontDestroy)
                        {
                            DontDestroyOnLoad(go);
                        }
                    }
                }
                return _instance;
            }
        }
    }

    #region >>>> Unity

    private void OnApplicationQuit()
    {
        _shutdown = true;
    }

    protected virtual void OnDestroy()
    {
        _shutdown = true;
    }

    #endregion
}