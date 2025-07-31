using Engine.ECS.Components.CombatHandling;
using Engine.ECS.Entities;
using System.Linq;

namespace Engine.ECS.Components.ControlHandling.Behaviors.Targeting;

public class BehaviorTargetNearestEntity : Behavior
{
    public AlignmentType AlignmentType { get; set; }

    public BehaviorTargetNearestEntity(AlignmentType alignmentType)
    {
        AlignmentType = alignmentType;
    }

    public override void Action()
    {
        Owner.TargetPool ??= new TargetPool(Owner);
        Owner.TargetPool.TargetList.Clear();
        var nearestEntity = EntityManager.GetAllEntities()
            .Where(entity => entity.Alignment?.Type == AlignmentType && entity != Owner)
            .MinBy(entity => Owner.Position.GetDistanceTo(entity));
        Owner.TargetPool.TargetList.Add(nearestEntity);
    }
}
