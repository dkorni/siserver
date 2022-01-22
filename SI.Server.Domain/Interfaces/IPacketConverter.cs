using SI.Server.Domain.Packets;

namespace SI.Server.Domain.Interfaces
{
    public interface IPacketConverter<T>
    {
        public byte[] ConvertToBytes(Packet packet);

        public T ConvertToPacket(byte[] packet);
    }
}