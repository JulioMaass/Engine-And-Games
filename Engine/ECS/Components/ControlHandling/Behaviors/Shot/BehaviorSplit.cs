using Engine.ECS.Entities;

namespace Engine.ECS.Components.ControlHandling.Behaviors.Shot;

public class BehaviorSplit : Behavior
{
    public int SplitLevel { get; set; }

    public BehaviorSplit(int splitLevel)
    {
        SplitLevel = splitLevel;
    }

    public override void Action()
    {
        if (SplitLevel <= 0)
            return;

        // Create split shots
        var type = Owner.GetType();
        var splitShot1 = EntityManager.CreateEntityAt(type, Owner.Position.Pixel);
        var splitShot2 = EntityManager.CreateEntityAt(type, Owner.Position.Pixel);
        splitShot1.DamageDealer.CopyHitListFrom(Owner);
        splitShot2.DamageDealer.CopyHitListFrom(Owner);

        // Apply modifiers from original shot
        splitShot1.AddShotProperties();
        splitShot2.AddShotProperties();
        splitShot1.ShotProperties.CopyShotModifiers(Owner, 1);
        splitShot2.ShotProperties.CopyShotModifiers(Owner, 1);

        // Set movement
        splitShot1.AddMoveDirection(Owner.MoveDirection.Angle.Value + 45000);
        splitShot2.AddMoveDirection(Owner.MoveDirection.Angle.Value - 45000);
        splitShot1.Speed.SetMoveSpeedToCurrentDirection();
        splitShot2.Speed.SetMoveSpeedToCurrentDirection();
        splitShot1.Alignment.OwningEntity = Owner.Alignment.OwningEntity;
        splitShot2.Alignment.OwningEntity = Owner.Alignment.OwningEntity;

        EntityManager.MarkEntityForDeletion(Owner);
    }
}
