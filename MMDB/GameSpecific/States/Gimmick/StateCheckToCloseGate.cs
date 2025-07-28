using Engine.ECS.Components.ControlHandling.States;
using Engine.ECS.Components.PhysicsHandling;
using Engine.ECS.Entities;

namespace MMDB.GameSpecific.States.Gimmick;

public class StateCheckToCloseGate : State
{
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

    public override void Behavior()
    {
        if (EntityManager.PlayerEntity == null)
            return;

        var playerCollisionRectangle = EntityManager.PlayerEntity.CollisionBox.GetCollisionRectangle();
        var ownerCollisionRectangle = Owner.CollisionBox.GetCollisionRectangle();
        var xDifference = ownerCollisionRectangle.X + ownerCollisionRectangle.Width - playerCollisionRectangle.X;
        if (xDifference <= 0)
        {
            Owner.SolidBehavior.SolidType = SolidType.Solid;
            Owner.Sprite.IsVisible = true;
        }
    }
}
