using Client;
using System.Diagnostics;
using System.Xml.Serialization;

internal class Program
{
    private static void Main(string[] args)
    {
        String config = "C:\\Users\\Roberto\\Desktop\\Proiect Florea\\DSE\\Client\\Client\\default_cfg.xml";
        String output = "C:\\Users\\Roberto\\Desktop\\Proiect Florea\\DSE\\Client\\Client\\output.xml";

        //1. Read configuration Data from the Server 

        FileManager configFileManager = new FileManager(config);
        String configurationName = "Config-2";
        configFileManager.UpdateAttribute(configurationName);

        Directory.SetCurrentDirectory(@"C:\Program Files (x86)\PSATSim");

        ProcessStartInfo processStartInfo = new ProcessStartInfo();

        processStartInfo.FileName = "psatsim_con.exe";
        processStartInfo.ArgumentList.Add(config);
        processStartInfo.ArgumentList.Add(output);
        Process.Start(processStartInfo);
    }
}