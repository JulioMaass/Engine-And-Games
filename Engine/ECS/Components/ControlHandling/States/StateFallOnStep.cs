using Engine.ECS.Entities;

namespace Engine.ECS.Components.ControlHandling.States;

public class StateFallOnStep : State
{
    public int Delay { get; set; } = 0;

    public StateFallOnStep(int delay = 0)
    {
        Delay = delay;
    }

    public override bool StartCondition()
    {
        if (EntityManager.PlayerEntity == null)
            return false;
        var playerIsFalling = EntityManager.PlayerEntity.Speed.Y >= 0;
        var isColliding = Owner.Physics.EntityCollisionChecking.CollidesWithEntityAtPixel(EntityManager.PlayerEntity, Owner.Position.Pixel);
        var isCollidingUp = Owner.Physics.EntityCollisionChecking.CollidesWithEntityAtPixel(EntityManager.PlayerEntity, Owner.Position.Pixel - (0, 1));
        return playerIsFalling && !isColliding && isCollidingUp;
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
        Owner.Speed.SetYSpeed(0);
    }

    public override void Behavior()
    {
        if (Frame >= Delay)
            Owner.Gravity.IsAffectedByGravity = true;
    }
}