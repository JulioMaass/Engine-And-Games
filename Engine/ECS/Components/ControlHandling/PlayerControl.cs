using Engine.ECS.Entities.EntityCreation;
using Engine.Managers.Input;

namespace Engine.ECS.Components.ControlHandling;

public class PlayerControl : Component
{
    // PLAYER CONTROLS
    public bool Up { get; private set; }
    public bool Down { get; private set; }
    public bool Left { get; private set; }
    public bool Right { get; private set; }
    public bool Button1Press { get; private set; }
    public bool Button1Hold { get; private set; }
    public bool Button2Press { get; private set; }
    public bool Button2Hold { get; private set; }
    public bool Button3Press { get; private set; }
    public bool Button3Hold { get; private set; }
    public bool Button4Press { get; private set; }
    public bool Button4Hold { get; private set; }

    // Consequential controls
    public int DirectionX { get; private set; }
    public int DirectionY { get; private set; }
    public bool SwitchLeft { get; private set; }
    public bool SwitchRight { get; private set; }
    public bool SwitchReset { get; private set; }

    // Automatic controls
    public class AutomaticControl
    {
        private bool _isOn;

        public void TurnOn()
        {
            _isOn = true;
        }

        public bool Check()
        {
            var isOn = _isOn;
            _isOn = false;
            return isOn;
        }
    }

    public AutomaticControl GotHurt { get; } = new();
    public AutomaticControl StartTeleporting { get; } = new();

    public PlayerControl(Entity owner)
    {
        Owner = owner;
    }

    public void Update()
    {
        // Update controls
        Up = GameInput.Up.Holding && !GameInput.Down.Holding;
        Down = GameInput.Down.Holding && !GameInput.Up.Holding;
        Left = GameInput.Left.Holding && !GameInput.Right.Holding;
        Right = GameInput.Right.Holding && !GameInput.Left.Holding;

        // Update weapon switching
        SwitchLeft = GameInput.L.Pressed;
        SwitchRight = GameInput.R.Pressed;
        SwitchReset = GameInput.L.Holding && GameInput.R.Holding;

        Button1Press = GameInput.Button1.Pressed;
        Button1Hold = GameInput.Button1.Holding;
        Button2Press = GameInput.Button2.Pressed;
        Button2Hold = GameInput.Button2.Holding;
        Button3Press = GameInput.Button3.Pressed;
        Button3Hold = GameInput.Button3.Holding;
        Button4Press = GameInput.Button4.Pressed;
        Button4Hold = GameInput.Button4.Holding;

        UpdateDirection();
    }

    private void UpdateDirection()
    {
        // Update X direction
        if (Left)
            DirectionX = -1;
        else if (Right)
            DirectionX = 1;
        else
            DirectionX = 0;
        // Update Y direction
        if (Up)
            DirectionY = -1;
        else if (Down)
            DirectionY = 1;
        else
            DirectionY = 0;
    }
}
