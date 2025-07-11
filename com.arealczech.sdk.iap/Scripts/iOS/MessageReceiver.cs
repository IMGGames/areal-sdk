using UnityEngine;

namespace Areal.SDK.IAP {
    internal class MessageReceiver : MonoBehaviour {
        internal void Initialize() {
            Debug.Log("test");
            DontDestroyOnLoad(this);
        }
    }
}