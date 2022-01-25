using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SI.Server.Domain;
using SI.Server.Domain.Interfaces;
using SI.Server.Domain.Packets;

namespace SI.Server.Application.Jobs
{
    public class WorldStateSendJob : IJob
    {
        private readonly ISocketService _sender;
        private readonly GameState _gameState;

        private const int PlayerCount = 100;
        private const int SendDelay = 50;
        private CancellationTokenSource _cancellationToken;
        
        public WorldStateSendJob(ISocketService sender, 
            GameState gameState)
        {
            _sender = sender;
            _gameState = gameState;
        }

        public void Run()
        {
            // todo
            _cancellationToken = new CancellationTokenSource();
            Task.Run(async () => await ProcessState());
        }

        public void Stop()
        {
            _cancellationToken.Cancel();
        }

        private async Task ProcessState()
        {
            while (true)
            {
                var batch = _gameState.Players.Values.Select(x =>
                    new ObjectChangedTransformPacket(x.Id, x.Position, x.Rotation)).ToArray();
                await _sender.Broadcast(batch);
                await Task.Delay(SendDelay);
            }
        }
    }
}