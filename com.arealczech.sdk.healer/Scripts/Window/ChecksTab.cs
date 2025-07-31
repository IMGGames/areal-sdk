using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Areal.SDK.Healer {
    internal partial class HealerEditorWindow {
        private int _selectedCheckIndex;

        private bool _messagesFoldout = true;

        private void DrawChecksPage() {
            const int sidebarMaxWidth = 200;

            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical(GUILayout.MaxWidth(sidebarMaxWidth));

            for (var i = 0; i < _enabledChecks.Length; i++) {
                var check = _enabledChecks[i];

                EditorGUI.BeginDisabledGroup(_selectedCheckIndex == i);

                if (GUILayout.Button(check.DisplayName, GUILayout.MaxWidth(sidebarMaxWidth))) {
                    _selectedCheckIndex = i;
                }

                EditorGUI.EndDisabledGroup();
            }

            GUILayout.EndVertical();
            GUILayout.BeginVertical();

            if (_selectedCheckIndex >= _enabledChecks.Length) {
                _selectedCheckIndex = _enabledChecks.Length - 1;
            }

            if (_selectedCheckIndex >= 0) {
                var check = _enabledChecks[_selectedCheckIndex];

                if (_checkMessages.TryGetValue(check, out var checkResults) && checkResults != null) {
                    Queue<int> unfixedChecks = new Queue<int>();

                    if (checkResults.Length > 0) {
                        _messagesFoldout = EditorGUILayout.Foldout(_messagesFoldout, "Messages", true);
                        
                        for (var i = 0; i < checkResults.Length; i++) {
                            var checkResult = checkResults[i];

                            if (checkResult.Value == false) {
                                unfixedChecks.Enqueue(i);
                            }

                            if (_messagesFoldout) {
                                GUILayout.Label($"{checkResult.Value}: {checkResult.Key.GetMessage()}");
                            }
                        }

                        if (unfixedChecks.Count > 0) {
                            if (GUILayout.Button("Fix all")) {
                                while (unfixedChecks.Count > 0) {
                                    int index = unfixedChecks.Dequeue();

                                    try {
                                        var checkResult = checkResults[index].Key;
                                        check.Instance.Fix(checkResult);
                                        checkResults[index] = new(checkResult, true);
                                    }
                                    catch (Exception e) {
                                        Debug.LogException(e);
                                    }
                                }
                            }
                        }
                    }
                }

                if (GUILayout.Button("Run")) {
                    _checkMessages[check] = check.Instance.Check().Select(e => new KeyValuePair<ICheckResult, bool>(e, false)).ToArray();
                }
            }

            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }
    }
}
