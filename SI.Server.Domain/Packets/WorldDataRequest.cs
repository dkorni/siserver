using SI.Server.Domain.Enums;

namespace SI.Server.Domain.Packets
{
    public class WorldDataRequest : Packet
    {
        public override PacketType Type => PacketType.WorldDataRequest;
        public override byte DataSize => 4;

        public WorldDataRequest(int? objectId) : base(objectId)
        {
        }
    }
}