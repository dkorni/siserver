using System;
using System.Threading;
using System.Threading.Tasks;
using SI.Server.Domain;
using SI.Server.Domain.Constants;
using SI.Server.Domain.Converters;
using SI.Server.Domain.Interfaces;
using SI.Server.Domain.Packets;

namespace SI.Server.Application.Jobs
{
    public class WorldStateSendJob : IJob
    {
        private readonly Server _server;
        private readonly GameState _gameState;

        private byte[] _transformBatchBuffer;
        private const int PlayerCount = 100;
        private const int SendDelay = 50;
        private CancellationTokenSource _cancellationToken;
        
        public WorldStateSendJob(Server server, GameState gameState)
        {
            _server = server;
            _gameState = gameState;
            
            // buffer is 'B' + size of transform packet + player max count + 'E' 
            _transformBatchBuffer = new byte[(int)PacketDataSize.ObjectChangedTransform * PlayerCount + 2];
        }

        public void Run()
        {
            _cancellationToken = new CancellationTokenSource();
            Task.Run(async () => await ProcessState(), _cancellationToken.Token);
        }

        public void Stop()
        {
            _cancellationToken.Cancel();
        }

        private async Task ProcessState()
        {
            while (true)
            {
                // mark start of batch
                _transformBatchBuffer[0] = (byte)'B';
                var lastIndex = 1;
                foreach (var player in _gameState.Players.Values)
                {
                    var objectTransformPacket =
                        new ObjectChangedTransformPacket(player.Id, player.Position, player.Rotation);
                    var binaryPacket = PacketProvider.GetObjectChangedPositionBytePacket(objectTransformPacket);
                    for (int i = 0; i < binaryPacket.Length; i++)
                    {
                        lastIndex += i;
                        _transformBatchBuffer[lastIndex] = binaryPacket[i];
                    }
                }

                _transformBatchBuffer[lastIndex + 1] = (byte) 'E';
                await _server.Broadcast(_transformBatchBuffer);
                await Task.Delay(SendDelay);
            }
        }
    }
}