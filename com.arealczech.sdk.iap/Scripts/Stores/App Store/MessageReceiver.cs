using UnityEngine;

namespace Areal.SDK.IAP.Stores {
    internal class MessageReceiver : MonoBehaviour {
        internal void Initialize() {
            Debug.Log("test");
            DontDestroyOnLoad(this);
        }
    }
}