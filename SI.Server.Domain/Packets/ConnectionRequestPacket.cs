using SI.Server.Domain.Constants;
using SI.Server.Domain.Enums;

namespace SI.Server.Domain.Packets
{
    public class ConnectionRequestPacket : Packet
    {
        public override PacketType Type => PacketType.ConnectionRequest;

        public override byte DataSize => (byte)PacketDataSize.ConnectionRequest;
    
        public string PlayerName { get; }
        
        public ConnectionRequestPacket(int? objectId, string playerName) 
            : base(objectId)
        {
            PlayerName = playerName;
        }
    }
}