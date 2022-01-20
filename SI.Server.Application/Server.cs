using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Network.Packets;
using SI.Server.Application.Jobs;
using SI.Server.Domain;
using SI.Server.Domain.Converters;
using SI.Server.Domain.Entities;
using SI.Server.Domain.Enums;
using SI.Server.Domain.Packets;

namespace SI.Server.Application
{
    public class Server
    {
        private UdpClient _serverClient;
        private Queue<int> _availablePlayerIds;
        private HashSet<IPEndPoint> _connections;
        private GameState _gameState; 

        public Server(AsynchronousSocketListener listener)
        {
            listener.MessageHandler = ProcessMessage;
            _availablePlayerIds = new Queue<int>(Enumerable.Range(1, 100).ToArray());
            _connections = new HashSet<IPEndPoint>();
            _gameState = new GameState();
            var job = new WorldStateSendJob(this, _gameState);
            job.Run();
        }

        private void ProcessMessage(byte[] message, UdpClient client, IPEndPoint e)
        {
            if (_serverClient == null)
                _serverClient = client;
            
            if (message[2] == (byte)PacketType.ConnectionRequest)
            {
                var playerId = _availablePlayerIds.Dequeue();
                var packet = PacketProvider.GetPacket<ConnectionRequestPacket>(message);
                packet.ObjectId = playerId;
                var player = new Player(playerId, packet.PlayerName, e, Vector3.Zero, Quaternion.Zero);
                _gameState.Players[playerId] = player;
                Console.WriteLine("Player {0} joined with address {1}", packet.PlayerName, $"{e.Address}:{e.Port}");
                message = PacketProvider.GetConnectionRequestBytePacket(packet);
                client.SendAsync(message, message.Length, player.Address);

                message[2] = (byte) PacketType.PlayerJoined;
                _connections.Add(e);
                Broadcast(message);
            }

            if (message[2] == (byte) PacketType.ObjectChangedTransform)
            {
                var packet = PacketProvider.GetPacket<ObjectChangedTransformPacket>(message);
                var objectId = packet.ObjectId;
                _gameState.Players[objectId.Value].Position = packet.Position;
                _gameState.Players[objectId.Value].Rotation = packet.Rotation;
            }

            if (message[2] == (byte) PacketType.WorldDataRequest)
            {
                foreach (var player in  _gameState.Players.Values)
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

            if (message[2] == (byte) PacketType.Disconnect)
            {
                var packet = PacketProvider.GetPacket<DisconnectPacket>(message);
                _gameState.Players.Remove(packet.ObjectId.Value, out _);
                _connections.Remove(e);
                Broadcast(message);
            }
        }
        
        public Task Broadcast(byte[] message)
        {
            var tasks =  _gameState.Players.
                Select(x => _serverClient.SendAsync(message, message.Length, x.Value.Address));
            return Task.WhenAll(tasks);
        }
    }
}