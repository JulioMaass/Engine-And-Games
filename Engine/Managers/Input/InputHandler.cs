using Engine.Types;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Engine.Managers.Input;

public static class InputHandler
{
    public static KeyboardState KeyboardState;

    // Input lists
    public static List<Button> DebugInputList { get; } = new();
    public static List<Button> GameInputList { get; } = new();
    public static List<Button> EditorInputList { get; } = new();
    public static List<Button> SpecialInputList { get; } = new();

    // Special commands
    public static Button Ctrl { get; } = new(Keys.LeftControl);
    public static Button Shift { get; } = new(Keys.LeftShift);
    public static Button Alt { get; } = new(Keys.LeftAlt);

    public static void Initialize()
    {
        DebugInput.Initialize();
        GameInput.Initialize();
        EditorInput.Initialize();
        SpecialInputList.Add(Ctrl);
        SpecialInputList.Add(Shift);
        SpecialInputList.Add(Alt);
    }

    public static void Update()
    {
        KeyboardState = Keyboard.GetState();
        MouseHandler.Update();
        GamepadHandler.Update();
        UpdateInputList(SpecialInputList);
    }

    public static void UpdateInputList(List<Button> buttonList)
    {
        foreach (var button in buttonList)
            button.Update();
        foreach (var button in buttonList)
            button.PointedUpdate();
        foreach (var button in buttonList)
            button.UpdateFrame();
    }

    public static bool CtrlCommand(Button button) =>
        button.Pressed && Ctrl.Holding;

    public static bool ShortcutCommand(Button button) =>
        button.Pressed && !(Ctrl.Holding || Shift.Holding || Alt.Holding);
}
