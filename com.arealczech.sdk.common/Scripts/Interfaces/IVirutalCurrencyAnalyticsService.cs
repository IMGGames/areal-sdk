using System.Collections.Generic;
using Areal.SDK.Common.Enums;

namespace Areal.SDK.Common {
    public interface IVirtualCurrencyAnalyticsService : IAnalyticsService {
        public void LogCurrentBalance(Dictionary<string, long> resources);
        public void LogVirtualCurrencyAccrual(string source, AccrualType accrualType, Dictionary<string, long> resources);
        public void LogVirtualCurrencyPayment(string purchaseId, string purchaseCategory, int purchaseAmount, Dictionary<string, long> resources);
    }
}
