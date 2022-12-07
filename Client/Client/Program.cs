using Client;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Xml.Linq;
using System.Xml.Serialization;
using TestClient;

internal class Program
{
    private static void Main(string[] args)
    {

        Config config = new Config();
        Connection connection = new Connection(config.GetPort(), config.GetPort());

        String outputPath = Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName).FullName + @"/output.xml";
        String configPath = Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName).FullName + @"/default_cfg.xml";

        FileManager configFileManager = new FileManager(configPath);
        ConfigurationData configurationData = new ConfigurationData();

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
        configFileManager.UpdateAttribute(9, configurationData.trace, configurationData.general);
        configFileManager.UpdateAttribute(10, configurationData.output, configurationData.general);
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

        Directory.SetCurrentDirectory(Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName).FullName + @"/Tools/PSATSim");

        ProcessStartInfo processStartInfo = new ProcessStartInfo();
        processStartInfo.FileName = "psatsim_con.exe";
        processStartInfo.ArgumentList.Add(configPath);
        processStartInfo.ArgumentList.Add(outputPath);
        Process.Start(processStartInfo);
    }
}