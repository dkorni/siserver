using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using ConsoleApp1.IoC;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Core;
using SI.Server.Application;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("logs/server.txt")
                .CreateLogger();

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            
            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.RegitserMainModule();
            var provider = serviceCollection.BuildServiceProvider();
            var server = provider.GetRequiredService<Server>();
            Log.Logger.Information("SI Server started and listen incoming packets.");
            Console.ReadLine();
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Log.Logger.Fatal((Exception)e.ExceptionObject, "Unhandled exception:");
        }
    }
}
