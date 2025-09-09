using Engine.Main;
using Engine.Managers.Graphics;
using Engine.Types;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Engine.Managers;

public static class Input
{
    public static KeyboardState KeyboardState;
    private static GamePadState _gamePadState;
    private static MouseState _mouseState;
    private static JoystickState _joystickState;
    private static int _deadZone = 4096;

    // MOUSE HANDLING
    // Left Button
    public static bool MouseLeftPressed;
    public static bool MouseLeftHold;
    private static bool _lastMouseLeftHold;
    public static bool MouseLeftReleased;
    // Right Button
    public static bool MouseRightPressed;
    public static bool MouseRightHold;
    private static bool _lastMouseRightHold;
    public static bool MouseRightReleased;
    // Position
    public static IntVector2 MousePositionOnGame;
    private static IntVector2 _mousePositionOnScreen;
    private static IntVector2 _lastMousePositionOnScreen;

    public class Button
    {
        public readonly InputType InputType;
        private int _pressFrame;
        public Keys Key { get; private set; }
        public Buttons GamePadButton { get; private set; }
        public int JoystickIndex { get; private set; } // For controllers that are not identified as a gamepad (XInput)
        public (int Axis, AxisSign Sign) JoystickAxisAndSign { get; private set; }
        public MouseButton MouseButton { get; set; }
        public bool Pressed => _pressFrame == 1;
        public bool Holding => _pressFrame > 0;
        public void AddFrame() => _pressFrame += 1;
        public void ResetFrame() => _pressFrame = 0;
        public Button(InputType inputType, Keys key, Buttons btn = Buttons.None, int joystickIndex = -1, (int, AxisSign) joystickAxisAndSign = default)
        {
            InputType = inputType;
            Key = key;
            GamePadButton = btn;
            JoystickIndex = joystickIndex;
            JoystickAxisAndSign = joystickAxisAndSign;
            InputList.Add(this);
        }

        public void RebindKey(Keys key) =>
            Key = key;
    }

    // Game input (doesn't work during debug pause)
    private static readonly List<Button> InputList = new();
    public static Button Up { get; private set; } = new(InputType.Game, Keys.NumPad5, Buttons.DPadUp, -1, (1, AxisSign.Negative));
    public static Button Down { get; private set; } = new(InputType.Game, Keys.NumPad2, Buttons.DPadDown, -1, (1, AxisSign.Positive));
    public static Button Left { get; private set; } = new(InputType.Game, Keys.NumPad1, Buttons.DPadLeft, -1, (0, AxisSign.Negative));
    public static Button Right { get; private set; } = new(InputType.Game, Keys.NumPad3, Buttons.DPadRight, -1, (0, AxisSign.Positive));
    public static Button Button1 { get; private set; } = new(InputType.Game, Keys.Z, Buttons.A, 3);
    public static Button Button2 { get; private set; } = new(InputType.Game, Keys.X, Buttons.B, 2);
    public static Button Button3 { get; private set; } = new(InputType.Game, Keys.C, Buttons.X, 1);
    public static Button Button4 { get; private set; } = new(InputType.Game, Keys.V, Buttons.Y, 0);
    public static Button L { get; private set; } = new(InputType.Game, Keys.A, Buttons.LeftShoulder, 4);
    public static Button R { get; private set; } = new(InputType.Game, Keys.S, Buttons.RightShoulder, 5);
    public static Button Start { get; private set; } = new(InputType.Game, Keys.Enter, Buttons.Start, 9);
    public static Button Select { get; private set; } = new(InputType.Game, Keys.Back, Buttons.Back, 8);

