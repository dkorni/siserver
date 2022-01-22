using System;
using SI.Server.Domain.Constants;
using SI.Server.Domain.Entities;
using SI.Server.Domain.Enums;

namespace SI.Server.Domain.Packets
{
    [Serializable]
    public class ObjectChangedTransformPacket : Packet
    {
        public override PacketType Type => PacketType.ObjectChangedTransform;

        public override byte DataSize => (byte)PacketDataSize.ObjectChangedTransform;

        public Vector3 Position { get; }
    
        public Quaternion Rotation { get; }

        public ObjectChangedTransformPacket(int objectId, Vector3 position, 
            Quaternion rotation) :
            base(objectId)
        {
            Position = position;
            Rotation = rotation;
        }
    }
}