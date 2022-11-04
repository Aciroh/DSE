
using Server;

internal class Program
{
    private static void Main(string[] args)
    {
        Connection con = new Connection();

        Config config = new();
        config.ReadConfig();
        config.PrintConfig();
    }
}

//Thread listener = new Thread(ParameterizedThreadStart);