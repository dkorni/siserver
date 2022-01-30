using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SI.Server.Domain.Interfaces;
using SI.Server.Domain.Packets;

namespace SI.Server.Application
{
    public class UdpSocketService : ISocketService
    {
        public Action<byte[], UdpClient, IPEndPoint> MessageHandler { get; set; }

        private struct UdpState
        {
            public UdpClient UdpClient;
            public IPEndPoint e;
        }

        private bool messageReceived = false;
        
        private HashSet<IPEndPoint> _connections = new HashSet<IPEndPoint>();

        private UdpClient _udpClient;
        
        private readonly INetworkSerializer _networkSerializer;

        public UdpSocketService(INetworkSerializer networkSerializer)
        {
            _networkSerializer = networkSerializer;
        }

        public void StartReceiveMessages()
        {
            // Receive a message and write it to the console.
            IPEndPoint e = new IPEndPoint(IPAddress.Any, 11000);
            _udpClient= new UdpClient(e);

            UdpState s = new UdpState();
            s.e = e;
            s.UdpClient = _udpClient;

            _udpClient.BeginReceive(ReceiveCallback, s);
        }
        
        private void ReceiveCallback(IAsyncResult ar)
        {
            UdpClient u = ((UdpState)(ar.AsyncState)).UdpClient;
            IPEndPoint e = ((UdpState)(ar.AsyncState)).e;

            byte[] receiveBytes = u.EndReceive(ar, ref e);
            
            u.BeginReceive(ReceiveCallback, ar.AsyncState);

            MessageHandler?.Invoke(receiveBytes, u, e);
            messageReceived = true;
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