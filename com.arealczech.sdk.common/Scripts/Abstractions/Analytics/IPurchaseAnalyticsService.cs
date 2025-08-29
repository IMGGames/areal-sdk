namespace Areal.SDK.Common {
    public interface IPurchaseAnalyticsService : IAnalyticsService {
        public void LogPurchaseInitiation(string productId, string isoCurrencyCode, decimal price);
        public void LogPurchase(string productId, string transactionId, string isoCurrencyCode, decimal price);
    }
}
