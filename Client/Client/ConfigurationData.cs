using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    internal class ConfigurationData
    {

        //XML nodes paths
        public String config { get; set; } = "config";
        public String general { get; set; } = "config/general";
        public String execution { get; set; } = "config/execution";
        public String memory { get; set; } = "config/memory";
        public String memoryL1code { get; set; } = "config/memory/l1_code";
        public String memoryL1data { get; set; } = "config/memory/l1_data";
        public String memoryL2 { get; set; } = "config/memory/l2";
        public String system { get; set; } = "config/memory/system";
        
        //General
        public String configurationName { get; set; } = "Config-7";
        public String superscalar { get; set; } = "6";
        public String rename { get; set; } = "18";
        public String reorder { get; set; } = "20";
        public String rsb_architecture { get; set; } = "distributed";
        public String rs_per_rsb { get; set; } = "2";
        public String speculative { get; set; } = "true";
        public String speculation_accuracy { get; set; } = "0.980";
        public String separate_dispatch { get; set; } = "true";
        public String seed { get; set; } = "0";

        public List<String> tracesList = new List<string> {
            "applu.tra",
            "compress.tra",
            "epic.tra",
            "fpppp.tra",
            "ijpeg.tra",
            "mpeg2d.tra",
            "mpeg2e.tra",
            "pegwitd.tra",
            "perl.tra",
            "toast.tra"
        };
        public String outputPath { get; set; } = "output.xml";
        public String vdd { get; set; } = "2.3";
        public String frequency { get; set; } = "600";
     
        //Execution
        public String architecturee { get; set; } = "standard";
        public String integer { get; set; } = "2";
        public String floating { get; set; } = "2";
        public String branch { get; set; } = "1";
        public String memoryp { get; set; } = "1";
      
        //Memory
        public String architecture { get; set; } = "l2";
        public String hitratel1c { get; set; } = "0.990";
        public String latencyl1c { get; set; } = "1";
        public String hitratel1d { get; set; } = "0.970";
        public String latencyl1d { get; set; } = "1";
        public String hitratel2 { get; set; } = "0.990";
        public String latencyl2 { get; set; } = "3";
        public String latency { get; set; } = "20";


        // Output Results

        public String outputTargetNodePath { get; set; } = "variation/general";

    }
}
