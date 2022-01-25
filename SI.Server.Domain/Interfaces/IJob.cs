namespace SI.Server.Domain.Interfaces
{
    public interface IJob
    {
        void Run();

        void Stop();
    }
}