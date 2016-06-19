using System;
using System.Linq;
using Elmah.Io.Alerting.Models;

namespace Elmah.Io.Alerting.Channels.Slack
{
    public class SlackChannel : ChannelBase
    {
        private readonly SlackClient _client;
        public SlackChannel(string urlWithAccessToken)
        {
            _client = new SlackClient(urlWithAccessToken);
        }

        public override void Alert(AlertSummary alertSummary)
        {
            _client.PostMessage(GetAlertOpenMessage(alertSummary));
        }

        public override void Close(string policy, int durationMinutes)
        {
            _client.PostMessage(GetAlertClosedMessage(policy, durationMinutes));

        }
    }
}
