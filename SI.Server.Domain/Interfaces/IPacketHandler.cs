using System.Net;
using System.Threading.Tasks;
using SI.Server.Domain.Enums;
using SI.Server.Domain.Packets;

namespace Contracts
{
    public interface IPacketHandler
    {
        public PacketType PacketType { get; }
        
        public void Handle(Packet packet, IPEndPoint e);
    }
}