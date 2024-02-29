
using System.Collections.Concurrent;

public interface IPlayerManager 
{
    ConcurrentDictionary <string, IPlayer> Players {get;set;} 

    void AddPlayer(IPlayer player);
    void RemovePlayer(string id);

    void RemovePlayers(IPlayer player);

    IPlayer FindPlayer(string id);

    IPlayer FindPlayer(IPlayer player);

    List<IPlayer> GetPlayers();
}