using System.Net;
using Contracts;
using SI.Server.Domain;
using SI.Server.Domain.Enums;
using SI.Server.Domain.Packets;

namespace SI.Server.Application.Handlers
{
    public class ObjectChangedTransformPacketHandler : IPacketHandler
    {
        private readonly GameState _gameState;
        public PacketType PacketType => PacketType.ObjectChangedTransform;

        public ObjectChangedTransformPacketHandler(GameState gameState)
        {
            _gameState = gameState;
        }
        
        public void Handle(Packet packet, IPEndPoint e)
        {
            var objectId = packet.ObjectId;
            var objectTransformChangedPacket = (ObjectChangedTransformPacket) packet;
            
            _gameState.Players[objectId.Value].Position = objectTransformChangedPacket.Position;
            _gameState.Players[objectId.Value].Rotation = objectTransformChangedPacket.Rotation;
        }
    }
}