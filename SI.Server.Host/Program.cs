using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using SI.Server.Application;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Listening for messages");
            var asyncSocketListener = new AsynchronousSocketListener();
            var server = new Server(asyncSocketListener);
            asyncSocketListener.StartReceiveMessages();
        }
    }
}
