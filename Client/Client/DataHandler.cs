using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    internal class DataHandler
    {
        String outputPath = Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName).FullName + @"/output.xml";
        String configPath = Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName).FullName + @"/default_cfg.xml";

        FileManager configFileManager;
        FileManager outputFileManager;
        ConfigurationData configurationData;

        public DataHandler() {
            configFileManager = new FileManager(configPath);
            outputFileManager = new FileManager(outputPath);
            configurationData = new ConfigurationData();
        }

        public List<String> getParametersListFromString(String s)
        {
            Console.WriteLine("Split list");
            List<String> parametersList = new List<string>();
            parametersList = s.Split("##").ToList();
            parametersList.RemoveAt(parametersList.Count-1);
            return parametersList;
        }

        public void updateConfigurationData(List<String> parametersList)
        {
            if (parametersList.Count < 17)
            {
                return;
            }
            //Config Name
            configurationData.configurationName = parametersList[0];
            //Superscalar
            configurationData.superscalar = parametersList[1];
            //Rename
            configurationData.rename = parametersList[2];
            //Reorder
            configurationData.reorder = parametersList[3];
            //RSBArchitecture
            configurationData.rsb_architecture = parametersList[4];
            //RsPerRSB
            configurationData.rs_per_rsb = parametersList[5];
            //Speculative
            configurationData.speculative = parametersList[6];
            //SpeculationAccuracy
            configurationData.speculation_accuracy = parametersList[7];
            //SeparateDispatch
            configurationData.separate_dispatch = parametersList[8];
            //Integer
            configurationData.integer = parametersList[9];
            //Floating
            configurationData.floating = parametersList[10];
            //Branch
            configurationData.branch = parametersList[11];
            //Memory
            configurationData.memoryp = parametersList[12];
            //MemoryArchitecture
            configurationData.architecture = parametersList[13];
            //L1DataHitrate
            configurationData.hitratel1d = parametersList[14];
            //L1CodeHitrate
            configurationData.hitratel1c = parametersList[15];
            //L2Hitrate
            configurationData.hitratel2 = parametersList[16];
        }

        public void setXmlConfiguration() {
            configFileManager.UpdateAttribute(0, configurationData.configurationName, configurationData.config);
            configFileManager.UpdateAttribute(0, configurationData.superscalar, configurationData.general);
            configFileManager.UpdateAttribute(1, configurationData.rename, configurationData.general);
            configFileManager.UpdateAttribute(2, configurationData.reorder, configurationData.general);
            configFileManager.UpdateAttribute(3, configurationData.rsb_architecture, configurationData.general);
            configFileManager.UpdateAttribute(4, configurationData.rs_per_rsb, configurationData.general);
            configFileManager.UpdateAttribute(5, configurationData.speculative, configurationData.general);
            configFileManager.UpdateAttribute(6, configurationData.speculation_accuracy, configurationData.general);
            configFileManager.UpdateAttribute(7, configurationData.separate_dispatch, configurationData.general);
            configFileManager.UpdateAttribute(8, configurationData.seed, configurationData.general);
            configFileManager.UpdateAttribute(10, configurationData.outputPath, configurationData.general);
            configFileManager.UpdateAttribute(11, configurationData.vdd, configurationData.general);
            configFileManager.UpdateAttribute(12, configurationData.frequency, configurationData.general);

            configFileManager.UpdateAttribute(0, configurationData.architecturee, configurationData.execution);
            configFileManager.UpdateAttribute(1, configurationData.integer, configurationData.execution);
            configFileManager.UpdateAttribute(2, configurationData.floating, configurationData.execution);
            configFileManager.UpdateAttribute(3, configurationData.branch, configurationData.execution);
            configFileManager.UpdateAttribute(4, configurationData.memoryp, configurationData.execution);

            configFileManager.UpdateAttribute(0, configurationData.architecture, configurationData.memory);

            configFileManager.UpdateAttribute(0, configurationData.architecture, configurationData.memory);
            configFileManager.UpdateAttribute(0, configurationData.hitratel1c, configurationData.memoryL1code);
            configFileManager.UpdateAttribute(1, configurationData.latencyl1c, configurationData.memoryL1code);
            configFileManager.UpdateAttribute(0, configurationData.hitratel1d, configurationData.memoryL1data);
            configFileManager.UpdateAttribute(1, configurationData.latencyl1d, configurationData.memoryL1data);
            configFileManager.UpdateAttribute(0, configurationData.hitratel2, configurationData.memoryL2);
            configFileManager.UpdateAttribute(1, configurationData.latencyl2, configurationData.memoryL2);
            configFileManager.UpdateAttribute(0, configurationData.latency, configurationData.system);
        }

        public void setTrace(int traceNumber) {
            configFileManager = new FileManager(configPath);
            configFileManager.UpdateAttribute(9, configurationData.tracesList[traceNumber], configurationData.general);
            Console.WriteLine(configurationData.tracesList[traceNumber]);
        }

        public String runConfiguration()
        {
            Directory.SetCurrentDirectory(Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName).FullName + @"/Tools/PSATSim");

            int numberOfTraces = 10;
            double ipcSum = 0;
            double powerSum = 0;

            for (int traceNumber = 0; traceNumber < numberOfTraces; traceNumber++)
            {
                setTrace(traceNumber);

                ProcessStartInfo processStartInfo = new ProcessStartInfo();
                processStartInfo.FileName = "psatsim_con.exe";
                processStartInfo.ArgumentList.Add(configPath);
                processStartInfo.ArgumentList.Add(outputPath);
                processStartInfo.ArgumentList.Add("-cg");

                Process process = new Process();
                process.StartInfo = processStartInfo;

                process.Start();

                while (!process.HasExited)
                {

                }

                ipcSum += Convert.ToDouble(outputFileManager.ReadAttribute(3, configurationData.outputTargetNodePath));
                powerSum += Convert.ToDouble(outputFileManager.ReadAttribute(5, configurationData.outputTargetNodePath));
            }

            double ipc = ipcSum / numberOfTraces;
            double power = powerSum/ numberOfTraces;

            string delimiter = "##";
            Directory.SetCurrentDirectory(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName + @"/bin/Debug/net6.0");
            return "output" + delimiter + configurationData.configurationName + delimiter + ipc + delimiter + power + delimiter;
        }
    }
}
