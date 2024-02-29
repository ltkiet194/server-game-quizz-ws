using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerKaLoop.Applications.Messaging.Constants
{
    public struct UserInfo
    {
        public string UserId { get; set; }

        public string DisplayName { get; set; }

        public string Avatar { get; set; }

        public long Score { get; set; }

        public long Amount { get; set; }

    }
}
