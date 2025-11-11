using Engine.ECS.Components.CombatHandling;
using Engine.ECS.Components.ControlHandling;
using Engine.ECS.Components.ControlHandling.States;
using Engine.ECS.Entities;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;
using System.Linq;

namespace SpaceMiner.GameSpecific.States;

public class StateOre : State
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

    public override void StateSettingBehavior()
    {
    }

    public override void Behavior()
    {
        if (AttractedToPlayer())
        {
            TargetPlayer();
            Owner.MoveDirection.SetAngleDirectionTo(Owner.TargetPool.TargetList.FirstOrDefault());
            Owner.Speed.SetMoveSpeedToCurrentDirection(3);
        }
        else
            Owner.Speed.SetMoveSpeedToCurrentDirection(Owner.Speed.MoveSpeed);
    }

    private void TargetPlayer()
    {
        Owner.TargetPool ??= new TargetPool(Owner);
        Owner.TargetPool.TargetList.Clear();
        var entitiesOfKind = EntityManager.GetFilteredEntitiesFrom(EntityKind.Player);
        entitiesOfKind.Remove(Owner);
        var nearestEntity = entitiesOfKind
            .Where(entity => entity.Alignment?.Type == AlignmentType.Friendly)
            .MinBy(entity => Owner.Position.GetDistanceTo(entity));
        Owner.TargetPool.TargetList.Add(nearestEntity);
    }

    private bool AttractedToPlayer()
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