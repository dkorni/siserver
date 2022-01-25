using System.Net;
using System.Threading.Tasks;
using SI.Server.Domain.Enums;
using SI.Server.Domain.Packets;

namespace Contracts
{
    public interface IPacketHandler
    {
        PacketType PacketType { get; }
        
        void Handle(Packet packet, IPEndPoint e);
    }
}