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
        public Connection(int broadcastPort)
        {
            this.broadcastPort = broadcastPort;
            FindServerViaBroadcast();
        }
        int broadcastPort;
        int serverPort;
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
                    if (serverResponse != "" && serverResponse != null)
                    {
                        serverIP = serverEndPoint.Address.ToString();
                        serverPort = Convert.ToInt32(serverResponse);
                        Thread connectThread = new Thread(ConnectTCP);
                        connectThread.Start();
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
            

            try
            {
                // Create a TcpClient.
                // Note, for this client to work you need to have a TcpServer
                // connected to the same address as specified by the server, port
                // combination.
                Int32 port = 13000;

                // Prefer a using declaration to ensure the instance is Disposed later.
                using TcpClient tcpClient = new TcpClient(serverIP, serverPort);
                string message = "Hello there, General Kenobi";

                // Translate the passed message into ASCII and store it as a Byte array.
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

                // Get a client stream for reading and writing.
                NetworkStream stream = tcpClient.GetStream();


                // Send the message to the connected TcpServer.
                stream.Write(data, 0, data.Length);

                Console.WriteLine("Sent: {0}", message);

                // Receive the server response.

                // Buffer to store the response bytes.
                data = new Byte[256];

                // String to store the response ASCII representation.
                String responseData = String.Empty;

                // Read the first batch of the TcpServer response bytes.
                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                Console.WriteLine("Received: {0}", responseData);

                // Explicit close is not necessary since TcpClient.Dispose() will be
                // called automatically.
                // stream.Close();
                // client.Close();
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }

            Console.WriteLine("\n Press Enter to continue...");
            Console.Read();
        }
    }
}
