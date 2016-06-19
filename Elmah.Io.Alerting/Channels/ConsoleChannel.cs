using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elmah.Io.Alerting.Models;

namespace Elmah.Io.Alerting.Channels
{
    public class ConsoleChannel : ChannelBase
    {
        public override void Alert(AlertSummary alertSummary)
        {
            Console.WriteLine(GetAlertOpenMessage(alertSummary));
        }

        public override void Close(string policy, int durationMinutes)
        {
            Console.WriteLine(GetAlertClosedMessage(policy, durationMinutes));
        }
    }
}
