namespace Server;

public class Handler
{
    public List<Simulation> simulations = new List<Simulation>();
    public ConfigGenerator generator = new ConfigGenerator();
    public Connection connection;
    public Config config;
    private Random rnd = new Random();
    private int generationNR = 0;
    private int configNR = 0;
    public Handler()
    {
        config = new Config();
        connection = new Connection(config.GetPort(), config.GetPort()+1);
        connection.OutputReceived += handleServerEvent;
        connection.ConnectionEstablished += handleServerEvent;

        //Generare configuratii random
        for (int i = 0; i < config.GetGenerationCount(); i++) //modificat getGenerationCount -> nu e nr de generatii ci nr de indivizii din populatia initiala
        {
            simulations.Add(new Simulation(null, generator.getRandomConfig("Config-" + i)));
            configNR++;
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
            List<List<Simulation>> paretoFronts = doPareto();

            //printSimulationsFromAllFronts(paretoFronts);

            for (var index = 0; index < paretoFronts.Count; index++)
            {
                List<Simulation> front = paretoFronts[index];
                front = calculateAndSortByCrowdingDistance(front);
            }

            printSortedSimulationsFromAllFronts(paretoFronts);

            //DELETE SIMULATIONS WHEN NEEDED
            //NEW GENERATION!!!!

            List<Simulation> allSimulations = new List<Simulation>();
            foreach (List<Simulation> front in paretoFronts)
            {
                foreach (Simulation simulation in front)
                {
                    allSimulations.Add(simulation);
                }
            }

            simulations = allSimulations;

            if (generationNR > 0)
            {
                for (int index = 0; index < config.GetGenerationCount(); index++)
                {
                    simulations.Remove(simulations.Last());
                }
            }

            if (generationNR == 3)
            {
                foreach (Simulation simulation in simulations)
                {
                    Console.WriteLine("Surviving simulation: " + simulation.Config.ConfigName);
                }
                Console.WriteLine(simulations.Count);
                return;
            }

            List<Simulation> childSimulations = getNewGeneration(simulations);
            generationNR++;
            foreach (var simulation in childSimulations)
            {
                simulations.Add(simulation);
            }

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

    private void printSimulationsFromAllFronts(List<List<Simulation>> paretoFronts)
    {
        for (var index = 0; index < paretoFronts.Count; index++)
        {
            var front = paretoFronts[index];
            Console.WriteLine("Front " + index + ": ");
            foreach (Simulation simulation in front)
            {
                Console.WriteLine("Simulation with:");
                Console.WriteLine("IPC: " + simulation.Output[0]);
                Console.WriteLine("Power: " + simulation.Output[1]);
            }
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
        }
    }

    private void printSortedSimulationsFromAllFronts(List<List<Simulation>> paretoFrontsSorted)
    {
        for (var index = 0; index < paretoFrontsSorted.Count; index++)
        {
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("Sorted front:");
            List<Simulation> front = paretoFrontsSorted[index];
            foreach (var simulation in front)
            {
                Console.WriteLine("Sim " + simulation.Config.ConfigName);
                Console.WriteLine("IPC " + simulation.Output[0]);
                Console.WriteLine("Power " + simulation.Output[1]);
                Console.WriteLine("Crowding distance " + simulation.CrowdingDistance);
                Console.WriteLine("Front " + simulation.FrontNumber);
            }
        }
    }

    private List<List<Simulation>> doPareto()
    {
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
        return findParetoFronts(outputArray);
    }

    private List<Simulation> getNewGeneration(List<Simulation> parentList)
    {
        List<Simulation> newList = new List<Simulation>();
        for (int j = 0; j < config.GetGenerationCount(); j++)
        {
            newList.Add(new Simulation(null,
                generator.getConfigFromParents(getRandomParent(parentList), getRandomParent(parentList),
                    "Config-" + configNR)));
            configNR++;
        }

        return newList;
    }

    private Simulation getRandomParent(List<Simulation> allSimulations)
    {
        Simulation parent1 = allSimulations[rnd.Next(allSimulations.Count)];
        Simulation parent2 = allSimulations[rnd.Next(allSimulations.Count)];

        if (parent1.FrontNumber > parent2.FrontNumber)
        {
            return parent1;
        }
        else if (parent1.FrontNumber == parent2.FrontNumber)
        {
            if (parent1.CrowdingDistance > parent2.CrowdingDistance)
            {
                return parent1;
            }
        }
        return parent2;
    }

    private List<Simulation> calculateAndSortByCrowdingDistance(List<Simulation> front)
    {
        int dimension = 2;
        for (int i = 0; i < dimension; i++)
        {
            front.Sort((sim1, sim2) =>
            {
                return (Convert.ToDouble(sim1.Output[i]) < Convert.ToDouble(sim2.Output[i])) ? -1 : 1;
            });
            front[0].CrowdingDistance = double.PositiveInfinity;
            front.Last().CrowdingDistance = double.PositiveInfinity;
            for (int index = 1; index < front.Count - 1; index++)
            {
                var simulation = front[index];
                Simulation highNeighbor = front[index + 1];
                Simulation lowNeighbor = front[index - 1];
                simulation.CrowdingDistance = 0.0;
                simulation.CrowdingDistance +=
                    Math.Abs(Convert.ToDouble(simulation.Output[i]) - Convert.ToDouble(highNeighbor.Output[i]));
                simulation.CrowdingDistance +=
                    Math.Abs(Convert.ToDouble(simulation.Output[i]) - Convert.ToDouble(lowNeighbor.Output[i]));
            }
        }
        front.Sort((sim1, sim2) =>
        {
            return (Convert.ToDouble(sim1.CrowdingDistance) < Convert.ToDouble(sim2.CrowdingDistance)) ? -1 : 1;
        });
        return front;
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