using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elmah.Io.Alerting.Models;

namespace Elmah.Io.Alerting.Channels
{
    public abstract class ChannelBase
    {
        protected virtual string GetAlertOpenMessage(AlertSummary alertSummary)
        {
            return $"Error rate for alert policy `{alertSummary.PolicyName}` is higher than normal with 20 logs in the last {alertSummary.FirstErrorTimeDiffString}. https://elmah.io/errorlog/search/?logId={alertSummary.LogId}#searchTab{Environment.NewLine}{Environment.NewLine}Frequent errors:```{Environment.NewLine}{string.Join(Environment.NewLine, alertSummary.MostFrequent.Select(p => p.Value + " x " + p.Key))}```";

        }

        protected virtual string GetAlertClosedMessage(string policyName, int durationMinutes)
        {
            return $"Alert for policy `{policyName}` closed after {durationMinutes} minutes";
        }

        public abstract void Alert(AlertSummary alertSummary);

        public abstract void Close(string policy, int durationMinutes);
    }
}
