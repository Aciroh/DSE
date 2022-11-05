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
            while(true)
            {
                Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                IPAddress iPAddress = IPAddress.Parse("255.255.255.255");

                byte[] sendbuf = Encoding.ASCII.GetBytes("Are you the server?");
                IPEndPoint iPEndPoint = new IPEndPoint(iPAddress, broadcastPort);

                s.SendTo(sendbuf, iPEndPoint);

                Console.WriteLine("Message sent");
                
                Thread.Sleep(1000);
            }
            
        }
    }
}
