using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Areal.SDK.Healer {
    internal partial class HealerEditorWindow : EditorWindow {
        private enum Page {
            Checks,
            Settings
        }

        [MenuItem("Areal SDK/Healer")]
        public static void ShowWindow() {
            GetWindow<HealerEditorWindow>(true, "Healer", true);
        }


        private CheckInfo[] _checks;
        private CheckInfo[] _enabledChecks;
        private readonly Dictionary<CheckInfo, KeyValuePair<ICheckResult, bool>[]> _checkMessages = new();

        private HealerSettings _settings;

        private void OnEnable() {
            _settings = new HealerSettings();
            _checks = CheckInfo.GetChecks();
            UpdateEnabledChecks();
        }

        private Page _currentPage;

        private void OnGUI() {
            const int windowPadding = 10;

            GUILayout.BeginHorizontal();
            GUILayout.Space(windowPadding);
            GUILayout.BeginVertical();
            GUILayout.Space(windowPadding);

            DrawNavigationBar();
            GUILayout.Space(5);
            DrawCurrentPage();

            GUILayout.Space(windowPadding);
            GUILayout.EndVertical();
            GUILayout.Space(windowPadding);
            GUILayout.EndHorizontal();
        }

        private void DrawNavigationBar() {
            GUILayout.BeginHorizontal();

            foreach (var page in (Page[])Enum.GetValues(typeof(Page))) {
                bool disabled = _currentPage == page;

                if (disabled) {
                    EditorGUI.BeginDisabledGroup(true);
                }

                if (GUILayout.Button(page.ToString(), GUILayout.Height(30))) {
                    _currentPage = page;
                }

                if (disabled) {
                    EditorGUI.EndDisabledGroup();
                }
            }

            GUILayout.EndHorizontal();
        }

        private void DrawCurrentPage() {
            switch (_currentPage) {
                case Page.Checks:
                    DrawChecksPage();
                    break;
                case Page.Settings:
                    DrawSettingsPage();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void UpdateEnabledChecks() {
            _enabledChecks = _checks.Where(e => !_settings.disabledChecks.Contains(e.Id)).ToArray();
        }
    }
}
