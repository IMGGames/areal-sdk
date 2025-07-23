using System.Collections.Generic;
using System.Linq;
using Areal.SDK.Common;
using UnityEngine;

namespace Areal.SDK {
    public static class Analytics {
        private const string Prefix = "[Analytics Adapter]";

        private static ICustomEventAnalyticsService[] _customEventServices;
        private static ITutorialAnalyticsService[] _tutorialServices;
        private static ILevelUpAnalyticsService[] _levelUpServices;

        public static void Init(params IAnalyticsService[] services) {
            _customEventServices = services.OfType<ICustomEventAnalyticsService>().ToArray();
            _tutorialServices = services.OfType<ITutorialAnalyticsService>().ToArray();
            _levelUpServices = services.OfType<ILevelUpAnalyticsService>().ToArray();
        }

        public static void LogCustomEvent(string eventName, params KeyValuePair<string, object>[] parameters) {
            if (_customEventServices is not { Length: > 0 }) {
                Debug.LogWarning($"{Prefix} {nameof(LogCustomEvent)}: no {nameof(ICustomEventAnalyticsService)} provided");
                return;
            }

            foreach (var service in _customEventServices) {
                service.LogCustomEvent(eventName, parameters);
            }
        }

        public static void LogTutorialStart() {
            if (_tutorialServices is not { Length: > 0 }) {
                Debug.LogWarning($"{Prefix} {nameof(LogTutorialStart)}: no {nameof(ITutorialAnalyticsService)} provided");
                return;
            }

            foreach (var service in _tutorialServices) {
                service.LogTutorialStart();
            }
        }

        public static void LogTutorialStep(int step) {
            if (_tutorialServices is not { Length: > 0 }) {
                Debug.LogWarning($"{Prefix} {nameof(LogTutorialStep)}: no {nameof(ITutorialAnalyticsService)} provided");
                return;
            }

            foreach (var service in _tutorialServices) {
                service.LogTutorialStep(step);
            }
        }

        public static void LogTutorialFinish() {
            if (_tutorialServices is not { Length: > 0 }) {
                Debug.LogWarning($"{Prefix} {nameof(LogTutorialFinish)}: no {nameof(ITutorialAnalyticsService)} provided");
                return;
            }

            foreach (var service in _tutorialServices) {
                service.LogTutorialFinish();
            }
        }

        public static void LogTutorialSkipped() {
            if (_tutorialServices is not { Length: > 0 }) {
                Debug.LogWarning($"{Prefix} {nameof(LogTutorialSkipped)}: no {nameof(ITutorialAnalyticsService)} provided");
                return;
            }


            foreach (var service in _tutorialServices) {
                service.LogTutorialSkipped();
            }
        }

        public static void LogLevelUp(int level) {
            if (_levelUpServices is not { Length: > 0 }) {
                Debug.LogWarning($"{Prefix} {nameof(LogLevelUp)}: no {nameof(ILevelUpAnalyticsService)} provided");
                return;
            }

            foreach (var service in _levelUpServices) {
                service.LogLevelUp(level);
            }
        }
    }
}
