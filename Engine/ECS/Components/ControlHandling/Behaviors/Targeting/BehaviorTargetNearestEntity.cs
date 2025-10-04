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

        var entitiesOfKind = EntityManager.GetFilteredEntitiesFrom(EntityKind);
        entitiesOfKind.Remove(Owner);
        var nearestEntity = entitiesOfKind
            .Where(entity => entity.Alignment?.Type == AlignmentType)
            .MinBy(entity => Owner.Position.GetDistanceTo(entity));
        Owner.TargetPool.TargetList.Add(nearestEntity);
    }
}
