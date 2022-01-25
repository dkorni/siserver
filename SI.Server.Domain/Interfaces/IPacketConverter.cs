using SI.Server.Domain.Packets;

namespace SI.Server.Domain.Interfaces
{
    public interface IPacketConverter<T>
    {
        byte[] ConvertToBytes(Packet packet);

        T ConvertToPacket(byte[] packet);
    }
}