using System;
using System.Collections.Generic;
using Elmah.Io.Alerting.Models;
using Elmah.Io.Client;

namespace Elmah.Io.Alerting.Settings
{
    public interface IQuerySetting : IFilterSetting
    {
        IFilterSetting WithQuery(FilterQuery query);
        IFilterSetting WithQuery(string query = "", DateTime? from = null, DateTime? to = null);
        
    }
}