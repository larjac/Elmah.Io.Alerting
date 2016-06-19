using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elmah.Io.Client;

namespace Elmah.Io.Alerting.Models
{
    public class AlertSummary
    {
        public string LogId { get; set; }
        public IDictionary<string, int> MostFrequent { get; set; }
        public string FirstErrorTimeDiffString { get; set; }
        public int TotalCount { get; set; }
        public IDictionary<Severity?, int> SeverityCount { get; set; }
        public string PolicyName { get; set; }

        public AlertSummary()
        {
            MostFrequent = new Dictionary<string, int>();
            SeverityCount = new Dictionary<Severity?, int>();
        }
    }
}
