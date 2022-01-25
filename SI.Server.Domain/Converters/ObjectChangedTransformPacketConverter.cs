using System;
using SI.Server.Domain.Entities;
using SI.Server.Domain.Interfaces;
using SI.Server.Domain.Packets;

namespace SI.Server.Domain.Converters
{
    public class ObjectChangedTransformPacketConverter : BasicPacketConverter
    {
        public override byte[] ConvertToBytes(Packet packet)
        {
            var transformPacket = packet as ObjectChangedTransformPacket;
            var buffer = GetBasicPacket(packet);

            var bitX = BitConverter.GetBytes(transformPacket.Position.X);
            var bitY = BitConverter.GetBytes(transformPacket.Position.Y);
            var bitZ = BitConverter.GetBytes(transformPacket.Position.Z);
            
            var rotX = BitConverter.GetBytes(transformPacket.Rotation.X);
            var rotY = BitConverter.GetBytes(transformPacket.Rotation.Y);
            var rotZ = BitConverter.GetBytes(transformPacket.Rotation.Z);
            var rotW = BitConverter.GetBytes(transformPacket.Rotation.W);

            // position:
            // x
            buffer[4] = bitX[0];
            buffer[5] = bitX[1];
            buffer[6] = bitX[2];
            buffer[7] = bitX[3];
            // y
            buffer[8] = bitY[0];
            buffer[9] = bitY[1];
            buffer[10] = bitY[2];
            buffer[11] = bitY[3];
            // z
            buffer[12] = bitZ[0];
            buffer[13] = bitZ[1];
            buffer[14] = bitZ[2];
            buffer[15] = bitZ[3];
            
            // rotation:
            // x
            buffer[16] = rotX[0];
            buffer[17] = rotX[1];
            buffer[18] = rotX[2];
            buffer[19] = rotX[3];
            // y
            buffer[20] = rotY[0];
            buffer[21] = rotY[1];
            buffer[22] = rotY[2];
            buffer[23] = rotY[3];
            // z
            buffer[24] = rotZ[0];
            buffer[25] = rotZ[1];
            buffer[26] = rotZ[2];
            buffer[27] = rotZ[3];
            // w
            buffer[28] = rotW[0];
            buffer[29] = rotW[1];
            buffer[30] = rotW[2];
            buffer[31] = rotW[3];

            return buffer;
        }

        public override Packet ConvertToPacket(byte[] binPacket)
        {
            var playerId = BitConverter.ToInt16(binPacket, 0);
            var posX = BitConverter.ToSingle(binPacket, 4);
            var posY = BitConverter.ToSingle(binPacket, 8);
            var posZ = BitConverter.ToSingle(binPacket, 12);
            
            var rotX = BitConverter.ToSingle(binPacket, 16);
            var rotY = BitConverter.ToSingle(binPacket, 20);
            var rotZ = BitConverter.ToSingle(binPacket, 24);
            var rotW = BitConverter.ToSingle(binPacket, 28);
            
            var packet = 
                new ObjectChangedTransformPacket(playerId, 
                    new Vector3(posX, posY, posZ), new Quaternion(rotX, rotY, rotZ, rotW));
            return packet;
        }
    }
}