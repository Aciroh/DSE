
using System.Net.Sockets;
using System.Net;
using System.Text;


namespace Server
{
    internal class Connection
    {
        private ConfigGenerator generator;
        private List<TcpClient> tcpClients = new List<TcpClient>();
        private List<Boolean> tcpClientsConfigSent = new List<Boolean>();
        public Connection(int listenPort, int streamPort)
        {
            generator = new ConfigGenerator();
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
                //var listenerThread = new Thread(
                //        () => CommunicateWithClient(tcpListener));
                //listenerThread.Start();
                CommunicateWithClient(tcpListener);
                clientCount++;
            }
        }

        
        //Metoda asta trebuie de fapt sa nu fie pe alt thread, si sa creeze doua thread-uri: Write si Read.
        private void CommunicateWithClient(TcpListener tcpListener)
        {
            Console.WriteLine("Waiting for a connection... ");

            // Perform a blocking call to accept requests.
            // You could also use server.AcceptSocket() here.
            // Aici foloseam using, dar using face dispose cand nu mai avem referinta client
            TcpClient client = tcpListener.AcceptTcpClient();
            addClientToList(client);
            Console.WriteLine("Connected!");
            int i;
            var readerThread = new Thread(
                    () => readFromClient(client));
            readerThread.Start();
            var writerThread = new Thread(
                () => writeToClient(client));
            writerThread.Start();
            // Loop to receive all the data sent by the client.
            // try
            // {
            //     while (true)
            //     {
            //         data = new Byte[256];
            //
            //         // String to store the response ASCII representation.
            //         String responseData = String.Empty;
            //
            //         // Read the first batch of the TcpServer response bytes.
            //         Int32 bytes = stream.Read(data, 0, data.Length);
            //         responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
            //         Console.WriteLine("Received: " + responseData);
            //     }
            // }
            // catch
            // {
            //     removeClientFromList(client);
            // }
        }

        private void readFromClient(TcpClient client)
        {
            // Loop to receive all the data sent by the client.
            try
            {
                while (true)
                {
                    Byte[] data = new Byte[256];
            
                    // String to store the response ASCII representation.
                    String responseData = String.Empty;
            
                    // Get a stream object for reading and writing
                    NetworkStream stream = client.GetStream();
                    // Read the first batch of the TcpServer response bytes.
                    Int32 bytes = stream.Read(data, 0, data.Length);
                    responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                    Console.WriteLine("Received: " + responseData);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception at read from client: " + e.Message);
                removeClientFromList(client);
            }
        }

        private void writeToClient(TcpClient client)
        {
            // Loop to send all the data to the client.
            try
            {
                while (true)
                {
                    Byte[] data = new Byte[256];
            
                    // String to store the response ASCII representation.
                    String responseData = String.Empty;
            
                    // Get a stream object for reading and writing
                    NetworkStream stream = client.GetStream();
                    // Read the first batch of the TcpServer response bytes.
                    Int32 bytes = stream.Read(data, 0, data.Length);
                    responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                    Console.WriteLine("Received: " + responseData);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception at write to client: " + e.Message);
                removeClientFromList(client);
            }
        }

        private void addClientToList(TcpClient client)
        {
            tcpClientsConfigSent.Add(false);
            tcpClients.Add(client);            
            Console.WriteLine("Adding client. New list count:" + tcpClients.Count);
            sendToFirstAvailableClient(generator.getRandomConfig("ConfigNew"));
        }

        private void removeClientFromList(TcpClient client)
        {
            Console.WriteLine("Removing client");
            if (tcpClients.Contains(client) && tcpClients.Count != 0)
            {
                int index = tcpClients.IndexOf(client);
                Console.WriteLine("Attempting to remove client at index" + index);
                tcpClients.RemoveAt(index);
                tcpClientsConfigSent.RemoveAt(index);
                client.Close();
                client.Dispose();
            }
        }
    
        public void sendToFirstAvailableClient(SimulationConfig config)
        {
            bool sent = false;
            Console.WriteLine("Sending config");
            while (!sent)
            {
                for (int i = 0; i < tcpClients.Count; i++)
                {
                    if (!tcpClientsConfigSent[i])
                    {
                        NetworkStream stream = tcpClients[i].GetStream();
                        byte[] msg = convertSimulationConfigToByte(config);
                        stream.Write(msg,0,msg.Length);
                        sent = true;
                    }
                }
            }
        }

        private byte[] convertSimulationConfigToByte(SimulationConfig config)
        {
            String entireConfig = "";
            String delimiter = "##";
            entireConfig += config.ConfigName + delimiter;
            entireConfig += config.Superscalar + delimiter;
            entireConfig += config.Rename + delimiter;
            entireConfig += config.Reorder + delimiter;
            entireConfig += config.RsbArchitecture + delimiter;
            entireConfig += config.RsPerRsb + delimiter;
            entireConfig += config.Speculative + delimiter;
            entireConfig += config.SpeculationAccuracy + delimiter;
            entireConfig += config.SeparateDispatch + delimiter;
            entireConfig += config.Integer + delimiter;
            entireConfig += config.Floating + delimiter;
            entireConfig += config.Branch + delimiter;
            entireConfig += config.Memory + delimiter;
            entireConfig += config.MemoryArchitecture + delimiter;
            entireConfig += config.L1DataHitrate + delimiter;
            entireConfig += config.L1CodeHitrate + delimiter;
            entireConfig += config.L2Hitrate + delimiter;
            return System.Text.Encoding.ASCII.GetBytes(entireConfig);
        }
    }
}
