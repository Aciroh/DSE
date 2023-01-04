using Client;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Xml.Linq;
using System.Xml.Serialization;
using Client;

internal class Program
{
    private static void Main(string[] args)
    {

        Config config = new Config();
        Connection connection = new Connection(config.GetPort());

        DataHandler dataHandler = new DataHandler();

        dataHandler.setXmlConfiguration();
        dataHandler.runConfiguration();
        
    }
}