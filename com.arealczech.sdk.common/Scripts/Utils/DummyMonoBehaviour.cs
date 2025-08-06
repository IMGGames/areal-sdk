using UnityEngine;

namespace Areal.SDK.Common.Utils {
    public class DummyMonoBehaviour : MonoBehaviour {
        public static DummyMonoBehaviour CreateUndestroyableInstance(string name) {
            GameObject go = new GameObject(name);
            DontDestroyOnLoad(go);
            
            return go.AddComponent<DummyMonoBehaviour>();
        }
    }
}
