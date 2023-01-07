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
            List<List<Simulation>> paretoFronts = findParetoFronts(outputArray);
            int i = 0;
            foreach (List<Simulation> front in paretoFronts)
            {
                Console.WriteLine("Front " + i + ": ");
                i++;
                foreach (Simulation simulation in front)
                {
                    Console.WriteLine("Simulation with:");
                    Console.WriteLine("IPC: " + simulation.Output[0]);
                    Console.WriteLine("Power: " + simulation.Output[1]);
                }
            }
            
            foreach (List<Simulation> front in paretoFronts)
            {
                crowdingDistanceSorter(front);
            }
            //DELETE SIMULATIONS WHEN NEEDED
            //NEW GENERATION!!!!
        }
        //Daca am configuratie generata, o trimit
        // A NU SE MODIFICA!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        for (var index = 0; index < simulations.Count; index++)
        {
            var simulation = simulations[index];
            if (simulation.Output == null)
            {
                connection.sendToFirstAvailableClient(simulation);
                return;
            }
        }
        // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        // A NU SE MODIFICA!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        //Daca nu am configuratie generata, generez N configuratii (cu FileHandler verific)
    }
    
    private void crowdingDistanceSorter(List<Simulation> front)
    {
        
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

    internal class outputToSort
    {
        internal double[] outputs;
        internal int dominationCount;
        internal List<outputToSort> dominated;
        internal outputToSort(double[] outputs, int dominationCount, List<outputToSort> dominated)
        {
            this.dominated = dominated;
            this.dominationCount = dominationCount;
            this.outputs = outputs;
        }
    }
    
    private List<List<Simulation>> findParetoFronts(double[][] outputs)
    {
        Array.Sort(outputs, (a, b) => b[0].CompareTo(a[0]));
        
        List<List<Simulation>> paretoFrontSimulations = new List<List<Simulation>>();

        List<outputToSort> allOutputs = new List<outputToSort>();
        foreach (var output in outputs)
        {
            allOutputs.Add(new outputToSort(output, 0, new List<outputToSort>()));
        }

        
        // Check what outputs are dominated
        foreach (outputToSort output in allOutputs)
        {
            foreach (outputToSort otherOutput in allOutputs)
            {
                if ((output.outputs[0] >= otherOutput.outputs[0] && 
                     output.outputs[1] <= otherOutput.outputs[1]) && 
                    (output.outputs[0] > otherOutput.outputs[0] || 
                     output.outputs[1] < otherOutput.outputs[1]))
                {
                    output.dominated.Add(otherOutput);
                    otherOutput.dominationCount++;
                }
            }
        }

        while (allOutputs.Count > 0)
        {
            List<outputToSort> thisFrontOutput = new List<outputToSort>();
            paretoFrontSimulations.Add(new List<Simulation>());
            foreach (outputToSort output in allOutputs.ToList())
            {
                if (output.dominationCount == 0)
                {
                    Simulation simulationForThisOutput = findFirstSimulation(output.outputs);
                    simulationForThisOutput.FrontNumber = paretoFrontSimulations.Count - 1;
                    paretoFrontSimulations.Last().Add(simulationForThisOutput);
                    allOutputs.Remove(output);
                    thisFrontOutput.Add(output);
                }
            }

            foreach (outputToSort output in thisFrontOutput)
            {
                foreach (outputToSort dominated in output.dominated)
                {
                    dominated.dominationCount--;
                }
            }
        }
        
        return paretoFrontSimulations;
    }

    private Simulation findFirstSimulation(double[] output)
    {
        for (var index = 0; index < simulations.Count; index++)
        {
            var simulation = simulations[index];
            if (Convert.ToDouble(simulation.Output[0]) == output[0] && Convert.ToDouble(simulation.Output[1]) == output[1])
            {
                return simulation;
            }
        }
        throw new Exception();
    }
}