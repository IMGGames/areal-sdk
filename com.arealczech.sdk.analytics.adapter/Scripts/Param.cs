using System.Collections.Generic;

namespace Areal.SDK {
    /// <summary>
    /// Alias/wrapper around KeyValuePair&lt;string, object&gt;.
    /// </summary>
    public readonly struct Param {
        private readonly KeyValuePair<string, object> _kvp;
        public string Key => _kvp.Key;
        public object Value => _kvp.Value;

        public Param(string key, object value) {
            _kvp = new KeyValuePair<string, object>(key, value);
        }

        public static implicit operator KeyValuePair<string, object>(Param pair) => pair._kvp;

        public override string ToString() => _kvp.ToString();
    }
}
