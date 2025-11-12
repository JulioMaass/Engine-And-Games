using Engine.ECS.Components.ControlHandling.States;
using Engine.ECS.Entities;
using Engine.Types;
using MMDB.GameSpecific.Entities.Chars;

namespace MMDB.GameSpecific.States.Player;

public class StateSlide : State
{
    private int Duration { get; }

    public StateSlide(int duration)
    {
        Duration = duration;
        SetCustomHitbox(new IntRectangle(8, 0, 16, 16));
    }

    public override bool StartCondition()
    {
        return Owner.PlayerControl.Button3Press && Owner.Physics.SolidCollisionChecking.IsOnTopOfSolid();
    }

    public override bool KeepCondition()
    {
        var isOnTopOfSolid = Owner.Physics.SolidCollisionChecking.IsOnTopOfSolid();
        var nextFrameIsSliding = Frame < Duration;
        var changedDirection = Owner.PlayerControl.DirectionX != 0 && Owner.PlayerControl.DirectionX != Owner.Facing.X;
        var canStandUp = !Owner.CollisionBox.DefaultHitboxCollidesWithSolid();

        if (!isOnTopOfSolid)
            return false;
        if (!canStandUp)
            return true;
        return nextFrameIsSliding && !changedDirection;
    }

    public override bool PostProcessingKeepCondition()
    {
        return Owner.Physics.SolidCollisionChecking.IsOnTopOfSolid();
    }

    public override void StateSettingBehavior()
    {
        // Create slide cloud
        var position = Owner.Position.Pixel + IntVector2.New(-10 * Owner.Facing.X, 12);
        var slideCloud = EntityManager.CreateEntityAt<SlideCloud>(position);
        slideCloud.Facing.CopyFacingFrom(Owner);
    }

    public override void Behavior()
    {
        Owner.Speed.SetXSpeed(Owner.Facing.X * Owner.Speed.DashSpeed);
    }
}