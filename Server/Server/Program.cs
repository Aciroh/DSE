using Server;

internal class Program
{
    private static void Main(string[] args)
    {

        Config config = new();

        Connection con = new Connection(config.GetPort(), config.GetPort()+1);

        
        
        
        //con.sendToFirstAvailableClient(generator.getRandomConfig("Config1"));
    }
}

//Thread listener = new Thread(ParameterizedThreadStart);