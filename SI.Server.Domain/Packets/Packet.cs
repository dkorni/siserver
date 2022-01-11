using System;
using SI.Server.Domain.Enums;

namespace SI.Server.Domain.Packets
{
    public class Packet
    {
        public int? ObjectId { get; set; }

        public virtual PacketType Type { get; }

        public virtual byte DataSize { get; }

        public Packet(int? objectId)
        {
            ObjectId = objectId;
        }
    }
}
