using System;
using System.Linq;
using System.Text;
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
            return buffer;
        }

        public override Packet ConvertToPacket(byte[] binPacket)
        {
            var playerId  = BitConverter.ToInt16(binPacket, 0);
            var playerName = Encoding.UTF8.GetString(binPacket.Skip(4).ToArray()).Trim('\0');
            var packet = new ConnectionRequestPacket(playerId, playerName);
            return packet;
        }
    }
}