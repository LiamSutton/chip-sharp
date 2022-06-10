namespace ChipSharp;

public class Vm
{
    public byte[] Memory { get; set; }
    public byte[] Display { get; set; }
    public ushort PC { get; set; }
    public ushort I { get; set; }
    public ushort[] Stack { get; set; }
    public ushort SP { get; set; }
    public byte DelayTimer { get; set; }
    public byte SoundTimer { get; set; }
    public byte[] V { get; set; }
    public bool[] Keys { get; set; }


    public Vm()
    {
        Memory = new byte[4096];
        Display = new byte[64 * 32];
        PC = 0;
        I = 0;
        Stack = new ushort[12];
        SP = 0;
        DelayTimer = 0;
        SoundTimer = 0;
        V = new byte[16];
        Keys = new bool[16];
    }
}