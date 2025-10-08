using Engine.Types;
using Microsoft.Xna.Framework.Input;

namespace Engine.Managers.Input;

public static class DebugInput
{
    // Debug input (works during debug pause)
    public static Button ToggleDebugMode { get; private set; } = new(Keys.D);
    public static Button Pause { get; private set; } = new(Keys.Q);
    public static Button Step { get; private set; } = new(Keys.W);
    public static Button Fullscreen { get; private set; } = new(Keys.F);
    public static Button ZoomIn { get; private set; } = new(Keys.Add);
    public static Button ZoomOut { get; private set; } = new(Keys.Subtract);
    public static Button Mute { get; private set; } = new(Keys.M);
    public static Button Mask { get; private set; } = new(Keys.N);
    public static Button Kill { get; private set; } = new(Keys.K);
    public static Button Reset { get; private set; } = new(Keys.P);

    public static void Initialize()
    {
        InputHandler.DebugInputList.Add(ToggleDebugMode);
        InputHandler.DebugInputList.Add(Pause);
        InputHandler.DebugInputList.Add(Step);
        InputHandler.DebugInputList.Add(Fullscreen);
        InputHandler.DebugInputList.Add(ZoomIn);
        InputHandler.DebugInputList.Add(ZoomOut);
        InputHandler.DebugInputList.Add(Mute);
        InputHandler.DebugInputList.Add(Mask);
        InputHandler.DebugInputList.Add(Kill);
        InputHandler.DebugInputList.Add(Reset);
    }
}
