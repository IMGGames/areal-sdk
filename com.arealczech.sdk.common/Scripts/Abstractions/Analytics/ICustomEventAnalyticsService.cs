using System.Collections.Generic;

namespace Areal.SDK.Common {
    public interface ICustomEventAnalyticsService : IAnalyticsService {
        public void LogCustomEvent(string eventName, params KeyValuePair<string, object>[] parameters);
    }
}
