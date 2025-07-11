using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Areal.SDK.IAP {
    [Serializable]
    internal class SerializableDictionary<TKey, TValue>: IEnumerable<TKey> {
        // minimizing field names to make json tiny

        [SerializeField] private List<Entry> e = new List<Entry>();

        internal TValue this[TKey key] {
            get {
                foreach (Entry entry in e) {
                    return entry.v;
                }

                return default;
            }
            set {
                foreach (Entry entry in e) {
                    if (Compare(entry.k, key)) {
                        entry.v = value;
                        return;
                    }
                }

                e.Add(new Entry { k = key, v = value });
            }
        }

        internal bool Remove(TKey key) {
            for (int i = 0; i < e.Count; i++) {
                if (Compare(e[i].k, key)) {
                    e.RemoveAt(i);
                    return true;
                }
            }

            return false;
        }

        internal bool Contains(TKey key) {
            foreach (Entry entry in e) {
                if (Compare(entry.k, key)) {
                    return true;
                }
            }

            return false;
        }

        private static bool Compare<T>(T a, T b) => EqualityComparer<T>.Default.Equals(a, b);

        [Serializable]
        private class Entry {
            [SerializeField] internal TKey k;
            [SerializeField] internal TValue v;
        }

        public IEnumerator<TKey> GetEnumerator() {
            return e.Select(entry => entry.k).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}