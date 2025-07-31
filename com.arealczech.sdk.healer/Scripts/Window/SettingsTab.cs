using UnityEditor;
using UnityEngine;

namespace Areal.SDK.Healer {
    internal partial class HealerEditorWindow {
        private bool _enabledChecksFoldout = true;

        private void DrawSettingsPage() {
            // ReSharper disable once AssignmentInConditionalExpression
            if (_enabledChecksFoldout = EditorGUILayout.Foldout(_enabledChecksFoldout, "Enabled checks", true)) {
                EditorGUI.indentLevel++;

                foreach (CheckInfo check in _checks) {
                    string id = check.Id;

                    bool wasEnabled = !_settings.disabledChecks.Contains(id);

                    GUILayout.BeginHorizontal();
                    GUILayout.Space(15);
                    bool enabled = GUILayout.Toggle(wasEnabled, check.DisplayName);
                    GUILayout.EndHorizontal();

                    if (enabled == wasEnabled) {
                        continue;
                    }

                    if (enabled) {
                        _settings.disabledChecks.Remove(check.Id);
                    } else {
                        _settings.disabledChecks.Add(check.Id);
                    }

                    UpdateEnabledChecks();
                }

                EditorGUI.indentLevel--;
            }
        }
    }
}
