using System.Collections.Generic;
using System.Linq;
using Areal.SDK.Common;
using Areal.SDK.Common.Enums;
using UnityEngine;

namespace Areal.SDK {
    public static class Analytics {
        private const string Prefix = "[Analytics Adapter]";

        private static ICustomEventAnalyticsService[] _customEventServices;
        private static ITutorialAnalyticsService[] _tutorialServices;
        private static ILevelUpAnalyticsService[] _levelUpServices;
        private static IPurchaseAnalyticsService[] _purchaseServices;
        private static IVirtualCurrencyAnalyticsService[] _virtualCurrencyServices;

        public static void Init(params IAnalyticsService[] services) {
            _customEventServices = services.OfType<ICustomEventAnalyticsService>().ToArray();
            _tutorialServices = services.OfType<ITutorialAnalyticsService>().ToArray();
            _levelUpServices = services.OfType<ILevelUpAnalyticsService>().ToArray();
            _purchaseServices = services.OfType<IPurchaseAnalyticsService>().ToArray();
            _virtualCurrencyServices = services.OfType<IVirtualCurrencyAnalyticsService>().ToArray();
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

        public static void LogPurchaseInitiation(string productId) {
            if (_purchaseServices is not { Length: > 0 }) {
                Debug.LogWarning($"{Prefix} {nameof(LogPurchaseInitiation)}: no {nameof(IPurchaseAnalyticsService)} provided");
                return;
            }

            foreach (var service in _purchaseServices) {
                service.LogPurchaseInitiation(productId);
            }
        }

        public static void LogPurchase(string productId, string transactionId, string isoCurrencyCode, decimal price) {
            if (_purchaseServices is not { Length: > 0 }) {
                Debug.LogWarning($"{Prefix} {nameof(LogPurchase)}: no {nameof(IPurchaseAnalyticsService)} provided");
                return;
            }

            foreach (var service in _purchaseServices) {
                service.LogPurchase(productId, transactionId, isoCurrencyCode, price);
            }
        }

        public static void LogCurrentBalance(Dictionary<string, long> resources) {
            if (_purchaseServices is not { Length: > 0 }) {
                Debug.LogWarning($"{Prefix} {nameof(LogCurrentBalance)}: no {nameof(IVirtualCurrencyAnalyticsService)} provided");
                return;
            }

            foreach (var service in _virtualCurrencyServices) {
                service.LogCurrentBalance(resources);
            }
        }

        public static void LogVirtualCurrencyAccrual(string source, AccrualType accrualType, Dictionary<string, long> resources) {
            if (_purchaseServices is not { Length: > 0 }) {
                Debug.LogWarning($"{Prefix} {nameof(LogVirtualCurrencyAccrual)}: no {nameof(IVirtualCurrencyAnalyticsService)} provided");
                return;
            }

            foreach (var service in _virtualCurrencyServices) {
                service.LogVirtualCurrencyAccrual(source, accrualType, resources);
            }
        }

        public static void LogVirtualCurrencyPayment(string purchaseId, string purchaseCategory, int purchaseAmount, Dictionary<string, long> resources) {
            if (_purchaseServices is not { Length: > 0 }) {
                Debug.LogWarning($"{Prefix} {nameof(LogVirtualCurrencyPayment)}: no {nameof(IVirtualCurrencyAnalyticsService)} provided");
                return;
            }

            foreach (var service in _virtualCurrencyServices) {
                service.LogVirtualCurrencyPayment(purchaseId, purchaseCategory, purchaseAmount, resources);
            }
        }
    }
}
