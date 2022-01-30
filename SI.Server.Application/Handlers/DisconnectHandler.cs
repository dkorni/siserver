using System.Net;
using Contracts;
using Serilog;
using SI.Server.Application.Providers;
using SI.Server.Domain.Enums;
using SI.Server.Domain.Interfaces;
using SI.Server.Domain.Packets;

namespace SI.Server.Application.Handlers
{
    public class DisconnectHandler : IPacketHandler
    {
        private readonly ISocketService _socketSender;
        private readonly PlayerProvider _playerProvider;
        public PacketType PacketType => PacketType.Disconnect;

        public DisconnectHandler(ISocketService socketSender, PlayerProvider playerProvider)
        {
            _socketSender = socketSender;
            _playerProvider = playerProvider;
        }
        
        public void Handle(Packet packet, IPEndPoint e)
        {
            var disconnectPacket = packet as DisconnectPacket;
            _playerProvider.Remove(packet.ObjectId.Value);
            Log.Logger.Information("Player with id {0} disconnected", disconnectPacket!.ObjectId);
            _socketSender.RemoveConnection(e);
            _socketSender.Broadcast(disconnectPacket);
        }
    }
}