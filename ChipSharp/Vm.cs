namespace ChipSharp;

public class Vm
{
    private const ushort RomStart = 0x200; // First 512 bytes or memory are empty for backwards compat reasons
    private const ushort MemoryStart = 0x0; // The first byte of the Vm's memory
    
    private byte[] Fonts = // Contains the systems font-set which will be loaded into memory 
    {
        0xF0, 0x90, 0x90, 0x90, 0xF0, // 0
        0x20, 0x60, 0x20, 0x20, 0x70, // 1
        0xF0, 0x10, 0xF0, 0x80, 0xF0, // 2
        0xF0, 0x10, 0xF0, 0x10, 0xF0, // 3
        0x90, 0x90, 0xF0, 0x10, 0x10, // 4
        0xF0, 0x80, 0xF0, 0x10, 0xF0, // 5
        0xF0, 0x80, 0xF0, 0x90, 0xF0, // 6
        0xF0, 0x10, 0x20, 0x40, 0x40, // 7
        0xF0, 0x90, 0xF0, 0x90, 0xF0, // 8
        0xF0, 0x90, 0xF0, 0x10, 0xF0, // 9
        0xF0, 0x90, 0xF0, 0x90, 0x90, // A
        0xE0, 0x90, 0xE0, 0x90, 0xE0, // B
        0xF0, 0x80, 0x80, 0x80, 0xF0, // C
        0xE0, 0x90, 0x90, 0x90, 0xE0, // D
        0xF0, 0x80, 0xF0, 0x80, 0xF0, // E
        0xF0, 0x80, 0xF0, 0x80, 0x80  // F
    };
    public byte[] Memory { get; set; } // Vm's RAM
    public byte[] Display { get; set; } // 64 x 32 1-bit pixel grid
    public ushort PC { get; set; } // Points to the current instruction in memory
    public ushort I { get; set; } // Used as a pointer to locations in memory
    public ushort[] Stack { get; set; } // Stores 16-bit addresses, used to call functions and return from them 
    public ushort SP { get; set; } // Points to an address in the stack
    public byte DelayTimer { get; set; } // Decrements at a rate of 60Hz until it reaches 0
    public byte SoundTimer { get; set; } // Decrements at a rate of 60Hz, will beep as long as value is non zero
    public byte[] V { get; set; } // 16 general purpose variable registers
    public bool[] Keys { get; set; } // Represents the state of the 16 available keys { Down = True, Up = False }

    
    // Initialise key components of the Vm
    public Vm()
    {
        Memory = new byte[4096];
        Display = new byte[64 * 32];
        PC = RomStart;
        I = 0;
        Stack = new ushort[12];
        SP = 0;
        DelayTimer = 0;
        SoundTimer = 0;
        V = new byte[16];
        Keys = new bool[16];
    }

    // Create a Vm and load the selected Rom into it's memory
    public static Vm CreateVm(byte[] rom)
    {
        var vm = new Vm();
        vm.LoadFonts();
        vm.LoadRom(rom);

        return vm;
    }
    
    // Load the font-set into the VM's memory
    private void LoadFonts()
    {
        Fonts.CopyTo(Memory, MemoryStart);
    }

    // Load a ROM into the VM's memory
    private void LoadRom(byte[] rom)
    {
        rom.CopyTo(Memory, RomStart);
    }
}