    // Debug input (works during debug pause)
    public static Button ToggleDebugMode { get; private set; } = new(InputType.Debug, Keys.D);
    public static Button Pause { get; private set; } = new(InputType.Debug, Keys.Q);
    public static Button Step { get; private set; } = new(InputType.Debug, Keys.W);
    public static Button Fullscreen { get; private set; } = new(InputType.Debug, Keys.F);
    public static Button ZoomIn { get; private set; } = new(InputType.Debug, Keys.Add);
    public static Button ZoomOut { get; private set; } = new(InputType.Debug, Keys.Subtract);
    public static Button Mute { get; private set; } = new(InputType.Debug, Keys.M);
    public static Button Mask { get; private set; } = new(InputType.Debug, Keys.N);
    public static Button Kill { get; private set; } = new(InputType.Debug, Keys.K);
    public static Button Reset { get; private set; } = new(InputType.Debug, Keys.P);

    // Stage editor input
    public static Button Save { get; private set; } = new(InputType.Debug, Keys.S);
    public static Button Edit { get; private set; } = new(InputType.Debug, Keys.E);
    public static Button Panning { get; private set; } = new(InputType.Debug, Keys.Space);
    public static Button LaptopMode { get; private set; } = new(InputType.Debug, Keys.L);
    // Tile mode
    public static Button TileMode { get; private set; } = new(InputType.Debug, Keys.T);
    public static Button TileBrushTool { get; private set; } = new(InputType.Debug, Keys.D);
    public static Button TileRectangleTool { get; private set; } = new(InputType.Debug, Keys.C);
    // Entity mode
    public static Button EntityMode { get; private set; } = new(InputType.Debug, Keys.E);
    public static Button EntityEditTool { get; private set; } = new(InputType.Debug, Keys.D);
    public static Button EntityPlaceTool { get; private set; } = new(InputType.Debug, Keys.E);
    public static Button SelectionUp { get; private set; } = new(InputType.Debug, Keys.Up);
    public static Button SelectionDown { get; private set; } = new(InputType.Debug, Keys.Down);
    public static Button Enter { get; private set; } = new(InputType.Debug, Keys.Enter);
    // Room mode
    public static Button RoomMode { get; private set; } = new(InputType.Debug, Keys.R);
    public static Button RoomMoveTool { get; private set; } = new(InputType.Debug, Keys.M);
    public static Button RoomCutAndGlueTool { get; private set; } = new(InputType.Debug, Keys.C);
    public static Button RoomResizeTool { get; private set; } = new(InputType.Debug, Keys.S);

    // Special commands
    public static Button Ctrl { get; } = new(InputType.Debug, Keys.LeftControl);
    public static Button Shift { get; } = new(InputType.Debug, Keys.LeftShift);
    public static Button Alt { get; } = new(InputType.Debug, Keys.LeftAlt);

    public static void UpdateInputState()
    {
        KeyboardState = Keyboard.GetState();
        _gamePadState = GamePad.GetState(0);
        _joystickState = Joystick.GetState(0);
    }

    public static void UpdateDebugInput()
    {
        foreach (var button in InputList.Where(button => button.InputType == InputType.Debug))
            UpdateButton(button);
        UpdateMouse();
    }

    public static void UpdateGameInput()
    {
        foreach (var button in InputList.Where(button => button.InputType == InputType.Game))
            UpdateButton(button);
    }

    private static void UpdateMouse()
    {
        UpdateMouseLastPosition();

        _mouseState = Mouse.GetState();

        UpdateMouseButtons();
        UpdateMousePositionOnGame();
    }

    private static void UpdateMouseButtons()
    {
        // Left mouse button
        _lastMouseLeftHold = MouseLeftHold;
        MouseLeftHold = _mouseState.LeftButton == ButtonState.Pressed;
        MouseLeftReleased = !MouseLeftHold && _lastMouseLeftHold;
        if (MouseLeftHold && !_lastMouseLeftHold)
            MouseLeftPressed = true;
        else
            MouseLeftPressed = false;

        // Right mouse button
        _lastMouseRightHold = MouseRightHold;
        MouseRightHold = _mouseState.RightButton == ButtonState.Pressed;
        MouseRightReleased = !MouseRightHold && _lastMouseRightHold;
        if (MouseRightHold && !_lastMouseRightHold)
            MouseRightPressed = true;
        else
            MouseRightPressed = false;
        _lastMouseRightHold = MouseRightHold;
    }

