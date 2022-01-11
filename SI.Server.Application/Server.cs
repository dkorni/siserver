using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using SI.Server.Domain.Converters;
using SI.Server.Domain.Entities;
using SI.Server.Domain.Enums;
using SI.Server.Domain.Packets;

namespace SI.Server.Application
{
    public class Server
    {
        private Queue<int> _availablePlayerIds;

        private Dictionary<int, Player> _players;

        public Server(AsynchronousSocketListener listener)
        {
            listener.MessageHandler = ProcessMessage;
            _availablePlayerIds = new Queue<int>(Enumerable.Range(1, 100).ToArray());
            _players = new Dictionary<int, Player>();
        }

        private void ProcessMessage(byte[] message, UdpClient client, IPEndPoint e)
        {
            if (message[2] == (byte)PacketType.ConnectionRequest)
            {
                var playerId = _availablePlayerIds.Dequeue();
                var packet = PacketProvider.GetPacket<ConnectionRequestPacket>(message);
                packet.ObjectId = playerId;
                var player = new Player(playerId, packet.PlayerName, e, Vector3.Zero, Quaternion.Zero);
                _players[playerId] = player;
                Console.WriteLine("Player {0} joined with address {1}", packet.PlayerName, $"{e.Address}:{e.Port}");
                message = PacketProvider.GetConnectionRequestBytePacket(packet);
                client.SendAsync(message, message.Length, player.Address);
            }
        }
    }
}