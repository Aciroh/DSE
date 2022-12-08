using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Server
{
    internal class Connection
    {
        public Connection(int listenPort, int streamPort)
        {
            this.listenPort = listenPort;
            this.streamPort = streamPort;
            if (CheckForRunningServers())
            {
                StartServer();
            }
        }

        private int listenPort;
        private int streamPort;
        private List<int> ports = new List<int> { 7777 };



        public void StartServer()
        {

            //udpClient.BeginReceive(new AsyncCallback(ListenForBroadcast()));
            //udpClient.ExclusiveAddressUse = false; // only if you want to send/receive on same machine.
            Thread listenBroadcast = new Thread(ListenUDP);
            listenBroadcast.Start();
        }

        internal bool CheckForRunningServers()
        {
            //This must be ran first, to check if there is another server on the network. If there is, cancel the execution and return exception or whatever
            //TODO
            return true;
        }

        public void ListenUDP()
        {
            IPEndPoint broadcastAddress = new IPEndPoint(IPAddress.Any, listenPort);
            UdpClient udpClient = new UdpClient();
            udpClient.Client.Bind(broadcastAddress);
            while (true)
            {
                Console.WriteLine($"Waiting for broadcast on {broadcastAddress}");
                byte[] bytes = udpClient.Receive(ref broadcastAddress);

                Console.WriteLine($"Received broadcast from {broadcastAddress} :");
                Console.WriteLine($" {Encoding.ASCII.GetString(bytes, 0, bytes.Length)}");
                String port = ports.Last().ToString();
                ports.Add(ports.Last() + 1);
                udpClient.Send(Encoding.ASCII.GetBytes(port), port.Length, broadcastAddress);
            }
        }

        public string GetLocalIP()
        {
            string ipResult = "0";
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    ipResult = ip.ToString();
                }
            }
            return ipResult;
        }

        private void ListenTCP()
        {
            //TODO
        }


    }
}
