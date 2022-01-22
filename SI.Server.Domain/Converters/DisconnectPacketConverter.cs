using System;
using SI.Server.Domain.Interfaces;
using SI.Server.Domain.Packets;

namespace SI.Server.Domain.Converters
{
    public class DisconnectPacketConverter : BasicPacketConverter
    {
        public override byte[] ConvertToBytes(Packet packet)
        {
            // TODO: fill method
            return new byte[1];
        }

        public override DisconnectPacket ConvertToPacket(byte[] binPacket)
        {
            var playerId = BitConverter.ToInt16(binPacket, 0);
            var packet = new DisconnectPacket(playerId);
            return packet;
        }
    }
}