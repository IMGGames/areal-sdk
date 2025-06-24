using System;
using System.Collections.Generic;
using Areal.SDK.Common;
using DevToDev.Analytics;

namespace Areal.SDK {
    public class DevToDev : ITutorialAnalyticsService, ICustomEventAnalyticsService, ILevelUpAnalyticsService {
        // ReSharper disable UnusedParameter.Local
        public DevToDev(
            string androidAppId = null,
            string iosAppId = null,
            string webAppId = null,
            string winAppId = null,
            string osxAppId = null,
            string uwpAppId = null) {
            // ReSharper restore UnusedParameter.Local
            // ReSharper disable once InlineTemporaryVariable
            string token =
#if UNITY_ANDROID
                androidAppId;
#elif UNITY_IOS
                iosAppId;
#elif UNITY_WEBGL
                webAppId;
#elif UNITY_STANDALONE_WIN
                winAppId;
#elif UNITY_STANDALONE_OSX
                osxAppId;
#elif UNITY_WSA
                uwpAppId;
#else
                null;
#endif

            if (token == null) {
                throw new ArgumentException("No App ID provided for the current platform.");
            }

            DTDAnalytics.Initialize(token);
        }

        public void LogTutorialStart() {
            DTDAnalytics.Tutorial(-1);
        }

        public void LogTutorialStep(int step) {
            DTDAnalytics.Tutorial(step);
        }

        public void LogTutorialFinish() {
            DTDAnalytics.Tutorial(-2);
        }

        public void LogTutorialSkipped() {
            DTDAnalytics.Tutorial(0);
        }

        public void LogCustomEvent(string eventName, params KeyValuePair<string, object>[] parameters) {
            var convertedParameters = new DTDCustomEventParameters();

            foreach (var (key, value) in parameters) {
                switch (value) {
                    case byte or sbyte or short or ushort or int or uint or long or ulong:
                        convertedParameters.Add(key, Convert.ToInt64(value));
                        break;
                    case double or float:
                        convertedParameters.Add(key, Convert.ToDouble(value));
                        break;
                    case bool boolValue:
                        convertedParameters.Add(key, boolValue);
                        break;
                    default:
                        convertedParameters.Add(key, value.ToString());
                        break;
                }
            }

            DTDAnalytics.CustomEvent(eventName, convertedParameters);
        }

        public void LogLevelUp(int level) {
            DTDAnalytics.LevelUp(level);
        }
    }
}
