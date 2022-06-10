using ChipSharp;

var rom = File.ReadAllBytes("Roms/IBMLogo.ch8");
var vm = Vm.CreateVm(rom);
vm.Run(false);
