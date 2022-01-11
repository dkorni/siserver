using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace SI.Server.Application
{
    public class AsynchronousSocketListener
    {
        public static ManualResetEvent allDone = new ManualResetEvent(false);

        public Action<byte[], UdpClient, IPEndPoint> MessageHandler { get; set; }

        private struct UdpState
        {
            public UdpClient UdpClient;
            public IPEndPoint e;
        }

        private bool messageReceived = false;

        public void StartReceiveMessages()
        {
            // Receive a message and write it to the console.
            IPEndPoint e = new IPEndPoint(IPAddress.Any, 11000);
            UdpClient u = new UdpClient(e);

            UdpState s = new UdpState();
            s.e = e;
            s.UdpClient = u;
            
            u.BeginReceive(ReceiveCallback, s);

            // Do some work while we wait for a message. For this example, we'll just sleep
            Console.ReadLine();
        }
        
        private void ReceiveCallback(IAsyncResult ar)
        {
            UdpClient u = ((UdpState)(ar.AsyncState)).UdpClient;
            IPEndPoint e = ((UdpState)(ar.AsyncState)).e;

            byte[] receiveBytes = u.EndReceive(ar, ref e);
            
            string receiveString = Encoding.ASCII.GetString(receiveBytes);
            u.BeginReceive(ReceiveCallback, ar.AsyncState);

            MessageHandler?.Invoke(receiveBytes, u, e);
            Console.WriteLine($"Received: {receiveString}");
            messageReceived = true;
        }
    }
}