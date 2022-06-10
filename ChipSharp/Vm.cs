namespace ChipSharp;

public class Vm
{
    private const int DisplaySize = 64 * 32; // Screen is a 64 * 32 pixel grid
    private const int MemorySize = 4096;
    private const int StackSize = 12;
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
    private ushort CurrentOpCode { get; set; } // The OpCode for the current instruction to be executed

    
    // Initialise key components of the Vm
    public Vm()
    {
        Memory = new byte[MemorySize];
        Display = new byte[DisplaySize];
        PC = RomStart;
        I = 0;
        Stack = new ushort[StackSize];
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

    public void Reset()
    {
        PC = RomStart;
        I = 0;
        SP = 0;
        Display = new byte[DisplaySize];
    }
    
    // Load a ROM into the VM's memory
    private void LoadRom(byte[] rom)
    {
        rom.CopyTo(Memory, RomStart);
    }

    public void Run(bool debugMode)
    {
        Console.WriteLine("== Running Vm ==");
        for (var i = 0; i < 10; i++)
        {
            if (debugMode)
            {
                OutputDebugInformation();
            }
            
            Tick();
        }

        Console.WriteLine("== Vm Finished ==\n");
    }
    private void Tick()
    {
        CurrentOpCode = (ushort)(Memory[PC] << 8 | Memory[PC + 1]);
        PC += 2;
        
        switch (CurrentOpCode & 0xF000)
        {
            case 0x0000 when CurrentOpCode == 0x00E0:
                Execute0x00E0();
                break;
            case 0xA000:
                Execute0xA000();
                break;
            case 0x6000:
                Execute0x6000();
                break;
            case 0x7000:
                Execute0x7000();
                break;
            default:
                throw new InvalidOperationException($"Unimplemented OpCode: {CurrentOpCode:x4}, PC: {PC}");
        }
    }

    #region OpCodes

    // 0x00E0
    private void Execute0x00E0() // Clear the display
    {
        Display = new byte[DisplaySize];
    }

    private void Execute0xA000() // Set I register = NNN
    {
        var nnn =  (ushort)(CurrentOpCode & 0x0FFF);
        I = nnn;

        PC += 2;
    }

    private void Execute0x6000() // Set Vx = KK
    {
        var x = (ushort) (CurrentOpCode & 0x0F00) >> 8;
        var kk = (byte) (CurrentOpCode & 0x00FF);
        V[x] = kk;

        PC += 2;
    }
    #endregion

    private void Execute0x7000()
    {
        var x = (ushort) (CurrentOpCode & 0x0F00) >> 8;
        var nn = (byte) (CurrentOpCode & 0x00FF);

        V[x] += nn;

        PC += 2;
    }

    // DXYN
    private void Execute0xD000()
    {
    }
    private void OutputDebugInformation()
    {
        Console.WriteLine($"PC: {PC}");
    }
}