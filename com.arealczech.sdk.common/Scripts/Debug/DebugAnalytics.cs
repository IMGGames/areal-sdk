using System.Collections.Generic;
using System.Linq;

namespace Areal.SDK.Common.Debug {
    public class DebugAnalytics : ICustomEventAnalyticsService, ITutorialAnalyticsService, ILevelUpAnalyticsService {
        private const string Prefix = "[" + nameof(DebugAnalytics) + "]";
        
        public void LogCustomEvent(string eventName, params KeyValuePair<string, object>[] parameters) {
            string header = $"{Prefix} Custom event: {eventName}";

            if (parameters.Length > 0) {
                header += "\n - " + string.Join("\n - ", parameters.Select(e => $"{e.Key}: {e.Value}"));
            }

            UnityEngine.Debug.Log(header);
        }

        public void LogTutorialStart() {
            UnityEngine.Debug.Log($"{Prefix} Tutorial started");
        }

        public void LogTutorialStep(int step) {
            UnityEngine.Debug.Log($"{Prefix} Tutorial step: {step}");
        }

        public void LogTutorialFinish() {
            UnityEngine.Debug.Log($"{Prefix} Tutorial finished");
        }

        public void LogTutorialSkipped() {
            UnityEngine.Debug.Log($"{Prefix} Tutorial skipped");
        }

        public void LogLevelUp(int level) {
            UnityEngine.Debug.Log($"{Prefix} Level up: {level}");
        }
    }
}
