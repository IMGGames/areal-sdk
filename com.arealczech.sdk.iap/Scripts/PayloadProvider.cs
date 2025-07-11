using System;
using System.Linq;
using UnityEngine;

namespace Areal.SDK.IAP {
    internal static class PayloadProvider {
        private const string PlayerPrefsKey = "Areal.SDK.IAP.PayloadProvider";

        private static SerializableDictionary<string, string> _dictionary;

        internal static void Load() {
            if (_dictionary != null) {
                return;
            }

            if (PlayerPrefs.HasKey(PlayerPrefsKey)) {
                try {
                    _dictionary = JsonUtility.FromJson<SerializableDictionary<string, string>>(PlayerPrefs.GetString(PlayerPrefsKey));
                    return;
                }
                catch (Exception e) {
                    Debug.LogError($"Unable to load {PlayerPrefsKey}, falling back to empty: {e}");
                }
            }

            _dictionary = new SerializableDictionary<string, string>();
        }

        internal static void Set(string id, string payload) {
            _dictionary[id] = payload;
            Save();
        }

        internal static string Get(string id) => _dictionary[id];

        internal static void Remove(string id) {
            _dictionary.Remove(id);
            Save();
        }

        public static bool Contains(string productId) => _dictionary.Contains(productId);

        public static void CleanNotPresent(string[] keys) {
            foreach (var key in _dictionary) {
                if (keys.Contains(key)) {
                    continue;
                }

                _dictionary.Remove(key);
            }

            Save();
        }

        private static void Save() => PlayerPrefs.SetString(PlayerPrefsKey, JsonUtility.ToJson(_dictionary));
    }
}