using Engine.ECS.Entities.EntityCreation;
using Engine.Managers;

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
        Up = Input.Up.Holding && !Input.Down.Holding;
        Down = Input.Down.Holding && !Input.Up.Holding;
        Left = Input.Left.Holding && !Input.Right.Holding;
        Right = Input.Right.Holding && !Input.Left.Holding;

        Button1Press = Input.Button1.Pressed;
        Button1Hold = Input.Button1.Holding;
        Button2Press = Input.Button2.Pressed;
        Button2Hold = Input.Button2.Holding;
        Button3Press = Input.Button3.Pressed;
        Button3Hold = Input.Button3.Holding;
        Button4Press = Input.Button4.Pressed;
        Button4Hold = Input.Button4.Holding;

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
