using System.Net;
using System.Threading.Tasks;
using SI.Server.Domain.Packets;

namespace SI.Server.Domain.Interfaces
{
    public interface ISocketSender
    {
        public void AddConnection(IPEndPoint connection);
        
        public Task SendAsync(Packet packet, IPEndPoint e);
        
        public Task SendAsync(Packet[] batch, IPEndPoint e);

        public void RemoveConnection(IPEndPoint connection);

        public Task Broadcast(Packet packet);

        public Task Broadcast(Packet[] batch);
    }
}