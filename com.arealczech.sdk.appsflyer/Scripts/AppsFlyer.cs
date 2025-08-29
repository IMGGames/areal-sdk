using System;
using System.Collections.Generic;
using System.Globalization;
using Areal.SDK.Common;
using UnityEngine;

namespace Areal.SDK {
    public class AppsFlyer : ICustomEventAnalyticsService, IPurchaseAnalyticsService, ILevelUpAnalyticsService, ILoginAnalyticsService {
        private const string Prefix = "[Areal SDK AppsFlyer]";

        public AppsFlyer(string devKey, string appId, bool debugMode = false) {
            if (devKey == null) {
                throw new ArgumentNullException(nameof(devKey), "Dev Key not provided.");
            }

            if (appId == null) {
                throw new ArgumentNullException(nameof(appId), "App ID not provided.");
            }

            AppsFlyerSDK.AppsFlyer.setIsDebug(debugMode);
            AppsFlyerSDK.AppsFlyer.initSDK(devKey, appId);
            AppsFlyerSDK.AppsFlyer.startSDK();
        }

        public void LogCustomEvent(string eventName, params (string key, object value)[] parameters) {
            Dictionary<string, string> convertedParameters = new Dictionary<string, string>();

            foreach ((string key, object value) in parameters) {
                if (value == null) {
                    Debug.LogWarning($"[{Prefix}] {eventName}: Parameter '{key}' has null value and was skipped");
                    continue;
                }

                switch (value) {
                    case double or float or decimal:
                        convertedParameters.Add(key, Convert.ToDecimal(value).ToString(CultureInfo.InvariantCulture));
                        break;
                    default:
                        convertedParameters.Add(key, value.ToString());
                        break;
                }
            }

            AppsFlyerSDK.AppsFlyer.sendEvent(eventName, convertedParameters);
        }

        public void LogLevelUp(int level) {
            AppsFlyerSDK.AppsFlyer.sendEvent(AFInAppEvents.LEVEL_ACHIEVED, new Dictionary<string, string> {{"af_level", level.ToString()}});
        }

        public void LogPurchaseInitiation(string productId, string isoCurrencyCode, decimal price) {
            AppsFlyerSDK.AppsFlyer.sendEvent(AFInAppEvents.INITIATED_CHECKOUT, new Dictionary<string, string> {
                { AFInAppEvents.CURRENCY, isoCurrencyCode },
                { AFInAppEvents.PRICE, price.ToString(CultureInfo.InvariantCulture) },
                { AFInAppEvents.CONTENT_ID, productId }, 
            });
        }

        public void LogPurchase(string productId, string transactionId, string isoCurrencyCode, decimal price) {
            AppsFlyerSDK.AppsFlyer.sendEvent(AFInAppEvents.PURCHASE, new Dictionary<string, string> {
                { AFInAppEvents.CURRENCY, isoCurrencyCode },
                { AFInAppEvents.REVENUE, price.ToString(CultureInfo.InvariantCulture) },
                { AFInAppEvents.ORDER_ID, transactionId },
                { AFInAppEvents.CONTENT_ID, productId }, 
            });
        }

        public void LogLogin() {
            AppsFlyerSDK.AppsFlyer.sendEvent(AFInAppEvents.LOGIN, new Dictionary<string, string>());
        }
    }
}
