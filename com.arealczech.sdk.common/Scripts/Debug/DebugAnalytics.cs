using System.Collections.Generic;
using System.Linq;
using Areal.SDK.Common.Enums;

namespace Areal.SDK.Common.Debug {
    public class DebugAnalytics : ICustomEventAnalyticsService, ITutorialAnalyticsService, ILevelUpAnalyticsService, IPurchaseAnalyticsService,
        IVirtualCurrencyAnalyticsService, ILoginAnalyticsService {
        private const string Prefix = "[" + nameof(DebugAnalytics) + "]";

        public void LogCustomEvent(string eventName, params (string key, object value)[] parameters) {
            string header = $"{Prefix} Custom event: {eventName}";

            if (parameters.Length > 0) {
                header += "\n - " + string.Join("\n - ", parameters.Select(e => $"{e.key}: {e.value}"));
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

        public void LogPurchaseInitiation(string productId, string isoCurrencyCode, decimal price) {
            UnityEngine.Debug.Log(
                $"{Prefix} Purchase initiation: " +
                $"productId='{productId}', " +
                $"isoCurrencyCode='{isoCurrencyCode}', " +
                $"price='{price}'"
            );
        }

        public void LogPurchase(string productId, string transactionId, string isoCurrencyCode, decimal price) {
            UnityEngine.Debug.Log(
                $"{Prefix} Purchase: " +
                $"productId='{productId}', " +
                $"transactionId='{transactionId}', " +
                $"isoCurrencyCode='{isoCurrencyCode}', " +
                $"price='{price}'"
            );
        }

        private static string FormatResources(Dictionary<string, long> resources) =>
            "\n - " + string.Join(" - \n", resources.Select(e => $"{e.Key}={e.Value}"));

        public void LogCurrentBalance(Dictionary<string, long> resources) {
            UnityEngine.Debug.Log($"{Prefix} Current balance:{FormatResources(resources)}");
        }

        public void LogVirtualCurrencyAccrual(string source, AccrualType accrualType, Dictionary<string, long> resources) {
            UnityEngine.Debug.Log(
                $"{Prefix} Currency accrual: " +
                $"source='{source}', " +
                $"accrualType='{accrualType}', " +
                $"resources:{FormatResources(resources)}"
            );
        }

        public void LogVirtualCurrencyPayment(string purchaseId, string purchaseCategory, int purchaseAmount, Dictionary<string, long> resources) {
            UnityEngine.Debug.Log(
                $"{Prefix} Virtual currency payment: " +
                $"purchaseId='{purchaseId}', " +
                $"purchaseCategory='{purchaseCategory}', " +
                $"purchaseAmount='{purchaseAmount}', " +
                $"resources:{FormatResources(resources)}"
            );
        }

        public void LogLogin() {
            UnityEngine.Debug.Log($"{Prefix} Login");
        }
    }
}
