using System;
using System.Collections.Generic;
using System.Linq;
using Areal.SDK.Common;
using Areal.SDK.Common.Enums;
using DevToDev.Analytics;

namespace Areal.SDK {
    public class DevToDev : ITutorialAnalyticsService, ICustomEventAnalyticsService, ILevelUpAnalyticsService, IPurchaseAnalyticsService,
        IVirtualCurrencyAnalyticsService {
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

        public void LogPurchaseInitiation(string productId) {
            // ignored
        }

        public void LogPurchase(string productId, string transactionId, string isoCurrencyCode, decimal price) {
            DTDAnalytics.RealCurrencyPayment(transactionId, (double)price, productId, isoCurrencyCode);
        }

        public void LogCurrentBalance(Dictionary<string, long> resources) {
            DTDAnalytics.CurrentBalance(resources);
        }

        public void LogVirtualCurrencyAccrual(string source, AccrualType accrualType, Dictionary<string, long> resources) {
            foreach (var (currency, amount) in resources) {
                DTDAnalytics.CurrencyAccrual(currency, (int)amount, source, ConvertAccrualTypeToDevToDev(accrualType));
            }
        }

        private static DTDAccrualType ConvertAccrualTypeToDevToDev(AccrualType type) {
            return type switch {
                AccrualType.Earned => DTDAccrualType.Earned,
                AccrualType.Bought => DTDAccrualType.Bought,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }

        public void LogVirtualCurrencyPayment(string purchaseId, string purchaseCategory, int purchaseAmount, Dictionary<string, long> resources) {
            DTDAnalytics.VirtualCurrencyPayment(
                purchaseId,
                purchaseCategory,
                purchaseAmount,
                resources.ToDictionary(e => e.Key, e => (int)e.Value)
            );
        }
    }
}
