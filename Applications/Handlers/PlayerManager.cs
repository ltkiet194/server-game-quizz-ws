
using System.Collections.Concurrent;
using ServerKaLoop.Logging;

public class PlayerManager : IPlayerManager
{
    public ConcurrentDictionary<string, IPlayer> Players { get; set; }
    private IGameLogger _logger;
    public PlayerManager(IGameLogger logger)
    {
        Players = new ConcurrentDictionary<string, IPlayer>();
        _logger = logger; //_logger
        
    }

    public void AddPlayer(IPlayer player)
    {
        if (FindPlayer(player.SessionId) != null)
        {
            Players.TryAdd(player.SessionId, player);
            _logger.Info($"List Player: {Players.Count}");
        }
    }
    public void RemovePlayer(string id)
    {
        if (FindPlayer(id) != null)
        {
            Players.TryRemove(id, out var player);
            if (player != null)
            {
                _logger.Info($"Remove Player: {player.SessionId} success");
                _logger.Info($"List Player: {Players.Count}");
            }

        }
    }

    public void RemovePlayers(IPlayer player)
    {
        this.RemovePlayer(player.SessionId);
    }
    public IPlayer FindPlayer(string id)
    {
        return Players.FirstOrDefault(p=>p.Value.Equals(id)).Value; 
    }

    public IPlayer FindPlayer(IPlayer player)
    {
        return Players.FirstOrDefault(p=>p.Value.Equals(player)).Value; 
    }
    public List<IPlayer> GetPlayers()
    {
        return Players.Values.ToList();
    }
}