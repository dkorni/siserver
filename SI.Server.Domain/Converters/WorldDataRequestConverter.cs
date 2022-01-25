using System;
using SI.Server.Domain.Interfaces;
using SI.Server.Domain.Packets;

namespace SI.Server.Domain.Converters
{
    public class WorldDataRequestConverter : BasicPacketConverter
    {
        public override byte[] ConvertToBytes(Packet packet)
        {
            var buffer = GetBasicPacket(packet);
            return buffer;
        }

        public override Packet ConvertToPacket(byte[] packet)
        {
            return new WorldDataRequest(null);
        }
    }
}