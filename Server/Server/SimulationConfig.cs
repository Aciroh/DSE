namespace Server;

public class SimulationConfig
{
    public string ConfigName => config_name;

    public int Superscalar => superscalar;

    public int Rename => rename;

    public int Reorder => reorder;

    public string RsbArchitecture => rsb_architecture;

    public int RsPerRsb => rs_per_rsb;

    public bool Speculative => speculative;

    public float SpeculationAccuracy => speculation_accuracy;

    public bool SeparateDispatch => separate_dispatch;

    public int Integer => integer;

    public int Floating => floating;

    public int Branch => branch;

    public int Memory => memory;

    public string MemoryArchitecture => memory_architecture;

    public float L1DataHitrate => l1_data_hitrate;

    public float L1CodeHitrate => l1_code_hitrate;

    public float L2Hitrate => l2_hitrate;

    //String name
    private String config_name;
    //[1...16]
    private int superscalar;
    //[1...512]
    private int rename;
    //[1...512]
    private int reorder;
    //[Distributed, Centralized, Hybrid]
    private String rsb_architecture;
    //[1...8]
    private int rs_per_rsb;
    //[True/False]
    private bool speculative;
    //[0...1]
    private float speculation_accuracy;
    //[True/False]
    private bool separate_dispatch;
    //[1...8]
    private int integer;
    //[1...8]
    private int floating;
    //[1...8]
    private int branch;
    //[1...8]
    private int memory;
    //[L1,L2,System]
    private String memory_architecture;
    //[0...1]
    private float l1_data_hitrate;
    //[0...1]
    private float l1_code_hitrate;
    //[0...1]
    private float l2_hitrate;

    public SimulationConfig(
        String config_name, 
        int superscalar, 
        int rename, 
        int reorder, 
        String rsb_architecture, 
        int rs_per_rsb, 
        bool speculative, 
        float speculation_accuracy, 
        bool separate_dispatch, 
        int integer, 
        int floating, 
        int branch, 
        int memory, 
        String memory_architecture, 
        float l1_data_hitrate, 
        float l1_code_hitrate, 
        float l2_hitrate)
    {
        this.config_name = config_name;
        this.superscalar = superscalar;
        this.rename = rename;
        this.reorder = reorder;
        this.rsb_architecture = rsb_architecture;
        this.rs_per_rsb = rs_per_rsb;
        this.speculative = speculative;
        this.speculation_accuracy = speculation_accuracy;
        this.separate_dispatch = separate_dispatch;
        this.integer = integer;
        this.floating = floating;
        this.branch = branch;
        this.memory = memory;
        this.memory_architecture = memory_architecture;
        this.l1_data_hitrate = l1_data_hitrate;
        this.l1_code_hitrate = l1_code_hitrate;
        this.l2_hitrate = l2_hitrate;
    }
}