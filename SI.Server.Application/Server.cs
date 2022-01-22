using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Contracts;
using SI.Server.Application.Jobs;
using SI.Server.Domain;
using SI.Server.Domain.Enums;
using SI.Server.Domain.Interfaces;

namespace SI.Server.Application
{
    public class Server
    {
        private INetworkSerializer _networkSerializer;
        private readonly IDictionary<PacketType, IPacketHandler> _handlres;

        public Server(GameState gameState, 
            IAsynchronousSocketListener listener,
            INetworkSerializer networkSerializer,
            ISocketSender socketSender,
            IDictionary<PacketType, IPacketHandler> handlres)
        {
            listener.MessageHandler = ProcessMessage;
            _networkSerializer = networkSerializer;
            _handlres = handlres;

            var job = new WorldStateSendJob(socketSender, gameState);
            job.Run();
        }

        private void ProcessMessage(byte[] message, UdpClient client, IPEndPoint e)
        {
            var packet = _networkSerializer.Deserialize(message);
            _handlres.TryGetValue(packet.Type, out var handler);
            
            if(handler != null)
                handler.Handle(packet, e);
        }
    }
}