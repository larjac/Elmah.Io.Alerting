using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Elmah.Io.Client;

namespace Elmah.Io.Alerting.Models
{
    internal class MessageGroup
    {
        public Message Message { get; set; }
        public int Count { get; set; }
    }
}
