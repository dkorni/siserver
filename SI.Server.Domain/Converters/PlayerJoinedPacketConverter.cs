using System;
using System.Linq;
using System.Text;
using Network.Packets;
using SI.Server.Domain.Interfaces;
using SI.Server.Domain.Packets;

namespace SI.Server.Domain.Converters
{
    public class PlayerJoinedPacketConverter : BasicPacketConverter
    {
        public override byte[] ConvertToBytes(Packet packet)
        {
            var packetModel = packet as PlayerJoinedPacket;
            var buffer = GetBasicPacket(packet);
            
            for (int i = 0; i < packetModel.PlayerName.Length; i++)
            {
                buffer[i + 4] = (byte)packetModel.PlayerName[i];
            }
            return buffer;
        }

        public override PlayerJoinedPacket ConvertToPacket(byte[] binPacket)
        {
            var playerId = BitConverter.ToInt16(binPacket, 0);
            var playerName = Encoding.UTF8.GetString(binPacket.Skip(4).ToArray()).Trim('\0');;
            var packet = new PlayerJoinedPacket(playerId, playerName);
            return packet;
        }
    }
}