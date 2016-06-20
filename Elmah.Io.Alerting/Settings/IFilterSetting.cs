using System;
using System.Collections.Generic;
using Elmah.Io.Client;

namespace Elmah.Io.Alerting.Settings
{
    public interface IFilterSetting
    {
        IChannelSetting When(Func<List<Message>, bool> func);
    }

    public interface IFilterSettingHelper : IFilterSetting
    {
        ITimeUnitSetting InPast(int timeUnit);
    }

    public interface ITimeUnitSetting
    {
        IFilterSetting Seconds();
        IFilterSetting Minutes();
        IFilterSetting Hours();
    }
}
