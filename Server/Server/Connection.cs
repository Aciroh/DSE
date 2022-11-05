using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

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


        public void StartServer()
        {

            //udpClient.BeginReceive(new AsyncCallback(ListenForBroadcast()));
            //udpClient.ExclusiveAddressUse = false; // only if you want to send/receive on same machine.
            Thread listenThread = new Thread(ListenForBroadcast);
            listenThread.Start();
        }

        internal bool CheckForRunningServers()
        {
            //This must be ran first, to check if there is another server on the network. If there is, cancel the execution and return exception or whatever
            //TODO
            return true;
        }

        public void ListenForBroadcast()
        {
            IPEndPoint broadcastAddress = new IPEndPoint(IPAddress.Any, listenPort);
            UdpClient udpClient = new UdpClient();
            while (true)
            {
                Console.WriteLine("Waiting for broadcast");
                udpClient.Client.Bind(broadcastAddress);
                byte[] bytes = udpClient.Receive(ref broadcastAddress);

                Console.WriteLine("Received broadcast from " + broadcastAddress + " : ");
                Console.WriteLine(Encoding.ASCII.GetString(bytes, 0, bytes.Length));
            }
        }


    }
}
