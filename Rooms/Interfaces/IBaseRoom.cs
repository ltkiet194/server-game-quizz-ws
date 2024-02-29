using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerKaLoop.Rooms.Interfaces
{
    public interface IBaseRoom
    {
        public string Id { get; } 
        public ConcurrentDictionary<string,IPlayer> Players { get; set; }
        bool JoinRoom(IPlayer player);

        bool ExitRoom(IPlayer player);

        bool ExitRoom(string id);

        IPlayer FindPlayer(string id);

        void SendMessage(string mes);

        void SendMessage<T>(WsMessage<T> message);

        void SendMessage<T>(WsMessage<T> message ,string idIgnore);

    }
}
