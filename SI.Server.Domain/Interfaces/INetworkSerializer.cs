using SI.Server.Domain.Packets;

namespace SI.Server.Domain.Interfaces
{
    public interface INetworkSerializer
    {
        public byte[] Serialize(Packet obj);

        public byte[] Serialize(Packet[] batch);
        
        public Packet Deserialize(byte[] binaryObj);
    }
}