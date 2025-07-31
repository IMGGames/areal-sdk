using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Areal.SDK.Healer {
    internal class CheckInfo {
        public readonly string DisplayName;
        public readonly Type CheckType;
        public readonly ICheck Instance;

        public string Id => CheckType.FullName;

        public CheckInfo(Type type) {
            CheckType = type;

            DisplayNameAttribute displayNameAttribute = type.GetCustomAttribute<DisplayNameAttribute>();

            if (displayNameAttribute != null) {
                DisplayName = displayNameAttribute.Name;
            } else {
                DisplayName = type.Name;

                if (DisplayName.ToLower().EndsWith("check")) {
                    DisplayName = ObjectNames.NicifyVariableName(DisplayName[..^5]);
                }
            }

            Instance = (ICheck)Activator.CreateInstance(type);
        }

        public static CheckInfo[] GetChecks() {
            Type interfaceType = typeof(ICheck);

            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            List<CheckInfo> checks = new List<CheckInfo>();

            foreach (var assembly in assemblies) {
                try {
                    checks.AddRange(assembly.GetTypes().Where(t =>
                        interfaceType.IsAssignableFrom(t) && t.IsClass && !t.IsAbstract
                    ).Select(e => new CheckInfo(e)));
                }
                catch (ReflectionTypeLoadException e) {
                    Debug.LogWarning($"Reflection error loading types from {assembly.FullName}: {e}");
                }
            }

            return checks.ToArray();
        }
    }
}
