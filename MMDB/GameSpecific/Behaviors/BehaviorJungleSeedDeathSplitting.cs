using Engine.ECS.Components.ControlHandling.Behaviors;
using Engine.ECS.Entities;
using MMDB.GameSpecific.Entities.Weapons;

namespace MMDB.GameSpecific.Behaviors;

public class BehaviorJungleSeedDeathSplitting : Behavior
{
    public override void Action()
    {
        var ownerLevel = ((JungleSeed)Owner).SplitLevel;
        if (ownerLevel == 1)
        {
            var seed1 = (JungleSeed)EntityManager.CreateEntityAt(typeof(JungleSeed), Owner.Position.Pixel);
            var seed2 = (JungleSeed)EntityManager.CreateEntityAt(typeof(JungleSeed), Owner.Position.Pixel);
            seed1.SetLevel(0);
            seed2.SetLevel(0);
            seed1.AddMoveDirection(Owner.MoveDirection.Angle.Value + 45000);
            seed2.AddMoveDirection(Owner.MoveDirection.Angle.Value - 45000);
            seed1.Speed.SetMoveSpeedToCurrentDirection();
            seed2.Speed.SetMoveSpeedToCurrentDirection();
            seed1.Alignment.OwningEntity = Owner.Alignment.OwningEntity;
            seed2.Alignment.OwningEntity = Owner.Alignment.OwningEntity;
        }
        else if (ownerLevel == 2)
        {
            var seed1 = (JungleSeed)EntityManager.CreateEntityAt(typeof(JungleSeed), Owner.Position.Pixel);
            var seed2 = (JungleSeed)EntityManager.CreateEntityAt(typeof(JungleSeed), Owner.Position.Pixel);
            var seed3 = (JungleSeed)EntityManager.CreateEntityAt(typeof(JungleSeed), Owner.Position.Pixel);
            var seed4 = (JungleSeed)EntityManager.CreateEntityAt(typeof(JungleSeed), Owner.Position.Pixel);
            seed1.SetLevel(0);
            seed2.SetLevel(0);
            seed3.SetLevel(0);
            seed4.SetLevel(0);
            seed1.AddMoveDirection(Owner.MoveDirection.Angle.Value + 0 + 45000);
            seed2.AddMoveDirection(Owner.MoveDirection.Angle.Value + 90000 + 45000);
            seed3.AddMoveDirection(Owner.MoveDirection.Angle.Value + 180000 + 45000);
            seed4.AddMoveDirection(Owner.MoveDirection.Angle.Value + 270000 + 45000);
            seed1.Speed.SetMoveSpeedToCurrentDirection();
            seed2.Speed.SetMoveSpeedToCurrentDirection();
            seed3.Speed.SetMoveSpeedToCurrentDirection();
            seed4.Speed.SetMoveSpeedToCurrentDirection();
            seed1.Alignment.OwningEntity = Owner.Alignment.OwningEntity;
            seed2.Alignment.OwningEntity = Owner.Alignment.OwningEntity;
            seed3.Alignment.OwningEntity = Owner.Alignment.OwningEntity;
            seed4.Alignment.OwningEntity = Owner.Alignment.OwningEntity;
        }
        else if (ownerLevel == 3)
        {
            var seed1 = (JungleSeed)EntityManager.CreateEntityAt(typeof(JungleSeed), Owner.Position.Pixel);
            var seed2 = (JungleSeed)EntityManager.CreateEntityAt(typeof(JungleSeed), Owner.Position.Pixel);
            var seed3 = (JungleSeed)EntityManager.CreateEntityAt(typeof(JungleSeed), Owner.Position.Pixel);
            var seed4 = (JungleSeed)EntityManager.CreateEntityAt(typeof(JungleSeed), Owner.Position.Pixel);
            var seed5 = (JungleSeed)EntityManager.CreateEntityAt(typeof(JungleSeed), Owner.Position.Pixel);
            var seed6 = (JungleSeed)EntityManager.CreateEntityAt(typeof(JungleSeed), Owner.Position.Pixel);
            var seed7 = (JungleSeed)EntityManager.CreateEntityAt(typeof(JungleSeed), Owner.Position.Pixel);
            var seed8 = (JungleSeed)EntityManager.CreateEntityAt(typeof(JungleSeed), Owner.Position.Pixel);
            seed1.SetLevel(0);
            seed2.SetLevel(0);
            seed3.SetLevel(0);
            seed4.SetLevel(0);
            seed5.SetLevel(0);
            seed6.SetLevel(0);
            seed7.SetLevel(0);
            seed8.SetLevel(0);
            seed1.AddMoveDirection(0);
            seed2.AddMoveDirection(90000);
            seed3.AddMoveDirection(180000);
            seed4.AddMoveDirection(270000);
            seed5.AddMoveDirection(0 + 45000);
            seed6.AddMoveDirection(90000 + 45000);
            seed7.AddMoveDirection(180000 + 45000);
            seed8.AddMoveDirection(270000 + 45000);
            seed1.Speed.SetMoveSpeedToCurrentDirection();
            seed2.Speed.SetMoveSpeedToCurrentDirection();
            seed3.Speed.SetMoveSpeedToCurrentDirection();
            seed4.Speed.SetMoveSpeedToCurrentDirection();
            seed5.Speed.SetMoveSpeedToCurrentDirection();
            seed6.Speed.SetMoveSpeedToCurrentDirection();
            seed7.Speed.SetMoveSpeedToCurrentDirection();
            seed8.Speed.SetMoveSpeedToCurrentDirection();
            seed1.Alignment.OwningEntity = Owner.Alignment.OwningEntity;
            seed2.Alignment.OwningEntity = Owner.Alignment.OwningEntity;
            seed3.Alignment.OwningEntity = Owner.Alignment.OwningEntity;
            seed4.Alignment.OwningEntity = Owner.Alignment.OwningEntity;
            seed5.Alignment.OwningEntity = Owner.Alignment.OwningEntity;
            seed6.Alignment.OwningEntity = Owner.Alignment.OwningEntity;
            seed7.Alignment.OwningEntity = Owner.Alignment.OwningEntity;
            seed8.Alignment.OwningEntity = Owner.Alignment.OwningEntity;
        }
    }
}
