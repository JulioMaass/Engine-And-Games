using Candle.GameSpecific.Entities.BgObjects;
using Engine.ECS.Components.ControlHandling.States;
using Engine.ECS.Components.ItemsHandling;
using Engine.ECS.Entities;
using Engine.Helpers;
using Engine.Managers.GlobalManagement;
using System;
using System.Linq;
using Color = Microsoft.Xna.Framework.Color;

namespace Candle.GameSpecific.States;

public class StatePlayerControl : State
{
    private int DashFrame { get; set; }
    private int AttackFrame { get; set; }
    private bool BufferedAttack { get; set; }

    public override bool StartCondition()
    {
        return true;
    }

    public override bool KeepCondition()
    {
        return true;
    }

    public override bool PostProcessingKeepCondition()
    {
        return true;
    }

    public override void StateSettingBehavior()
    {
    }

    public override void Behavior()
    {
        // TODO: Make an actual state machine for the player
        // Direction // TODO: Make acceleration and deceleration (test 0.2acc and 0.3dec)
        if (Owner.PlayerControl.Left)
            Owner.Speed.SetXSpeed(-Owner.Speed.MoveSpeed);
        else if (Owner.PlayerControl.Right)
            Owner.Speed.SetXSpeed(Owner.Speed.MoveSpeed);
        else
            Owner.Speed.SetXSpeed(0);

        // Jump // TODO: Limit to 1 double jump
        if (Owner.PlayerControl.Button2Press)
        {
            if (Owner.Physics.SolidCollisionChecking.IsOnTopOfSolid() // On ground
                || StatsManager.CheckForUnlock(Owner, stats => stats.DoubleJump)) // Double Jump
                Owner.Speed.SetYSpeed(-Owner.Speed.JumpSpeed);
        }
        else if (!Owner.PlayerControl.Button2Hold && Owner.Speed.Value.Y < 0)
            Owner.Speed.SetYSpeed(0);

        // Dash // TODO: Limit to 1 per jump
        if (Owner.PlayerControl.Button3Press && StatsManager.CheckForUnlock(Owner, stats => stats.Dash))
            DashFrame = 15;
        if (DashFrame > 0)
        {
            DashFrame--;
            Owner.Speed.SetXSpeed(Owner.Speed.DashSpeed * Owner.Facing.X);
            Owner.Speed.SetYSpeed(0);
        }

        // Attack
        AttackFrame--;
        if (Owner.PlayerControl.Button1Press && AttackFrame <= 10)
            BufferedAttack = true;
        if (AttackFrame <= 0 && BufferedAttack)
        {
            AttackFrame = 20;
            Owner.Shooter.CheckToShoot();
            BufferedAttack = false;
        }

        // Tick life
        var burnSpeed = 60;
        burnSpeed *= StatsManager.GetMultipliedStats(Owner, stats => stats.BurningRateMultiplier, true, true, false);
        if (GlobalManager.Values.Timer % burnSpeed == 0)
            Owner.DamageTaker.CurrentHp.Amount -= 1;

        // Wax drops
        if (GlobalManager.Values.Timer % 120 == 0)
            EntityManager.CreateEntityAt(typeof(FxWaxDrip), Owner.Position.Pixel + (Owner.Facing.X * -4, 0));
    }

    public override void PostProcessingBehavior()
    {
        // Candle flame
        var color1 = CustomColor.FlameOrange;
        GenerateFlameFx(-12, 14, color1, 0, 0);
        GenerateFlameFx(-13, 10, color1, 0, 0);
        GenerateFlameFx(-14, 7, color1, 0, 0);
        GenerateFlameFx(-15, 3, color1, 1, 0);
        GenerateFlameFx(-16, 0, color1, 1, 0);

        var color2 = CustomColor.FlameYellow;
        GenerateFlameFx(-12, 14, color2, 1, 1);
        GenerateFlameFx(-13, 10, color2, 1, 1);
        GenerateFlameFx(-14, 7, color2, 2, 1);
        GenerateFlameFx(-15, 3, color2, 2, 1);
        GenerateFlameFx(-16, 0, color2, 3, 1);

        var color3 = CustomColor.PicoWhite;
        GenerateFlameFx(-12, 14, color3, 2, 2);
        GenerateFlameFx(-13, 10, color3, 3, 2);
        GenerateFlameFx(-14, 7, color3, 3, 2);
        GenerateFlameFx(-15, 3, color3, 4, 2);
        GenerateFlameFx(-16, 0, color3, 4, 2);

        return;

        void GenerateFlameFx(int yOffset, int initialFrame, Color color, int addedFrame, int drawOrder)
        {
            var entity = EntityManager.CreateEntityAt(typeof(CandleFlame), Owner.Position.Pixel + (Owner.Facing.X * 0, yOffset));
            entity.Sprite.SetColor(color);
            entity.StateManager.CommandState(entity.StateManager.AutomaticStatesList.FirstOrDefault());
            var frameOffset = (int)((Math.Sin((GlobalManager.Values.Timer + initialFrame) / 3f) + 0.75) / 2 * 3) + addedFrame;
            entity.StateManager.CurrentState.Frame = frameOffset;
            entity.FrameHandler.SetFrame(frameOffset);
            entity.SetDrawOrder(drawOrder);
            entity.Speed.AddXSpeed(Owner.Facing.X * -0.25f);
        }
    }
}