using Engine.Managers.Input;
using Microsoft.Xna.Framework.Input;

namespace Engine.Types;

public class Button
{
    // Inputs
    private Keys KeyMain { get; set; }
    private Keys KeySecondary { get; set; }
    private Keys KeyHardcoded { get; set; }
    private Buttons GamePadButton { get; set; }
    private Buttons GamePadButtonSecondary { get; set; }
    private int JoystickIndex { get; set; } // For controllers that are not identified as a gamepad (XInput)
    private int JoystickIndexSecondary { get; set; }
    private (int Axis, AxisSign Sign) JoystickAxisAndSign { get; set; }
    private MouseButton MouseButton { get; set; }
    private Button PointedButton { get; set; } // Allows a button to be triggered by other button (e.g., confirm triggered by shoot)

    // State tracking
    private bool PressedThisFrame { get; set; }
    private int PressFrame { get; set; }
    public bool Pressed => PressFrame == 1;
    public bool Holding => PressFrame > 0;

    public Button(Keys key, Buttons btn = Buttons.None, int joystickIndex = -1, (int, AxisSign) joystickAxisAndSign = default)
    {
        KeyMain = key;
        GamePadButton = btn;
        JoystickIndex = joystickIndex;
        JoystickAxisAndSign = joystickAxisAndSign;
    }

    public Button(Keys keyMain, Keys keySecondary, Keys keyHardcoded, Buttons btn, (int, AxisSign) joystickAxisAndSign)
    {
        KeyMain = keyMain;
        KeySecondary = keySecondary;
        KeyHardcoded = keyHardcoded;
        GamePadButton = btn;
        JoystickAxisAndSign = joystickAxisAndSign;
    }

    public Button(Button pointedButton, Keys keyMain, Keys keySecondary)
    {
        PointedButton = pointedButton;
        KeyMain = keyMain;
        KeySecondary = keySecondary;
    }

    public void RebindKey(Keys key) =>
        KeyMain = key;

    public void RebindMouseButton(MouseButton mouseButton) =>
        MouseButton = mouseButton;

    public void Update()
    {
        PressedThisFrame = false;
        // ReSharper disable once ComplexConditionExpression
        if (InputHandler.KeyboardState.IsKeyDown(KeyMain)
            || InputHandler.KeyboardState.IsKeyDown(KeySecondary)
            || InputHandler.KeyboardState.IsKeyDown(KeyHardcoded)
            || GamepadHandler.GamePadState.IsButtonDown(GamePadButton)
            || GamepadHandler.GamePadState.IsButtonDown(GamePadButtonSecondary)
            || GamepadHandler.JoystickButtonIsDown(JoystickIndex)
            || GamepadHandler.JoystickButtonIsDown(JoystickIndexSecondary)
            || GamepadHandler.JoystickAxisIsDown(JoystickAxisAndSign)
            || MouseHandler.MouseButtonPressed(MouseButton))
            PressedThisFrame = true;
    }

    public void PointedUpdate()
    {
        if (PointedButton is null)
            return;
        if (PointedButton.Pressed)
            PressedThisFrame = true;
    }

    public void UpdateFrame()
    {
        if (PressedThisFrame)
            AddFrame();
        else
            ResetFrame();
    }

    private void AddFrame() => PressFrame += 1;
    private void ResetFrame() => PressFrame = 0;
}