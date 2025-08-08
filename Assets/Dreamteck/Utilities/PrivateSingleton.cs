namespace Dreamteck
{
    using System.Linq;
    using UnityEngine;

    public class PrivateSingleton<T> : MonoBehaviour where T : Component
    {
        [SerializeField] protected bool _dontDestryOnLoad = true;
        [SerializeField] protected bool _overrideInstance = false;

        protected static T _instance;

        protected virtual void Awake()
        {
            if (_instance != null && _instance != this)
            {
                if (_overrideInstance)
                {
                    Destroy(_instance.gameObject);
                    _instance = this as T;
                    Init();
                }
                else
                {
                    Destroy(this.gameObject);
                }
            }
            else
            {
                _instance = this as T;

                if (_dontDestryOnLoad)
                {
                    DontDestroyOnLoad(gameObject);
                }
                Init();
            }
        }

        protected virtual void Init()
        {
        }

        protected virtual void OnDestroy()
        {
            if (_instance == this && !_overrideInstance)
            {
                _instance = null;
            }
        }
    }
}
