using System.Drawing;
using SI.Server.Domain.Constants;
using SI.Server.Domain.Enums;
using SI.Server.Domain.Packets;

namespace Network.Packets
{
    public class PlayerJoinedPacket : Packet
    {
        public override PacketType Type => PacketType.PlayerJoined;

        public override byte DataSize => (byte)PacketDataSize.ConnectionRequest;

        public Colors Color { get; set; }
    
        public string PlayerName { get; }
        
        public PlayerJoinedPacket(int? objectId, string playerName, Colors color) 
            : base(objectId)
        {
            PlayerName = playerName;
            Color = color;
        }
    }
}