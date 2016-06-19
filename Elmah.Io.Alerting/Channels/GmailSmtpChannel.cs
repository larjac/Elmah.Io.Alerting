using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Elmah.Io.Alerting.Models;

namespace Elmah.Io.Alerting.Channels
{
    public class GmailSmtpChannel : ChannelBase
    {
        private readonly string _username;
        private readonly string _password;
        private readonly string _recipients;

        public GmailSmtpChannel(string username, string password, string recipients)
        {
            _username = username;
            _password = password;
            _recipients = recipients;
        }

        public override void Alert(AlertSummary alertSummary)
        {
            SendEmail($"Error rate alert for policy '{alertSummary.PolicyName}'", GetAlertOpenMessage(alertSummary));
        }

        public override void Close(string policy, int durationMinutes)
        {
            SendEmail(GetAlertClosedMessage(policy, durationMinutes), "");
        }

        protected void SendEmail(string subject, string body)
        {
            using (var client = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential(_username, _password),
                EnableSsl = true
            })
            {
                client.Send("elmahio@mathem.se", _recipients, subject, body);
            }
        }
    }
}
