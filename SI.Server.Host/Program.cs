using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using ConsoleApp1.IoC;
using Microsoft.Extensions.DependencyInjection;
using SI.Server.Application;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.RegitserMainModule();
            var provider = serviceCollection.BuildServiceProvider();
            var server = provider.GetRequiredService<Server>();
        }
    }
}
