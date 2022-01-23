using System.Net;
using Contracts;
using Network.Packets;
using SI.Server.Application.Providers;
using SI.Server.Domain.Enums;
using SI.Server.Domain.Interfaces;
using SI.Server.Domain.Packets;

namespace SI.Server.Application.Handlers
{
    public class WorldDataRequestHandler : IPacketHandler
    {
        private readonly ISocketService _socketSender;
        private readonly PlayerProvider _playerProvider;
        public PacketType PacketType => PacketType.WorldDataRequest;

        public WorldDataRequestHandler(ISocketService socketSender, PlayerProvider playerProvider)
        {
            _socketSender = socketSender;
            _playerProvider = playerProvider;
        }
        
        public void Handle(Packet packet, IPEndPoint e)
        {
            var players = _playerProvider.GetPlayers();
            foreach (var player in  players)
            {
                // TODO: it's wrong that WorldDataRequest is processed later
                // than connection request, must be fixed and checking removed
                if(player.Address.Equals(e))
                    continue;
                    
                var playerJoinedPacket = new PlayerJoinedPacket(player.Id, player.Name);
                _socketSender.SendAsync(playerJoinedPacket, e);
                var transformPacket = new ObjectChangedTransformPacket(player.Id, player.Position, player.Rotation);
                _socketSender.SendAsync(transformPacket, e);
            }
        }
    }
}