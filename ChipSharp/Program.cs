using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using Window = ChipSharp.Window;

var gameWindowSettings = new GameWindowSettings
{
    UpdateFrequency = 600
};

var nativeWindowSettings = new NativeWindowSettings
{
    Size = new Vector2i(1024, 512),
    Profile = ContextProfile.Compatability,
    Title = "Chip #"
};

var window = new Window(gameWindowSettings, nativeWindowSettings);
window.Run();