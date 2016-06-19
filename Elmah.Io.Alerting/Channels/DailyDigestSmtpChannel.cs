using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elmah.Io.Alerting.Models;

namespace Elmah.Io.Alerting.Channels
{
    public class DailyDigestSmtpChannel : GmailSmtpChannel
    {
        public DailyDigestSmtpChannel(string username, string password, string recipients) : base(username, password, recipients)
        {
        }

        public override void Alert(AlertSummary alertSummary)
        {
            SendEmail("Daily Elmah summary",
                $"There were {alertSummary.TotalCount} new errors in the last 24 hours.{Environment.NewLine}https://elmah.io/errorlog/search/?logId={alertSummary.LogId}#searchTab");
        }
    }
}
