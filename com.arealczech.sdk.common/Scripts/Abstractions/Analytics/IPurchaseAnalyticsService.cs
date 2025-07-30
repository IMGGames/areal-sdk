namespace Areal.SDK.Common {
    public interface IPurchaseAnalyticsService : IAnalyticsService {
        public void LogPurchaseInitiation(string productId);
        public void LogPurchase(string productId, string transactionId, string isoCurrencyCode, decimal price);
    }
}
