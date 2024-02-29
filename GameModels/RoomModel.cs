using ServerKaLoop.GameModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerKaLoop.GameModels
{
    public class RoomModel : BaseModel
    {
       public string RoomCode { get; set; }
        public string RoomName { get; set; }

        public bool isStart { get; set; }
        public bool isEnd { get; set; }
        public bool isFull { get; set; }
        public string QuizzesId { get; set; }
        public bool Online { get; set; }

    }
}
