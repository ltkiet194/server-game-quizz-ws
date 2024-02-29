using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerKaLoop.Applications.Messaging.Constants
{
    public struct JoinRoomData
    {
        public string DisplayName { get; set; }
        public string Avatar { get; set; }
        public string RoomCode { get; set; }
        public string Email { get; set; }

    }
}
