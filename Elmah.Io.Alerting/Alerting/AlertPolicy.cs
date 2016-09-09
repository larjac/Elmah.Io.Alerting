using System;
using System.Collections.Generic;
using System.Linq;
using Elmah.Io.Alerting.Channels;
using Elmah.Io.Alerting.Client;
using Elmah.Io.Alerting.Models;
using Elmah.Io.Alerting.Settings;
using Elmah.Io.Client;
using FluentScheduler;

namespace Elmah.Io.Alerting.Alerting
{
    public class AlertPolicy : IAlertSetting
    {
        public DateTime? AlertStarted { get; set; }

        public List<ChannelBase> AlertChannels { get;  set; }
        public List<string> Logs { get;  set; }
        public FilterQuery FilterQuery { get; set; } //TODO - allow for several queries?
        public Func<List<Message>, bool> WhenFunc { get; set; }
        public string PolicyName { get; set; }
        private readonly AlertRunner _alertRunner;
        private AlertPolicy(AlertRunner alertRunner)
        {
            _alertRunner = alertRunner;
        }

        public static IAlertSetting Create(string policyName, IApiClient client = null, int realertAfterMinutes = 60)
        {
            var instance = new AlertPolicy(new AlertRunner(client ?? new ElmahIoApiClient(), realertAfterMinutes)) { PolicyName = policyName };
            return instance;
        }
        public IQuerySetting ForLogs(params string[] logs)
        {
            Logs = logs.ToList();
            return this;
        }

        public IFilterSetting WithQuery(FilterQuery filterQuery)
        {
            FilterQuery = filterQuery;
            return this;
        }

        public IFilterSetting WithQuery(string query = "", Func<DateTime> @from = null, Func<DateTime> to = null)
        {
            return WithQuery(new FilterQuery { Query = query, From = @from?.Invoke(), To = to?.Invoke() });
        }

        public IChannelSetting When(Func<List<Message>, bool> func)
        {
            WhenFunc = func;
            return this;
        }


        public IScheduleSetting ToChannels(params ChannelBase[] alertChannels)
        {
            AlertChannels = alertChannels.ToList();
            return this;
        }

        public void WithIntervalHours(int hours)
        {
            JobManager.AddJob(() => _alertRunner.Run(this), s => s.ToRunNow().AndEvery(hours).Hours());
        }

        public void WithIntervalMinutes(int minutes)
        {
            JobManager.AddJob(() => _alertRunner.Run(this), s => s.ToRunNow().AndEvery(minutes).Minutes());
        }

        public void WithIntervalSeconds(int seconds)
        {
            JobManager.AddJob(() => _alertRunner.Run(this), s => s.ToRunNow().AndEvery(seconds).Seconds());
        }

        public void RunDailyAt(int hours, int minutes)
        {
            JobManager.AddJob(() => _alertRunner.Run(this), s => s.ToRunEvery(1).Days().At(hours, minutes));
        }

    }
}
