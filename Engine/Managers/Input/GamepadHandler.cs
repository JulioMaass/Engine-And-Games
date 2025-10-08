using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

namespace Engine.Managers.Input;

public static class GamepadHandler
{
    public static GamePadState GamePadState { get; private set; }
    public static JoystickState JoystickState { get; private set; }
    public static int DeadZone { get; private set; } = 4096;

    public static void Update()
    {
        GamePadState = GamePad.GetState(0);
        JoystickState = Joystick.GetState(0);
    }

    public static bool JoystickButtonIsDown(int buttonIndex)
    {
        if (buttonIndex == -1)
            return false;
        if (JoystickState.IsConnected == false)
            return false;
        return JoystickState.Buttons[buttonIndex] == ButtonState.Pressed;
    }

    public static bool JoystickAxisIsDown((int Axis, AxisSign Sign) joystickAxisAndSign)
    {
        if (joystickAxisAndSign.Sign == 0)
            return false;
        if (JoystickState.IsConnected == false)
            return false;
        if (Math.Abs(JoystickState.Axes[joystickAxisAndSign.Axis]) <= DeadZone)
            return false;
        return Math.Sign(JoystickState.Axes[joystickAxisAndSign.Axis]) == (int)joystickAxisAndSign.Sign;
    }

    public static void PrintGamepadButtons(GamePadState gamePadState)
    {
        if (gamePadState.Buttons.A == ButtonState.Pressed) { Debug.WriteLine("Pressed A"); }
        if (gamePadState.Buttons.B == ButtonState.Pressed) { Debug.WriteLine("Pressed B"); }
        if (gamePadState.Buttons.X == ButtonState.Pressed) { Debug.WriteLine("Pressed X"); }
        if (gamePadState.Buttons.Y == ButtonState.Pressed) { Debug.WriteLine("Pressed Y"); }
        if (gamePadState.Buttons.Start == ButtonState.Pressed) { Debug.WriteLine("Pressed Start"); }
        if (gamePadState.Buttons.Back == ButtonState.Pressed) { Debug.WriteLine("Pressed Back"); }
        if (gamePadState.Buttons.LeftShoulder == ButtonState.Pressed) { Debug.WriteLine("Pressed LeftShoulder"); }
        if (gamePadState.Buttons.RightShoulder == ButtonState.Pressed) { Debug.WriteLine("Pressed RightShoulder"); }
        if (gamePadState.Buttons.LeftStick == ButtonState.Pressed) { Debug.WriteLine("Pressed LeftStick"); }
        if (gamePadState.Buttons.RightStick == ButtonState.Pressed) { Debug.WriteLine("Pressed RightStick"); }
        if (gamePadState.DPad.Up == ButtonState.Pressed) { Debug.WriteLine("Pressed DPad Up"); }
        if (gamePadState.DPad.Down == ButtonState.Pressed) { Debug.WriteLine("Pressed DPad Down"); }
        if (gamePadState.DPad.Left == ButtonState.Pressed) { Debug.WriteLine("Pressed DPad Left"); }
        if (gamePadState.DPad.Right == ButtonState.Pressed) { Debug.WriteLine("Pressed DPad Right"); }
        if (gamePadState.ThumbSticks.Left.X != 0.0f) { Debug.WriteLine("ThumbSticks.Left X = " + gamePadState.ThumbSticks.Left.X); }
        if (gamePadState.ThumbSticks.Left.Y != 0.0f) { Debug.WriteLine("ThumbSticks.Left Y = " + gamePadState.ThumbSticks.Left.Y); }
        if (gamePadState.ThumbSticks.Right.X != 0.0f) { Debug.WriteLine("ThumbSticks.Right X = " + gamePadState.ThumbSticks.Right.X); }
        if (gamePadState.ThumbSticks.Right.Y != 0.0f) { Debug.WriteLine("ThumbSticks.Right Y = " + gamePadState.ThumbSticks.Right.Y); }
        if (gamePadState.Triggers.Left != 0.0f) { Debug.WriteLine("Trigger.Left = " + gamePadState.Triggers.Left); }
        if (gamePadState.Triggers.Right != 0.0f) { Debug.WriteLine("Trigger.Right = " + gamePadState.Triggers.Right); }
    }
}

public enum AxisSign
{
    None = 0,
    Positive = 1,
    Negative = -1,
}