using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerKaLoop.Applications.Messaging.Constants
{
    public struct CreateRoomData
    {
        public string RoomName { get; set; }
        public string RoomCode { get; set; }
        public string QuizzesId { get; set; }
            
    }
}
