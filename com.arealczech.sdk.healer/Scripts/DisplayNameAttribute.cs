using System;

namespace Areal.SDK.Healer {
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class DisplayNameAttribute : Attribute {
        public readonly string Name;

        public DisplayNameAttribute(string name) {
            Name = name;
        }
    }
}