    private static void UpdateMousePositionOnGame()
    {
        _mousePositionOnScreen = (_mouseState.Position - Camera.FullScreenOffset) / Camera.ZoomScale;
        MousePositionOnGame = _mousePositionOnScreen + Camera.ZoomPanning;
    }

    public static bool ClickedOnGameScreen()
    {
        if (!MouseLeftHold && !MouseRightHold) return false;
        if (!GameManager.Game.IsActive) return false;

        return MouseIsOnGameScreen();
    }

    public static bool MouseIsOnGameScreen()
    {
        var mousePosition = _mouseState.Position - Camera.FullScreenOffset;
        var screen = new IntRectangle(0, 0, Settings.ScreenScaledSize);
        return screen.Contains(mousePosition);
    }

    public static bool ClickedOnEditingMenu()
    {
        if (!MouseLeftHold && !MouseRightHold) return false;
        if (!GameManager.Game.IsActive) return false;

        var mousePosition = _mouseState.Position - Camera.FullScreenOffset;
        var menuRectangle = new IntRectangle(Settings.ScreenScaledSize.Width, 0, Settings.EditingMenuWidth, Settings.ScreenScaledSize.Height);
        return menuRectangle.Contains(mousePosition);
    }

    public static IntVector2 MousePositionOnMenu()
    {
        return _mouseState.Position - Camera.FullScreenOffset - Settings.EditingMenuPosition;
    }

    private static void UpdateMouseLastPosition()
    {
        _lastMousePositionOnScreen = _mousePositionOnScreen;
    }

    public static IntVector2 GetMouseMovement()
    {
        var movement = _mousePositionOnScreen - _lastMousePositionOnScreen;
        return movement;
    }

    private static void UpdateButton(Button button)
    {
        if (KeyboardState.IsKeyDown(button.Key) || _gamePadState.IsButtonDown(button.GamePadButton)
            || JoystickButtonIsDown(button.JoystickIndex) || JoystickAxisIsDown(button.JoystickAxisAndSign) || MouseButtonPressed(button.MouseButton))
            button.AddFrame();
        else
            button.ResetFrame();
    }

    private static bool JoystickButtonIsDown(int buttonIndex)
    {
        if (buttonIndex == -1)
            return false;
        if (_joystickState.IsConnected == false)
            return false;
        return _joystickState.Buttons[buttonIndex] == ButtonState.Pressed;
    }

    private static bool JoystickAxisIsDown((int Axis, AxisSign Sign) joystickAxisAndSign)
    {
        if (joystickAxisAndSign.Sign == 0)
            return false;
        if (_joystickState.IsConnected == false)
            return false;
        if (Math.Abs(_joystickState.Axes[joystickAxisAndSign.Axis]) <= _deadZone)
            return false;
        return Math.Sign(_joystickState.Axes[joystickAxisAndSign.Axis]) == (int)joystickAxisAndSign.Sign;
    }

    private static bool MouseButtonPressed(MouseButton mouseButton) =>
        mouseButton switch
        {
            MouseButton.Left => MouseLeftHold,
            MouseButton.Right => MouseRightHold,
            _ => false
        };

    public static bool CtrlCommand(Button button) =>
        button.Pressed && Ctrl.Holding;

    public static bool ShortcutCommand(Button button) =>
        button.Pressed && !(Ctrl.Holding || Shift.Holding || Alt.Holding);

    // ReSharper disable once UnusedMember.Global
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

public enum InputType
{
    Debug,
    Game
}

public enum MouseButton
{
    None,
    Left,
    Right,
}

public enum AxisSign
{
    None = 0,
    Positive = 1,
    Negative = -1,
}