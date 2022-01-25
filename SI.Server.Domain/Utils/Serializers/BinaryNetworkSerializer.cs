using System;
using System.Collections.Generic;
using SI.Server.Domain.Converters;
using SI.Server.Domain.Enums;
using SI.Server.Domain.Interfaces;
using SI.Server.Domain.Packets;

namespace SI.Server.Domain.Utils.Serializers
{
    public class BinaryNetworkSerializer : INetworkSerializer
    {
        private readonly IDictionary<PacketType, BasicPacketConverter> _converters;

        public BinaryNetworkSerializer(IDictionary<PacketType, BasicPacketConverter> converters)
        {
            _converters = converters;
        }
        
        public byte[] Serialize(Packet packet)
        {
            _converters.TryGetValue(packet.Type, out var converter);
            if (converter == null)
                return Array.Empty<byte>();

            var bytes = converter.ConvertToBytes(packet);
            return bytes;
        }

        public byte[] Serialize(Packet[] batch)
        {
            var bufferLength = batch.Length + 2 + 4*batch.Length;
            
            foreach (var packet in batch)
            {
                bufferLength += packet.DataSize;
            }

            var buffer = new byte[bufferLength];
            buffer[0] = (byte)'<';
            buffer[bufferLength-1] = (byte)'>';
            
            int copyIndex = 1;
            for (var i = 0; i < batch.Length; i++)
            {
                var bytes = Serialize(batch[i]);
                try
                {
                    bytes.CopyTo(buffer, copyIndex);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                
                copyIndex += batch[i].DataSize+4;
                buffer[copyIndex] = (byte) ':';
                copyIndex++;
            }

            // batch format:
            // < 27 36 82 63 : . . . : 94 22 21 23 >
            
            return buffer;
        }

        public Packet Deserialize(byte[] binaryObj)
        {
            var type = (PacketType) binaryObj[2];
            _converters.TryGetValue(type, out var converter);
            if (converter == null)
                return null;

            var packet = converter.ConvertToPacket(binaryObj);
            return packet;
        }
    }
}