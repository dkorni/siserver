using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using SI.Server.Domain.Packets;

namespace SI.Server.Domain.Interfaces
{
    public interface ISocketService
    {
        public Action<byte[], UdpClient, IPEndPoint> MessageHandler { get; set; }
        public void StartReceiveMessages();
        
        public void AddConnection(IPEndPoint connection);
        
        public Task SendAsync(Packet packet, IPEndPoint e);
        
        public Task SendAsync(Packet[] batch, IPEndPoint e);

        public void RemoveConnection(IPEndPoint connection);

        public Task Broadcast(Packet packet);

        public Task Broadcast(Packet[] batch);
    }
}