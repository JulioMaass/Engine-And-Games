using Engine.ECS.Entities;
using Engine.Types;

namespace Engine.ECS.Components.ControlHandling.Conditions;

public class ConditionAttractedToPlayer : Condition
{
    protected override bool IsTrue()
    {
        if (EntityManager.PlayerEntity == null)
            return false;
        var attractionRadius = EntityManager.PlayerEntity.ItemGetter.TotalAttractionRadius;
        var currentDistance = IntVector2.GetDistance(
            Owner.Position.Pixel,
            EntityManager.PlayerEntity.Position.Pixel
        );
        return currentDistance < attractionRadius;
    }
}