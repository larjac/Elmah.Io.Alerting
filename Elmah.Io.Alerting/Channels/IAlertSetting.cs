using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Elmah.Io.Alerting.Models;
using Elmah.Io.Client;

namespace Elmah.Io.Alerting.Channels
{
    public interface IAlertSetting : IQueryFilter, IScheduleFilter, IChannelFilter
    {
        List<ChannelBase> AlertChannels { get; set; }
        List<string> Logs { get; set; }
        FilterQuery FilterQuery { get; set; }
        Func<List<Message>, bool> WhenFunc { get; set; }

        IQueryFilter ForLogs(params string[] logs);
    }
}
