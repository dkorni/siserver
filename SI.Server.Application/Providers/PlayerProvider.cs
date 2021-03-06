using System.Collections.Generic;
using System.Linq;
using System.Net;
using SI.Server.Domain;
using SI.Server.Domain.Entities;
using SI.Server.Domain.Enums;

namespace SI.Server.Application.Providers
{
    public class PlayerProvider
    {
        private readonly GameState _gameState;
        private Queue<int> _availablePlayerIds;
        
        public PlayerProvider(GameState gameState)
        {
            _gameState = gameState;
            _availablePlayerIds = new Queue<int>(Enumerable.Range(1, 100).ToArray());
        }
        
        public Player AddPlayer(string playerName, Colors color, IPEndPoint e)
        {
            var playerId = _availablePlayerIds.Dequeue();
            var player = new Player(playerId, playerName, e, Vector3.Zero, Quaternion.Zero, color);
            _gameState.Players[playerId] = player;
            return player;
        }

        public Player[] GetPlayers()
        {
            return _gameState.Players.Values.ToArray();
        }

        public void Remove(int playerId)
        {
            _gameState.Players.TryRemove(playerId, out _);
            _availablePlayerIds.Enqueue(playerId);
        }
    }
}