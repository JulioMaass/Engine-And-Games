using Engine.Types;
using Microsoft.Xna.Framework.Input;

namespace Engine.Managers.Input;

public static class GameInput
{
    // Game input (doesn't work during debug pause)
    public static Button Up { get; private set; } = new(Keys.NumPad5, Keys.H, Keys.Up, Buttons.DPadUp, (1, AxisSign.Negative));
    public static Button Down { get; private set; } = new(Keys.NumPad2, Keys.N, Keys.Down, Buttons.DPadDown, (1, AxisSign.Positive));
    public static Button Left { get; private set; } = new(Keys.NumPad1, Keys.B, Keys.Left, Buttons.DPadLeft, (0, AxisSign.Negative));
    public static Button Right { get; private set; } = new(Keys.NumPad3, Keys.M, Keys.Right, Buttons.DPadRight, (0, AxisSign.Positive));

    public static Button Button1 { get; private set; } = new(Keys.Z, Buttons.A, 3);
    public static Button Button2 { get; private set; } = new(Keys.X, Buttons.B, 2);
    public static Button Button3 { get; private set; } = new(Keys.C, Buttons.X, 1);
    public static Button Button4 { get; private set; } = new(Keys.V, Buttons.Y, 0);

    public static Button L { get; private set; } = new(Keys.A, Buttons.LeftShoulder, 4);
    public static Button R { get; private set; } = new(Keys.S, Buttons.RightShoulder, 5);
    public static Button Start { get; private set; } = new(Keys.Enter, Buttons.Start, 9);
    public static Button Select { get; private set; } = new(Keys.Back, Buttons.Back, 8);

    public static Button Confirm { get; } = new(Button1, Keys.Enter, Keys.Space);
    public static Button Cancel { get; } = new(Button2, Keys.Escape, Keys.Back);


    public static void Initialize()
    {
        InputHandler.GameInputList.Add(Up);
        InputHandler.GameInputList.Add(Down);
        InputHandler.GameInputList.Add(Left);
        InputHandler.GameInputList.Add(Right);

        InputHandler.GameInputList.Add(Button1);
        InputHandler.GameInputList.Add(Button2);
        InputHandler.GameInputList.Add(Button3);
        InputHandler.GameInputList.Add(Button4);

        InputHandler.GameInputList.Add(L);
        InputHandler.GameInputList.Add(R);
        InputHandler.GameInputList.Add(Start);
        InputHandler.GameInputList.Add(Select);

        InputHandler.GameInputList.Add(Confirm);
        InputHandler.GameInputList.Add(Cancel);
    }
}

