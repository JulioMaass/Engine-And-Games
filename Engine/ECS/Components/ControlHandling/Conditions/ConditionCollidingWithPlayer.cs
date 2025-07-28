using Engine.ECS.Entities;

namespace Engine.ECS.Components.ControlHandling.Conditions;

public class ConditionCollidingWithPlayer : Condition
{
    protected override bool IsTrue()
    {
        return Owner.CollisionBox?.CollidesWithEntityPixel(EntityManager.PlayerEntity) == true;
    }
}
