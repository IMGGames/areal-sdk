namespace Areal.SDK.Common {
    public interface IProgressionAnalyticsService : IAnalyticsService {
        void LogStartProgression(string eventName, string source);
        void LogFinishProgression(string eventName, bool successful = true);
    }
}
