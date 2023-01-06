using Client;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Xml.Linq;
using System.Xml.Serialization;
using Client;

internal class Program
{
    private static Config config;
    private static Connection connection;
    private static DataHandler dataHandler;
    
    private static void Main(string[] args)
    {
        config = new Config();
        connection = new Connection(config.GetPort());
        connection.ConfigurationReceived += configReceived;
        dataHandler = new DataHandler();
    }

    internal static void configReceived(object s, EventArgs e)
    {
        String received = s as string;
        Console.WriteLine("Configuration received in program.cs");
        Console.WriteLine("Configuration is: " + received);
        while (dataHandler == null)
        {
            
        }
        dataHandler.updateConfigurationData(dataHandler.getParametersListFromString(received));
        dataHandler.setXmlConfiguration();
        String output = dataHandler.runConfiguration();
        connection.sendOutput(output);
    }
}