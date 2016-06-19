using Elmah.Io.Alerting.Models;

namespace Elmah.Io.Alerting.Client
{
    public interface IApiClient
    {
        ElmahIoResponse GetMessages(string logId, FilterQuery query);
        ElmahIoResponse GetLogName(string logId);
    }
}
