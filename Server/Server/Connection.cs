using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal class Connection
    {
        Connection(int listenPort, int streamPort)
        {
            this.listenPort = listenPort;
            this.streamPort = streamPort;
        }

        private int listenPort;
        private int streamPort;


        public void StartServer()
        {

        }

        internal void CheckForOtherServers()
        {
            //This must be ran first, to check if there is another server on the network. If there is, cancel the execution and return exception or whatever
        }

        public void ListenForBroadcast()
        {
            while(true)
            {

            }
        }


    }
}
