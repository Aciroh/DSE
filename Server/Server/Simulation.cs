namespace Server;

public class Simulation
{
    public double? CrowdingDistance
    {
        get => crowdingDistance;
        set => crowdingDistance = value;
    }

    public int? FrontNumber
    {
        get => frontNumber;
        set => frontNumber = value;
    }

    public SimulationConfig Config => config;

    private List<String>? output;

    private int? frontNumber;

    private double? crowdingDistance;

    public List<string>? Output
    {
        get => output;
        set => output = value;
    }

    private SimulationConfig config;

    public Simulation(List<String> output, SimulationConfig config)
    {
        this.config = config;
        this.output = output;
    }
}