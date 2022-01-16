using SI.Server.Domain.Enums;

namespace SI.Server.Domain.Packets
{
    public class DisconnectPacket : Packet
    {
        public override PacketType Type => PacketType.Disconnect;

        public override byte DataSize => 4;

        public DisconnectPacket(int? objectId) : base(objectId)
        {
        }
    }
}