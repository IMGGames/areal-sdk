namespace Areal.SDK.Common {
    public interface ITutorialAnalyticsService : IAnalyticsService {
        public void LogTutorialStart();
        public void LogTutorialStep(int step);
        public void LogTutorialFinish();
        public void LogTutorialSkipped();
    }
}
