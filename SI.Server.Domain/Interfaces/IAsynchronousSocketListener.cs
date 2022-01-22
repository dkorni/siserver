using System;
using System.Net;
using System.Net.Sockets;

namespace SI.Server.Domain.Interfaces
{
    public interface IAsynchronousSocketListener
    {
        public Action<byte[], UdpClient, IPEndPoint> MessageHandler { get; set; }
        public void StartReceiveMessages();
    }
}