namespace Server;

public class ConfigGenerator
{
    Random rnd = new Random();
    public SimulationConfig getRandomConfig(String configName)
    {
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

    //Crossover 
    public SimulationConfig getConfigFromParents(Simulation parent1, Simulation parent2, String configName)
    {
        // Following line generates a bool
        // rnd.Next(2) == 0;
        SimulationConfig child = new SimulationConfig(
            configName,
            rnd.Next(2) == 0 ? parent1.Config.Superscalar : parent2.Config.Superscalar,
            rnd.Next(2) == 0 ? parent1.Config.Rename : parent2.Config.Rename,
            rnd.Next(2) == 0 ? parent1.Config.Reorder : parent2.Config.Reorder,
            rnd.Next(2) == 0 ? parent1.Config.RsbArchitecture : parent2.Config.RsbArchitecture,
            rnd.Next(2) == 0 ? parent1.Config.RsPerRsb : parent2.Config.RsPerRsb,
            rnd.Next(2) == 0 ? parent1.Config.Speculative : parent2.Config.Speculative,
            rnd.Next(2) == 0 ? parent1.Config.SpeculationAccuracy : parent2.Config.SpeculationAccuracy,
            rnd.Next(2) == 0 ? parent1.Config.SeparateDispatch : parent2.Config.SeparateDispatch,
            rnd.Next(2) == 0 ? parent1.Config.Integer : parent2.Config.Integer,
            rnd.Next(2) == 0 ? parent1.Config.Floating : parent2.Config.Floating,
            rnd.Next(2) == 0 ? parent1.Config.Branch : parent2.Config.Branch,
            rnd.Next(2) == 0 ? parent1.Config.Memory : parent2.Config.Memory,
            rnd.Next(2) == 0 ? parent1.Config.MemoryArchitecture : parent2.Config.MemoryArchitecture,
            rnd.Next(2) == 0 ? parent1.Config.L1DataHitrate : parent2.Config.L1DataHitrate,
            rnd.Next(2) == 0 ? parent1.Config.L1CodeHitrate : parent2.Config.L1CodeHitrate,
            rnd.Next(2) == 0 ? parent1.Config.L2Hitrate : parent2.Config.L2Hitrate
        );
        return child;
    }
}