namespace Server;

public class ConfigGenerator
{
    public SimulationConfig getRandomConfig(String configName)
    {
        Random rnd = new Random();
        Dictionary<int, String> rsb_architecture_dictionary = new Dictionary<int, string>()
            { { 0, "distributed" }, { 1, "centralized" }, { 2, "hybrid" } };
        Dictionary<int, String> memory_architecture_dictionary = new Dictionary<int, string>()
            { { 0, "l1" }, { 1, "l2" }, { 2, "system" } };
        SimulationConfig config = new SimulationConfig(
            configName,
            rnd.Next(1, 17),
            rnd.Next(1, 513),
            rnd.Next(1, 513),
            rsb_architecture_dictionary[rnd.Next(3)],
            rnd.Next(1, 9),
            rnd.Next(2)==0,
            rnd.NextSingle(),
            rnd.Next(2)==0,
            rnd.Next(1,9),
            rnd.Next(1,9),
            rnd.Next(1,9),
            rnd.Next(1,9),
            memory_architecture_dictionary[rnd.Next(3)],
            rnd.NextSingle(),
            rnd.NextSingle(),
            rnd.NextSingle());
        return config;
    }
}