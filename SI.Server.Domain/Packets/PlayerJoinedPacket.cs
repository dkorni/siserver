using SI.Server.Domain.Constants;
using SI.Server.Domain.Enums;
using SI.Server.Domain.Packets;

namespace Network.Packets
{
    public class PlayerJoinedPacket : Packet
    {
        public override PacketType Type => PacketType.PlayerJoined;

        public override byte DataSize => (byte)PacketDataSize.ConnectionRequest;
    
        public string PlayerName { get; }
        
        public PlayerJoinedPacket(int? objectId, string playerName) 
            : base(objectId)
        {
            PlayerName = playerName;
        }
    }
}