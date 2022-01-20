namespace SI.Server.Domain.Interfaces
{
    public interface IJob
    {
        public void Run();

        public void Stop();
    }
}