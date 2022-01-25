using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using SI.Server.Domain.Packets;

namespace SI.Server.Domain.Interfaces
{
    public interface ISocketService
    {
        Action<byte[], UdpClient, IPEndPoint> MessageHandler { get; set; }
        void StartReceiveMessages();
        
        void AddConnection(IPEndPoint connection);
        
        Task SendAsync(Packet packet, IPEndPoint e);
        
        Task SendAsync(Packet[] batch, IPEndPoint e);

        void RemoveConnection(IPEndPoint connection);

        Task Broadcast(Packet packet);

        Task Broadcast(Packet[] batch);
    }
}