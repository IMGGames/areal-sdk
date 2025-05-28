using System.Collections.Generic;
using System.Linq;
using Areal.SDK.Common;

namespace Areal.SDK {
    public static class Analytics {
        private static ICustomEventAnalyticsService[] _customEventServices;
        private static ITutorialAnalyticsService[] _tutorialServices;
        private static ILevelUpAnalyticsService[] _levelUpServices;

        public static void Init(params IAnalyticsService[] services) {
            _customEventServices = services.OfType<ICustomEventAnalyticsService>().ToArray();
            _tutorialServices = services.OfType<ITutorialAnalyticsService>().ToArray();
            _levelUpServices = services.OfType<ILevelUpAnalyticsService>().ToArray();
        }

        public static void LogCustomEvent(string eventName, params KeyValuePair<string, object>[] parameters) {
            foreach (var service in _customEventServices) {
                service.LogCustomEvent(eventName, parameters);
            }
        }

        public static void LogTutorialStart() {
            foreach (var service in _tutorialServices) {
                service.LogTutorialStart();
            }
        }

        public static void LogTutorialStep(int step) {
            foreach (var service in _tutorialServices) {
                service.LogTutorialStep(step);
            }
        }

        public static void LogTutorialFinish() {
            foreach (var service in _tutorialServices) {
                service.LogTutorialFinish();
            }
        }

        public static void LogTutorialSkipped() {
            foreach (var service in _tutorialServices) {
                service.LogTutorialSkipped();
            }
        }

        public static void LogLevelUp(int level) {
            foreach (var service in _levelUpServices) {
                service.LogLevelUp(level);
            }
        }
    }
}
