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
        if (simulations.Last().Output != null)
        {
            //DoPareto
            List<double> ipcList = new List<double>();
            List<double> powerList = new List<double>();
            for (var index = 0; index < simulations.Count; index++)
            {
                var simulation = simulations[index];
                Console.WriteLine(index + " ipc=" + simulation.Output[0] + " power=" + simulation.Output[1]);
                ipcList.Add(Convert.ToDouble(simulation.Output[0]));
                powerList.Add(Convert.ToDouble(simulation.Output[1]));
            }
            double[][] outputArray = ipcList.Zip(powerList, (x, y) => new double[] { x, y }).ToArray();
            simulations = findParetoFront(outputArray);
            foreach (var simulation in simulations)
            {
                Console.WriteLine("Simulation " + simulation.Config.ConfigName + " has output ipc " + simulation.Output[0] + " and power " + simulation.Output[1]);
            }
            //DELETE SIMULATIONS WHEN NEEDED
            //NEW GENERATION!!!!
        }
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
        String ipc = outputList[2];
        String power = outputList[3];
        
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

    private List<Simulation> findParetoFront(double[][] outputs)
    {
        Array.Sort(outputs, (a, b) => b[0].CompareTo(a[0]));
        
        List<Simulation> paretoFrontSimulations = new List<Simulation>();
        
        for (int index = 0; index < outputs.Length; index++)
        {
            double[] output = outputs[index];
            bool isDominated = false;
            
            for (int i = 0; i < paretoFrontSimulations.Count; i++)
            {
                // If the point is dominated, set the flag to true and break out of the loop
                if (output[0] >= Convert.ToDouble(paretoFrontSimulations[i].Output[0]) &&
                    output[1] <= Convert.ToDouble(paretoFrontSimulations[i].Output[1]))
                {
                    isDominated = true;
                    break;
                }
            }

            if (!isDominated)
            {
                Simulation simulation = findFirstSimulation(output);
                if (simulation != null) paretoFrontSimulations.Add(simulation);
                paretoFrontSimulations.RemoveAll(x => Convert.ToDouble(x.Output[0]) <= output[0] && Convert.ToDouble(x.Output[1]) >= output[1]);
            }
        }
        return paretoFrontSimulations;
    }

    private Simulation? findFirstSimulation(double[] output)
    {
        for (var index = 0; index < simulations.Count; index++)
        {
            var simulation = simulations[index];
            if (Convert.ToDouble(simulation.Output[0]) == output[0] && Convert.ToDouble(simulation.Output[1]) == output[1])
            {
                return simulation;
            }
        }
        return null;
    }
}