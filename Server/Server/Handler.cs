namespace Server;

public class Handler
{
    public List<Simulation> simulations = new List<Simulation>();
    public ConfigGenerator generator = new ConfigGenerator();
    public Connection connection;
    public Config config;
    public Handler()
    {
        config = new Config();
        connection = new Connection(config.GetPort(), config.GetPort()+1);
        connection.OutputReceived += handleServerEvent;
        connection.ConnectionEstablished += handleServerEvent;
        for (int i = 0; i < config.GetGenerationCount(); i++)
        {
            simulations.Add(new Simulation(null, generator.getRandomConfig("Config-" + i)));
            Console.WriteLine("Handler ordered config " + i);
        }
    }
    
    public void handleServerEvent(object o, EventArgs e)
    {
        while (simulations.Count == 0)
        {
            
        }
        if (o != null)
        {
            String simulationOutput = o as string;
            attachOutputToSimulation(simulationOutput);
        }
        //Daca am N outputuri gata, atunci calculez generatie noua
        //Daca am configuratie generata, o trimit
        for (var index = 0; index < simulations.Count; index++)
        {
            var simulation = simulations[index];
            if (simulation.Output == null)
            {
                connection.sendToFirstAvailableClient(simulation.Config);
                return;
            }
        }
        //Daca nu am configuratie generata, generez N configuratii (cu FileHandler verific)
    }

    private void attachOutputToSimulation(String simulationOutput)
    {
        List<String> outputList = simulationOutput.Split("##").ToList();
        String name = outputList[1];
        String ipc = outputList[1];
        String power = outputList[1];
        
        for (int index = 0; index < simulations.Count; index++)
        {
            if (simulations[index].Output == null)
            {
                if (simulations[index].Config.ConfigName == name)
                {
                    Console.WriteLine(index + " Count " + simulations.Count);
                    simulations[index].Output = new List<string>();
                    simulations[index].Output.Add(ipc);
                    simulations[index].Output.Add(power);
                }
            }
        }
    }
}