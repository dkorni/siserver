using System;
using SI.Server.Domain.Packets;

namespace SI.Server.Domain.Converters
{
    public abstract class BasicPacketConverter
    {
        public abstract byte[] ConvertToBytes(Packet packet);

        public abstract Packet ConvertToPacket(byte[] packet);
        
        protected static byte[] GetBasicPacket(Packet packetModel)
        {
            var packet = new byte[4+packetModel.DataSize];

            if (packetModel.ObjectId.HasValue)
            {
                var bitId = BitConverter.GetBytes(packetModel.ObjectId.Value);
                packet[0] = bitId[0];
                packet[1] = bitId[1];
            }
          
            packet[2] = (byte)packetModel.Type;
            packet[3] = packetModel.DataSize;
            return packet;
        }
    }
}