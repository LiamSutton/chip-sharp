using ChipSharp;

var vm = Vm.CreateVm(new byte[] {0});
vm.Run(false);
vm.Reset();
vm.Run(true);