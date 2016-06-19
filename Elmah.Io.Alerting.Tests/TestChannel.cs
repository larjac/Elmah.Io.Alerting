using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elmah.Io.Alerting.Channels;
using Elmah.Io.Alerting.Models;

namespace Elmah.Io.Alerting.Tests
{
    public class TestChannel : ChannelBase
    {
        public int AlertCount { get; private set; }
        public AlertSummary Summary { get; private set; }

        public override void Alert(AlertSummary alertSummary)
        {
            Summary = alertSummary;
            AlertCount++;
        }

        public override void Close(string policy, int durationMinutes)
        {
            AlertCount = 0;
        }
    }
}
