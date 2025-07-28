using Engine.ECS.Components.ControlHandling.SecondaryStates;
using Engine.ECS.Components.ControlHandling.States;
using Engine.Helpers;
using Engine.Main;
using Engine.Types;

namespace MMDB.GameSpecific.States.Player;

public class StateClimb : State
{
    public override bool AllowSecondaryState { get; protected set; } = true;
    private float ClimbSpeed { get; }
    private int GrabDistance => 3;
    private int TopSkip => 8;

    public StateClimb(float climbSpeed)
    {
        ClimbSpeed = climbSpeed;
        UpdatesFrameOnTransitions = true;
    }

    public override bool StartCondition()
    {
        var upCondition = Owner.PlayerControl.Up
            && Owner.Physics.StairHandling.CanGrabStair(GrabDistance, -Owner.CollisionBox.MaskTop, Owner.CollisionBox.MaskBottom);
        var downCondition = Owner.PlayerControl.Down
            && Owner.Physics.StairHandling.CanGrabStair(GrabDistance, Owner.CollisionBox.MaskBottom + 1, Owner.CollisionBox.MaskBottom + 1)
            && Owner.Physics.SolidCollisionChecking.IsOnTopOfSolid()
            && Owner.StateManager.CurrentState is not StateJump;

        return (upCondition || downCondition) && !Owner.CollisionBox.DefaultHitboxCollidesWithSolid();
    }

    public override bool KeepCondition()
    {
        var isOnStairBottom = Owner.Physics.SolidCollisionChecking.IsOnTopOfSolid()
            && !Owner.Physics.StairHandling.CanGrabStair(GrabDistance, Owner.CollisionBox.MaskBottom + 1, Owner.CollisionBox.MaskBottom + 1);
        var jumpingOff = Owner.PlayerControl.Button2Press && !Owner.PlayerControl.Up;

        return Owner.Physics.StairHandling.IsOverlappingWithStair() && !isOnStairBottom && !jumpingOff;
    }

    public override bool PostProcessingKeepCondition()
    {
        return KeepCondition();
    }

    public override void StateSettingBehavior()
    {
        AnimationFrame = 0;

        // X
        if (Owner.PlayerControl.Up)
        {
            Owner.Physics.FreeMovement.SetPixelX(Owner.Physics.StairHandling.GetNearestStairX(GrabDistance, 0));
            Owner.Physics.FreeMovement.SetPixelX(Owner.Physics.StairHandling.GetNearestStairX(GrabDistance, -Owner.CollisionBox.MaskTop));
            Owner.Physics.FreeMovement.SetPixelX(Owner.Physics.StairHandling.GetNearestStairX(GrabDistance, Owner.CollisionBox.MaskBottom));
        }
        if (Owner.PlayerControl.Down)
            Owner.Physics.FreeMovement.SetPixelX(Owner.Physics.StairHandling.GetNearestStairX(GrabDistance, Owner.CollisionBox.MaskBottom + 1));

        // Y
        if (Owner.PlayerControl.Down)
        {
            Owner.Physics.FreeMovement.SetPixelY(Owner.Position.Pixel.Y + TopSkip);
        }
    }

    public override void Behavior()
    {
        // X speed
        Owner.Speed.SetXSpeed(0);

        // Y speed
        if (Owner.PlayerControl.Up)
        {
            Owner.Speed.SetYSpeed(-ClimbSpeed);
            CheckToSkipTop();
        }
        else if (Owner.PlayerControl.Down)
            Owner.Speed.SetYSpeed(ClimbSpeed);
        else
            Owner.Speed.SetYSpeed(0);

        // Secondary state behavior
        if (Owner.StateManager.CurrentSecondaryState is not SecondaryStateNone)
        {
            AnimationFrame = 0;
            Owner.Speed.SetYSpeed(0);
        }
    }

    private void CheckToSkipTop()
    {
        if (Owner.Physics.StairHandling.CanGrabStair(GrabDistance, -Owner.CollisionBox.MaskTop, Owner.CollisionBox.MaskBottom - TopSkip)) return;

        Owner.Speed.SetYSpeed(0);
        Owner.Physics.FreeMovement.SetPixelY(Owner.Position.Pixel.Y.RoundDownDivision(Settings.TileSize.Y) * Settings.TileSize.Y);
    }

    public override void UpdateAnimationFrame()
    {
        if (Owner.PlayerControl.Up || Owner.PlayerControl.Down)
            AnimationFrame++;
        else if (Owner.PlayerControl.Right || Owner.PlayerControl.Left)
            AnimationFrame = 0;
    }

    public override bool GetFlipAnimation()
    {
        var loopedAnimationFrame = AnimationFrame % (SpritePattern.Count * 2);
        return loopedAnimationFrame >= SpritePattern.Count;
    }

    public override IntVector2 GetAnimationOffset()
    {
        var offset = IntVector2.Zero;

        var loopedAnimationFrame = AnimationFrame % (SpritePattern.Count * 2);
        if (loopedAnimationFrame >= SpritePattern.Count)
            offset.X = 1;
        if (Owner.Sprite.IsFlipped)
            offset *= -1;

        return offset;
    }
}