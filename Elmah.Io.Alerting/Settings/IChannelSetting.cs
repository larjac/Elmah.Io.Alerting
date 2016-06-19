using System;
using System.Collections.Generic;
using Elmah.Io.Alerting.Channels;
using Elmah.Io.Client;

namespace Elmah.Io.Alerting.Settings
{
    public interface IChannelSetting
    {
        IScheduleSetting ToChannels(params ChannelBase[] alertChannel);

    }
}