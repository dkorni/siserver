using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using SI.Server.Domain.Interfaces;
using SI.Server.Domain.Packets;

namespace SI.Server.Application
{
    public class SocketSender : ISocketSender
    {
        private readonly INetworkSerializer _networkSerializer;
        private HashSet<IPEndPoint> _connections;
        private UdpClient _udpClient;

        public SocketSender(INetworkSerializer networkSerializer)
        {
            _networkSerializer = networkSerializer;
            _udpClient = new UdpClient();
            _connections = new HashSet<IPEndPoint>();
        }

        public void AddConnection(IPEndPoint connection)
        {
            _connections.Add(connection);
        }

        public async Task SendAsync(Packet packet, IPEndPoint e)
        {
            var bytes = _networkSerializer.Serialize(packet);
            await _udpClient.SendAsync(bytes, bytes.Length, e);
        }
        
        public async Task SendAsync(Packet[] batch, IPEndPoint e)
        {
            var bytes = _networkSerializer.Serialize(batch);
            await _udpClient.SendAsync(bytes, bytes.Length, e);
        }

        public void RemoveConnection(IPEndPoint connection)
        {
            _connections.Remove(connection);
        }

        public Task Broadcast(Packet packet)
        {
            var bytes = _networkSerializer.Serialize(packet);
            var tasks =  _connections.
                Select(x => _udpClient.SendAsync(bytes, bytes.Length, x));
            return Task.WhenAll(tasks);
        }
        
        public Task Broadcast(Packet[] batch)
        {
            var bytes = _networkSerializer.Serialize(batch);
            var tasks =  _connections.
                Select(x => _udpClient.SendAsync(bytes, bytes.Length, x));
            return Task.WhenAll(tasks);
        }
    }
}