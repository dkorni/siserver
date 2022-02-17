using System;
using System.Linq;
using System.Text;
using SI.Server.Domain.Enums;
using SI.Server.Domain.Interfaces;
using SI.Server.Domain.Packets;

namespace SI.Server.Domain.Converters
{
    public class ConnectionRequestPacketConverter : BasicPacketConverter
    {
        public override byte[] ConvertToBytes(Packet packet)
        {
            var packetModel = packet as ConnectionRequestPacket;
            var buffer = GetBasicPacket(packetModel);
            
            for (int i = 0; i < packetModel.PlayerName.Length; i++)
            {
                buffer[i + 4] = (byte)packetModel.PlayerName[i];
            }
            buffer[20] = (byte) packetModel.Color;
            return buffer;
        }

        public override Packet ConvertToPacket(byte[] binPacket)
        {
            var playerId  = BitConverter.ToInt16(binPacket, 0);
            var nameBuffer = new byte[16];
            Array.Copy(binPacket, 4, nameBuffer, 0, 16);
            var playerName = Encoding.ASCII.GetString(nameBuffer);
            var color = (Colors) binPacket[20];
            var packet = new ConnectionRequestPacket(playerId, playerName,color);
            return packet;
        }
    }
}