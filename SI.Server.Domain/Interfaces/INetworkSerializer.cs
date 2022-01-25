using SI.Server.Domain.Packets;

namespace SI.Server.Domain.Interfaces
{
    public interface INetworkSerializer
    {
        byte[] Serialize(Packet obj);

        byte[] Serialize(Packet[] batch);
        
        Packet Deserialize(byte[] binaryObj);
    }
}