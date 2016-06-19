using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elmah.Io.Alerting.Channels;
using Elmah.Io.Alerting.Client;
using Elmah.Io.Alerting.Models;
using RestSharp;

namespace Elmah.Io.Alerting.Alerting
{
    public class AlertRunner
    {
        private readonly IApiClient _apiClient;
        private readonly int _realertAfterMinutes;

        public AlertRunner(IApiClient apiClient, int realertAfterMinutes = 60)
        {
            _apiClient = apiClient;
            _realertAfterMinutes = realertAfterMinutes;
        }

        public void Run(AlertPolicy alertPolicy)
        {
            foreach (var logId in alertPolicy.Logs)
            {
                var elmahResponse = _apiClient.GetMessages(logId, alertPolicy.FilterQuery);
                
                var alertSummary = CreateAlertSummary(elmahResponse, alertPolicy.PolicyName, logId);

                SetTimeDiffString(elmahResponse, alertSummary);

                if (alertPolicy.WhenFunc(elmahResponse.Messages))
                {
                    if (alertPolicy.AlertStarted == null || (DateTime.Now - alertPolicy.AlertStarted).Value.TotalMinutes > _realertAfterMinutes)
                    {
                        alertPolicy.AlertStarted = DateTime.Now;
                        alertPolicy.AlertChannels.ForEach(p => p.Alert(alertSummary));
                    }
                }
                else
                {
                    if (alertPolicy.AlertStarted != null)
                    {
                        alertPolicy.AlertChannels.ForEach(
                            p => p.Close(alertPolicy.PolicyName, (int) (DateTime.Now - alertPolicy.AlertStarted.Value).TotalMinutes));
                        alertPolicy.AlertStarted = null;
                    }

                }
            }
        }

        private static void SetTimeDiffString(ElmahIoResponse elmahResponse, AlertSummary alertSummary)
        {
            if (elmahResponse.Messages.Any())
            {
                var timeDiff = (DateTime.UtcNow - elmahResponse.Messages.Min(p => p.DateTime));
                if (timeDiff.TotalSeconds < 60)
                {
                    alertSummary.FirstErrorTimeDiffString = (int) timeDiff.TotalSeconds + " seconds";
                }
                else
                {
                    alertSummary.FirstErrorTimeDiffString =
                        (int) (DateTime.UtcNow - elmahResponse.Messages.Min(p => p.DateTime)).TotalMinutes +
                        " minutes";
                }
            }
        }

        private static AlertSummary CreateAlertSummary(ElmahIoResponse elmahResponse, string policyName, string logId)
        {
            var alertSummary = new AlertSummary
            {
                TotalCount = elmahResponse.Total,
                PolicyName = policyName,
                LogId = logId,
                MostFrequent = elmahResponse.Messages.GroupBy(p => p.Type)
                    .OrderByDescending(p => p.Count()).Take(5)
                    .ToDictionary(p => p.Key, p => p.Count()),
                SeverityCount = elmahResponse.Messages.GroupBy(p => p.Severity)
                    .OrderByDescending(p => p.Count())
                    .ToDictionary(p => p.Key, p => p.Count())
            };
            return alertSummary;
        }
    }
}
