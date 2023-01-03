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
        private List<int> ports = new List<int> { 7778 };



        public void StartServer()
        {

            //udpClient.BeginReceive(new AsyncCallback(ListenForBroadcast()));
            //udpClient.ExclusiveAddressUse = false; // only if you want to send/receive on same machine.
            Thread listenBroadcast = new Thread(ListenUDP);
            listenBroadcast.Start();
            Thread listenTCP = new Thread(ListenTCP);
            listenTCP.Start();
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

        public IPAddress GetLocalIP()
        {
            IPAddress ipResult = IPAddress.None;
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    ipResult = ip;
                }
            }
            return ipResult;
        }

        private void ListenTCP()
        {
            //TODO

            int clientCount = 0;
            IPAddress localIP = GetLocalIP();


            while (true)
            {
                while(clientCount == ports.Count())
                {
                    //Do nothing. Wait for more clients
                }
                TcpListener tcpListener = new TcpListener(localIP, ports.Last());
                tcpListener.Start();
                var listenerThread = new Thread(
                        () => CommunicateWithClient(tcpListener));
                listenerThread.Start();
                clientCount++;
            }
        }

        private void CommunicateWithClient(TcpListener tcpListener)
        {
            String data = null;
            Byte[] bytes = new byte[256];
            while (true)
            {
                Console.Write("Waiting for a connection... ");

                // Perform a blocking call to accept requests.
                // You could also use server.AcceptSocket() here.
                using TcpClient client = tcpListener.AcceptTcpClient();
                Console.WriteLine("Connected!");

                data = null;

                // Get a stream object for reading and writing
                NetworkStream stream = client.GetStream();

                int i;

                // Loop to receive all the data sent by the client.
                while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    // Translate data bytes to a ASCII string.
                    data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                    Console.WriteLine("Received: {0}", data);

                    // Process the data sent by the client.
                    data = data.ToUpper();

                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

                    // Send back a response.
                    stream.Write(msg, 0, msg.Length);
                    Console.WriteLine("Sent: {0}", data);
                }
            }
        }


    }
}
