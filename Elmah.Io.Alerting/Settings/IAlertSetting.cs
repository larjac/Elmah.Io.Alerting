using System;
using System.Collections.Generic;
using Elmah.Io.Alerting.Channels;
using Elmah.Io.Alerting.Models;
using Elmah.Io.Client;

namespace Elmah.Io.Alerting.Settings
{
    public interface IAlertSetting : IQuerySetting, IScheduleSetting, IChannelSetting, IFilterSettingHelper, ITimeUnitSetting
    {
        List<ChannelBase> AlertChannels { get; set; }
        List<string> Logs { get; set; }
        FilterQuery FilterQuery { get; set; }
        Func<List<Message>, bool> WhenFunc { get; set; }

        IQuerySetting ForLogs(params string[] logs);
    }
}
