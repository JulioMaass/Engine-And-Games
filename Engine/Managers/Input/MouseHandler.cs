using Engine.Main;
using Engine.Managers.Graphics;
using Engine.Types;
using Microsoft.Xna.Framework.Input;

namespace Engine.Managers.Input;

public static class MouseHandler
{
    private static MouseState _mouseState;

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

    public static void Update()
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

    public static bool MouseButtonPressed(MouseButton mouseButton) =>
        mouseButton switch
        {
            MouseButton.Left => MouseLeftHold,
            MouseButton.Right => MouseRightHold,
            _ => false
        };

}

public enum MouseButton
{
    None,
    Left,
    Right,
}
