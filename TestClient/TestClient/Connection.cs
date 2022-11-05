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
            FindServerViaBroadcast();
        }
        int broadcastPort;
        int streamPort;
        string serverIP;

        void FindServerViaBroadcast()
        {
            var udpClient = new UdpClient();
            var requestData = Encoding.ASCII.GetBytes("Are you the server?");
            var serverEndPoint = new IPEndPoint(IPAddress.Any, 0);
            udpClient.EnableBroadcast = true;
            udpClient.Client.ReceiveTimeout = 100;
            while (true)
            {
                udpClient.Send(requestData, requestData.Length, new IPEndPoint(IPAddress.Broadcast, broadcastPort));
                Console.WriteLine("Sent message to: " + serverEndPoint.Address + " Port: " + serverEndPoint.Port);
                try
                {
                    var serverResponseData = udpClient.Receive(ref serverEndPoint);
                    var serverResponse = Encoding.ASCII.GetString(serverResponseData);
                    Console.WriteLine("Received " + serverResponse + " from " + serverEndPoint.Address.ToString());
                    if (serverResponse == "Yes, this is the server.")
                    {
                        serverIP = serverEndPoint.Address.ToString();
                        return;
                    }
                }
                catch 
                { 
                
                }
                
                Thread.Sleep(1000);
            }
        }

        private void ConnectTCP()
        {
            //TODO
        }
    }
}
