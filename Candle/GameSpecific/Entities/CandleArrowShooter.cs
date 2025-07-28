using Engine.ECS.Components.ShootingHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;

namespace Candle.GameSpecific.Entities;

public class CandleArrowShooter : Shooter
{
    public CandleArrowShooter(Entity owner) : base(owner)
    {
        Owner = owner;
        AddShootAction(ShootStraight);
        RelativeSpawnPosition = IntVector2.New(18, 0);
        ShotType = typeof(CandleArrow);
        ShotModifiers.Add(e => e.Speed.MoveSpeed = 4f);
    }
}