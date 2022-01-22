using System;
using System.Net;
using Contracts;
using Network.Packets;
using SI.Server.Application.Providers;
using SI.Server.Domain;
using SI.Server.Domain.Enums;
using SI.Server.Domain.Interfaces;
using SI.Server.Domain.Packets;
using SI.Server.Domain.Utils.Serializers;

namespace SI.Server.Application.Handlers
{
    public class ConnectionRequestHandler : IPacketHandler
    {
        private readonly ISocketSender _socketSender;
        private readonly PlayerProvider _playerProvider;
        private readonly BinaryNetworkSerializer _serializer;
        private readonly GameState _gameState;
        public PacketType PacketType => PacketType.ConnectionRequest;

        public ConnectionRequestHandler(ISocketSender socketSender, PlayerProvider playerProvider)
        {
            _socketSender = socketSender;
            _playerProvider = playerProvider;
        }
        
        public void Handle(Packet packet, IPEndPoint e)
        {
            var connectionPacket = packet as ConnectionRequestPacket;
            Console.WriteLine("Player {0} joined with address {1}", connectionPacket!.PlayerName, 
                $"{e.Address}:{e.Port}");

            var player = _playerProvider.AddPlayer(connectionPacket.PlayerName, e);
            connectionPacket.ObjectId = player.Id;
            _socketSender.AddConnection(player.Address);
            _socketSender.SendAsync(connectionPacket, player.Address);

            var playerJoinedPacket = new PlayerJoinedPacket(player.Id, player.Name);

            _socketSender.Broadcast(playerJoinedPacket);
        }
    }
}