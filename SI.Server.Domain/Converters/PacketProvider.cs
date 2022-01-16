using System;
using System.Linq;
using System.Text;
using Network.Packets;
using SI.Server.Domain.Entities;
using SI.Server.Domain.Enums;
using SI.Server.Domain.Packets;

namespace SI.Server.Domain.Converters
{
    public class PacketProvider
    {
        public static byte[] GetConnectionRequestBytePacket(ConnectionRequestPacket packetModel)
        {
            var packet = GetBasicPacket(packetModel);
            
            for (int i = 0; i < packetModel.PlayerName.Length; i++)
            {
                packet[i + 4] = (byte)packetModel.PlayerName[i];
            }
            return packet;
        }
        
        public static byte[] GetPlayerJoinedPacketBytePacket(PlayerJoinedPacket packetModel)
        {
            var packet = GetBasicPacket(packetModel);
            
            for (int i = 0; i < packetModel.PlayerName.Length; i++)
            {
                packet[i + 4] = (byte)packetModel.PlayerName[i];
            }
            return packet;
        }

        public static byte[] GetObjectChangedPositionBytePacket(ObjectChangedTransformPacket transformPacket)
        {
            var packet = GetBasicPacket(transformPacket);
            
            var bitX = BitConverter.GetBytes(transformPacket.Position.X);
            var bitY = BitConverter.GetBytes(transformPacket.Position.Y);
            var bitZ = BitConverter.GetBytes(transformPacket.Position.Z);
            
            // TODO: rotation
            
            packet[4] = bitX[0];
            packet[5] = bitX[1];
            packet[6] = bitX[2];
            packet[7] = bitX[3];
            packet[8] = bitY[0];
            packet[9] = bitY[1];
            packet[10] = bitY[2];
            packet[11] = bitY[3];
            packet[12] = bitZ[0];
            packet[13] = bitZ[1];
            packet[14] = bitZ[2];
            packet[15] = bitZ[3];

            return packet;
        }

        public static T GetPacket<T>(byte[] binPacket) where T: Packet
        {
            var type = (PacketType)binPacket[2];
            Packet packet = null;
            int playerId = 0;
            string playerName = "";
            switch (type)
            {
                case PacketType.ConnectionRequest:
                    playerId  = BitConverter.ToInt16(binPacket, 0);
                    playerName = Encoding.UTF8.GetString(binPacket.Skip(4).ToArray()).Trim('\0');
                    packet = new ConnectionRequestPacket(playerId, playerName);
                    break;
                case PacketType.PlayerJoined:
                    playerId = BitConverter.ToInt16(binPacket, 0);
                    playerName = Encoding.UTF8.GetString(binPacket.Skip(4).ToArray()).Trim('\0');;
                    packet = new PlayerJoinedPacket(playerId, playerName);
                    break;
                case PacketType.ObjectChangedTransform:
                    playerId = BitConverter.ToInt16(binPacket, 0);
                    var posX = BitConverter.ToSingle(binPacket, 4);
                    var posY = BitConverter.ToSingle(binPacket, 8);
                    var posZ = BitConverter.ToSingle(binPacket, 12);
                    packet = new ObjectChangedTransformPacket(playerId, new Vector3(posX, posY, posZ), Quaternion.Zero);
                    break;
                case PacketType.Disconnect:
                    playerId = BitConverter.ToInt16(binPacket, 0);
                    packet = new DisconnectPacket(playerId);
                    break;
            }

            return (T)packet;
        }
        
        private static byte[] GetBasicPacket(Packet packetModel)
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
