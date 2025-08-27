using Engine.ECS.Components.CombatHandling;
using Engine.ECS.Entities;
using Engine.ECS.Entities.EntityCreation;
using System.Linq;

namespace Engine.ECS.Components.ControlHandling.Behaviors.Targeting;

public class BehaviorTargetNearestEntity : Behavior
{
    public AlignmentType AlignmentType { get; set; }
    public EntityKind EntityKind { get; set; }

    public BehaviorTargetNearestEntity(AlignmentType alignmentType, EntityKind entityKind)
    {
        AlignmentType = alignmentType;
        EntityKind = entityKind;
    }

    public override void Action()
    {
        Owner.TargetPool ??= new TargetPool(Owner);
        Owner.TargetPool.TargetList.Clear();
        // ReSharper disable once ComplexConditionExpression
        var nearestEntity = EntityManager.GetAllEntities()
            .Where(entity => entity.Alignment?.Type == AlignmentType && entity != Owner)
            .Where(entity => (EntityKind == EntityKind.None || entity.EntityKind == EntityKind))
            .MinBy(entity => Owner.Position.GetDistanceTo(entity));
        Owner.TargetPool.TargetList.Add(nearestEntity);
    }
}
