using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerKaLoop.Applications.Messaging.Constants
{
    public struct NotifyData
    {
        public string type { get; set; }
        public string title { get; set; }
        public string text { get; set; }
    }
}
