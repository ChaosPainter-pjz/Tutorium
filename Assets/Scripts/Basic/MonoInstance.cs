using UnityEngine;

namespace Basic
{
    public abstract class MonoInstance<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance { get; private set; }

        public virtual void Awake()
        {
            if (Instance == null)
                Instance = this as T;
            else
                Debug.LogError(gameObject.name + "非单例异常");
        }
    }
}