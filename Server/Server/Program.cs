using Server;

internal class Program
{
    private static void Main(string[] args)
    {

        Config config = new();
        config.ReadConfig();
        config.PrintConfig();

        //Connection con = new Connection();

    }
}

//Thread listener = new Thread(ParameterizedThreadStart);