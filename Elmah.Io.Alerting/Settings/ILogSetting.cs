namespace Elmah.Io.Alerting.Settings
{
    public interface ILogSetting
    {
        IScheduleSetting ForLogs(params string[] logs);
    }
}