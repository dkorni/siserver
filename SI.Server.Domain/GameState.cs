using System.Collections.Concurrent;
using SI.Server.Domain.Entities;

namespace SI.Server.Domain
{
    public class GameState
    {
        public ConcurrentDictionary<int, Player> Players { get; }

        public GameState()
        {
            Players = new ConcurrentDictionary<int, Player>();
        }
    }
}