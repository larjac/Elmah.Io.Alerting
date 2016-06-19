using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elmah.Io.Alerting.Models
{
    public class FilterQuery
    {
        public string Query { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}
