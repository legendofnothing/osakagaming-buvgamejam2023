using UnityEngine;

namespace Core {
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
        private static T _instance;
        public static T instance {
            get {
                if (_instance == null) {
                    var instances = FindObjectsOfType<T>();
                    _instance = instances.Length > 0 ? 
                        instances[0] : 
                        new GameObject($"[SINGLETON] {typeof(T)}").AddComponent<T>();
                }

                return _instance; 
            }
        }
    }
}
