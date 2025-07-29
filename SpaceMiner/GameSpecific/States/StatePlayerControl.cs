using Engine.ECS.Components.ControlHandling.States;
using Engine.ECS.Components.Spawning;
using Engine.ECS.Entities;
using Engine.Helpers;
using Engine.Managers.GlobalManagement;
using Microsoft.Xna.Framework;
using SpaceMiner.GameSpecific.Entities;
using System;
using System.Linq;

namespace SpaceMiner.GameSpecific.States;

public class StatePlayerControl : State
{
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
        // TODO: Diagonals need to be slower
        if (Owner.PlayerControl.Up)
            Owner.Speed.SetYSpeed(-Owner.Speed.MoveSpeed);
        else if (Owner.PlayerControl.Down)
            Owner.Speed.SetYSpeed(Owner.Speed.MoveSpeed);
        else
            Owner.Speed.SetYSpeed(0);

        if (Owner.PlayerControl.Left)
            Owner.Speed.SetXSpeed(-Owner.Speed.MoveSpeed);
        else if (Owner.PlayerControl.Right)
            Owner.Speed.SetXSpeed(Owner.Speed.MoveSpeed);
        else
            Owner.Speed.SetXSpeed(0);

        // Attack
        AttackFrame--;
        var fireRate = (int)(Owner.Shooter.AutoFireRate / (1 + Owner.StatsManager.GetAddedFloatStats(stats => stats.ExtraAttackSpeed)));
        if (Owner.PlayerControl.Button1Hold && AttackFrame <= 10)
            BufferedAttack = true;
        if (AttackFrame <= 0 && BufferedAttack)
        {
            AttackFrame = fireRate;
            Owner.Shooter.CheckToShoot();
            BufferedAttack = false;
        }
    }

    public override void PostProcessingBehavior()
    {
        //if (Owner.FrameHandler.CurrentFrame % 60 != 0)
        //    return; // Only run every second

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
            var entity = EntityManager.CreateEntityAt(typeof(ShipFlame), Owner.Position.Pixel + (0, 20) + (Owner.Facing.X * 0, yOffset));
            entity.Sprite.SetColor(color);
            entity.StateManager.CommandState(entity.StateManager.AutomaticStatesList.FirstOrDefault());
            var frameOffset = (int)((Math.Sin((GlobalManager.Values.Timer + initialFrame) / 3f) + 0.75) / 2 * 3) + addedFrame;
            entity.StateManager.CurrentState.Frame = frameOffset;
            entity.FrameHandler.SetFrame(frameOffset);
            entity.FrameHandler.CheckDurationEnd();
            entity.StateManager.CurrentState.UpdateAnimationFrame();
            entity.SetDrawOrder(drawOrder);
            var xSpeed = ((float)GetRandom.UnseededInt(500) / 1000 - 0.25f); // from -0.25 to 0.25
            entity.Speed.AddXSpeed(Owner.Facing.X * xSpeed);
        }
    }
}