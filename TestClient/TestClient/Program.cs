using TestClient;

internal class Program
{
    private static void Main(string[] args)
    {
        Config config = new Config();
        Connection connection = new Connection(config.GetPort(), config.GetPort());

    }
}

//Thread listener = new Thread(ParameterizedThreadStart);