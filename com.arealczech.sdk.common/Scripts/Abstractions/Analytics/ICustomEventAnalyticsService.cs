namespace Areal.SDK.Common {
    public interface ICustomEventAnalyticsService : IAnalyticsService {
        public void LogCustomEvent(string eventName, params (string key, object value)[] parameters);
    }
}
