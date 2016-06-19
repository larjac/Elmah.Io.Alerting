using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elmah.Io.Alerting.Alerting;
using Elmah.Io.Alerting.Client;
using Elmah.Io.Alerting.Models;
using Elmah.Io.Client;
using FluentScheduler;

namespace Elmah.Io.Alerting.Channels
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
        public IQueryFilter ForLogs(params string[] logs)
        {
            Logs = logs.ToList();
            return this;
        }

        public IChannelFilter WithQuery(FilterQuery filterQuery)
        {
            FilterQuery = filterQuery;
            return this;
        }

        public IChannelFilter WithQuery(string query = "", DateTime? @from = null, DateTime? to = null)
        {
            return WithQuery(new FilterQuery { Query = query, From = from, To = to });
        }

        public IChannelFilter When(Func<List<Message>, bool> func)
        {
            WhenFunc = func;
            return this;
        }


        public IScheduleFilter ToChannels(params ChannelBase[] alertChannels)
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

    public interface ILogFilter
    {
        IScheduleFilter ForLogs(params string[] logs);
    }

    public interface IChannelFilter
    {
        IScheduleFilter ToChannels(params ChannelBase[] alertChannel);
        IChannelFilter When(Func<List<Message>, bool> func);
    }


    public interface IQueryFilter
    {
        IChannelFilter WithQuery(FilterQuery query);
        IChannelFilter WithQuery(string query = "", DateTime? from = null, DateTime? to = null);
        IChannelFilter When(Func<List<Message>, bool> func);
    }

    public interface IScheduleFilter
    {
        void WithIntervalHours(int hours);
        void WithIntervalMinutes(int minutes);
        void WithIntervalSeconds(int s);
        void RunDailyAt(int hours, int minutes);
    }

    internal interface IChannelBuilder
    {
    }
}
