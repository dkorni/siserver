using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Network.Packets;
using SI.Server.Application.Extensions;
using SI.Server.Domain.Converters;
using SI.Server.Domain.Entities;
using SI.Server.Domain.Enums;
using SI.Server.Domain.Packets;

namespace SI.Server.Application
{
    public class Server
    {
        private Queue<int> _availablePlayerIds;

        private ConcurrentDictionary<int, Player> _players;

        public Server(AsynchronousSocketListener listener)
        {
            listener.MessageHandler = ProcessMessage;
            _availablePlayerIds = new Queue<int>(Enumerable.Range(1, 100).ToArray());
            _players = new ConcurrentDictionary<int, Player>();
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

                message[2] = (byte) PacketType.PlayerJoined;
                Broadcast(client, message);
            }

            if (message[2] == (byte) PacketType.ObjectChangedTransform)
            {
                var packet = PacketProvider.GetPacket<ObjectChangedTransformPacket>(message);
                var objectId = packet.ObjectId;
                _players[objectId.Value].Position = packet.Position;
                _players[objectId.Value].Rotation = packet.Rotation;
                Broadcast(client, message);
            }

            if (message[2] == (byte) PacketType.WorldDataRequest)
            {
                foreach (var player in _players.Values)
                {
                    // TODO: it's wrong that WorldDataRequest is processed later
                    // than connection request, must be fixed and checking removed
                    if(player.Address.Equals(e))
                        continue;
                    
                    var playerJoinedPacket = new PlayerJoinedPacket(player.Id, player.Name);
                    message = PacketProvider.GetPlayerJoinedPacketBytePacket(playerJoinedPacket);
                    client.SendAsync(message, message.Length, e);
                    var transformPacket = new ObjectChangedTransformPacket(player.Id, player.Position, player.Rotation);
                    var transformMessage = PacketProvider.GetObjectChangedPositionBytePacket(transformPacket);
                    client.SendAsync(transformMessage, transformMessage.Length, e);
                }
            }
        }
        
        private Task Broadcast(UdpClient udpClient, byte[] message)
        {
            var tasks = _players.
                Select(x => udpClient.SendAsync(message, message.Length, x.Value.Address));
            return Task.WhenAll(tasks);
        }
    }
}