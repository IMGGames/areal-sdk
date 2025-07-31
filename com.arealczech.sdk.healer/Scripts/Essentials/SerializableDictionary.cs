using System;
using System.Collections.Generic;
using UnityEngine;

namespace Areal.SDK.Healer.Essentials {
    [Serializable]
    public class SerializableDictionary<TKey, TValue> {
        [SerializeField] private List<SerializableKeyValuePair> e = new List<SerializableKeyValuePair>();

        private readonly EqualityComparer<TKey> _keyComparer = EqualityComparer<TKey>.Default;

        public TValue this[TKey key]
        {
            get
            {
                foreach (var kvp in e) {
                    if (_keyComparer.Equals(kvp.k, key)) {
                        return kvp.v;
                    }
                }

                throw new KeyNotFoundException();
            }
            set
            {
                foreach (var kvp in e) {
                    if (_keyComparer.Equals(kvp.k, key)) {
                        kvp.v = value;
                        return;
                    }
                }

                e.Add(new SerializableKeyValuePair() { k = key, v = value });
            }
        }

        public bool TryGetValue(TKey key, out TValue value) {
            try {
                value = this[key];
                return true;
            }
            catch {
                value = default;
                return false;
            }
        }

        [Serializable]
        private class SerializableKeyValuePair {
            public TKey k;
            public TValue v;
        }
    }
}
