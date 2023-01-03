using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class Config
    {
        Dictionary<Parameters, string> config = new Dictionary<Parameters, string>();
        enum Parameters
        {
            ConfigPath,
            Port
        }

        public Config()
        {
            GetConfigLocation();
            config.Add(Parameters.Port, null);
            ReadConfig();
            PrintConfig();
        }


        // Reads current folder, goes 3 folders back and enters /Resources folder where it finds Config.cfg
        private void GetConfigLocation()
        {
            // Oposite slash required for MacOS compatibility
            string path = Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName).FullName + @"/Resources/Config.cfg";
            config.Add(Parameters.ConfigPath, path);
        }

        private void ReadConfig()
        {
            Console.WriteLine(config[Parameters.ConfigPath]);
            string[] lines = File.ReadAllLines(config[Parameters.ConfigPath]);
            foreach (string line in lines)
            {
                Console.WriteLine(line);
                switch (line)
                {
                    case string s when s.StartsWith("Port:"):
                        if (s.Count() > 5) config[Parameters.Port] = s.Substring(5);
                        break;
                        //Add more parameters here
                }
            }
        }

        private void PrintConfig()
        {
            Console.WriteLine();
            foreach (var config in config)
            {
                Console.WriteLine(config.Key + " : " + config.Value);
            }
        }

        public int GetPort()
        {
            return Convert.ToInt32(config[Parameters.Port]);
        }

    }
}
