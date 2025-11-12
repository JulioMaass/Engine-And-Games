using Engine.ECS.Components.ControlHandling.Behaviors;
using Engine.ECS.Entities;
using MMDB.GameSpecific.Entities.Weapons;

namespace MMDB.GameSpecific.Behaviors;

public class BehaviorJungleSeedCommandSplitting : Behavior
{
    public override void Action()
    {
        if (Owner.FrameHandler.CurrentFrame <= 1)
            return;

        var playerControl = Owner.Alignment.OwningEntity.PlayerControl;
        if (playerControl == null)
            return;
        if (!playerControl.Button1Press)
            return;
        if (((JungleSeed)Owner).SplitLevel <= 0)
            return;

        EntityManager.MarkEntityForDeletion(Owner);

        var seed1 = (JungleSeed)EntityManager.CreateEntityAt(typeof(JungleSeed), Owner.Position.Pixel);
        var seed2 = (JungleSeed)EntityManager.CreateEntityAt(typeof(JungleSeed), Owner.Position.Pixel);
        seed1.SetLevel(((JungleSeed)Owner).SplitLevel - 1);
        seed2.SetLevel(((JungleSeed)Owner).SplitLevel - 1);
        seed1.AddMoveDirection(Owner.MoveDirection.Angle.Value + 45000);
        seed2.AddMoveDirection(Owner.MoveDirection.Angle.Value - 45000);
        seed1.Speed.SetMoveSpeedToCurrentDirection();
        seed2.Speed.SetMoveSpeedToCurrentDirection();
        seed1.Alignment.OwningEntity = Owner.Alignment.OwningEntity;
        seed2.Alignment.OwningEntity = Owner.Alignment.OwningEntity;
    }
}
