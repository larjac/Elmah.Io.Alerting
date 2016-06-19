namespace Elmah.Io.Alerting.Settings
{
    public interface IScheduleSetting
    {
        void WithIntervalHours(int hours);
        void WithIntervalMinutes(int minutes);
        void WithIntervalSeconds(int s);
        void RunDailyAt(int hours, int minutes);
    }
}