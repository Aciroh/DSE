namespace Server;

public class Simulation
{

    public SimulationConfig Config => config;

    private List<String>? output;

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