using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace ChipSharp.Tests;

public class VmTest
{
    private Vm vm;
    private List<byte> displayList;
    private ushort romStart;
    private int displaySize;

    [SetUp]
    public void Setup()
    { 
        vm = Vm.CreateVm(System.Array.Empty<byte>());
        displayList = vm.Display.ToList();
        romStart = 0x200;
        displaySize = 2048;
    }

    [Test]
    public void Reset_Initialises_Variables_Correctly()
    {
        vm.Reset();
        Assert.AreEqual(romStart, vm.PC, $"PC should be {romStart} after the vm is reset.");
        Assert.AreEqual(0, vm.I, "I should be 0 after the vm is reset.");
        Assert.AreEqual(0, vm.SP, "SP should be 0 after the vm is reset.");
        Assert.AreEqual(0, displayList.Sum(x => x), "All elements in display should be 0 after the vm is reset.");
    }

    [Test]
    public void Display_Has_Correct_Number_Of_Elements()
    {
        Assert.AreEqual(displaySize, displayList.Count, $"display should always have exactly {displaySize} elements.");
    }
}