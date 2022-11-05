using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace TestClient
{
    internal class Connection
    {
        public Connection(int broadcastPort, int streamPort)
        {
            this.broadcastPort = broadcastPort;
            this.streamPort = streamPort;
            StartBroadcast();
        }
        int broadcastPort;
        int streamPort;

        void StartBroadcast()
        {

            var Client = new UdpClient();
            var RequestData = Encoding.ASCII.GetBytes("Are you the server?");
            var ServerEp = new IPEndPoint(IPAddress.Any, 0);
            Client.EnableBroadcast = true;
            


            while (true)
            {
                Client.Send(RequestData, RequestData.Length, new IPEndPoint(IPAddress.Broadcast, 7777));
                Console.WriteLine("Sent message to: " + ServerEp.Address + " Port: " + ServerEp.Port);
                Thread.Sleep(1000);
            }
            
        }
    }
}
