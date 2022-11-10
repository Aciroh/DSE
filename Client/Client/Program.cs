using System.Diagnostics;
using System.Xml.Serialization;

internal class Program
{
    private static void Main(string[] args)
    {
        String config = "C:\\Users\\Roberto\\Desktop\\Proiect Florea\\DSE\\Client\\Client\\default_cfg.xml";
        String output = "C:\\Users\\Roberto\\Desktop\\Proiect Florea\\DSE\\Client\\Client\\output.xml";

        Directory.SetCurrentDirectory(@"C:\Program Files (x86)\PSATSim");

        ProcessStartInfo processStartInfo = new ProcessStartInfo();

        processStartInfo.FileName = "psatsim_con.exe";
        processStartInfo.ArgumentList.Add(config);
        processStartInfo.ArgumentList.Add(output);
        Process.Start(processStartInfo);
    }
}