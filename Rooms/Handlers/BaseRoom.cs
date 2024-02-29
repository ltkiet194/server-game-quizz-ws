using ServerKaLoop.Applications.Messaging.Constants;
using ServerKaLoop.GameModels.Handlers;
using ServerKaLoop.Rooms.Constants;
using ServerKaLoop.Rooms.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerKaLoop.Rooms.Handlers
{
    public class BaseRoom : IBaseRoom
    {
        public string Id { get; set; }
        public RoomType RoomType { get; set; }
        public ConcurrentDictionary<string, IPlayer> Players { get; set; }
        public int MaxPlayers { get;  set; }


       

        public BaseRoom(RoomType type) {
            Id = GameHelper.RandomString(10);
            Players = new ConcurrentDictionary<string, IPlayer>();
            RoomType = type;
        }
        public bool ExitRoom(IPlayer player)
        {
            return this.ExitRoom(player.SessionId);
        }

        public IPlayer FindPlayer(string id)
        {
            return Players.FirstOrDefault(x => x.Key.Equals( id)).Value;
        }

        public virtual bool JoinRoom(IPlayer player)
        {
            if(FindPlayer(player.SessionId) == null)
            {
                if(Players.TryAdd(player.SessionId, player))
                {
                    this.RoomInfo();
                    return true;
                }
            }
            return false;
        }
        private void RoomInfo()
        {
            var lobby = new LobbyInfo
            {
                Players = Players.Values.Select(p=>p.GetUserInfo()).ToList(),
            };
            var mess = new WsMessage<LobbyInfo>(WsTags.Lobby, lobby);
            this.SendMessage(mess);

        }

        public bool ExitRoom(string id)
        {
            var player  = FindPlayer(id);
            if(player != null)
            {
                Players.TryRemove(player.SessionId, out player);
                this.RoomInfo();
                return true;
            }
            return false;
        }
        public void SendMessage(string mes)
        {
            lock (Players)
            {
                foreach (var player in Players.Values)
                {
                    player.SendMessage(mes);
                }
            }
        }

        public void SendMessage<T>(WsMessage<T> message)
        {
            lock (Players)
            {
                foreach (var player in Players.Values)
                {
                    player.SendMessage(message);
                }
            }
        }

        public void SendMessage<T>(WsMessage<T> message, string idIgnore)
        {
            lock(Players)
            {
                foreach (var player in Players.Values.Where(p => p.SessionId != idIgnore))
                {
                    if (!player.SessionId.Equals(idIgnore))
                    {
                        player.SendMessage(message);
                    }
                }
            }
        }
    }
}
