using Engine.ECS.Components.ShootingHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;

namespace Candle.GameSpecific.Entities;

public class CandleSlashShooter : Shooter
{
    public CandleSlashShooter(Entity owner) : base(owner)
    {
        Owner = owner;
        AddShootAction(ShootStill);
        RelativeSpawnPosition = IntVector2.New(20, 0);
        ShotType = typeof(CandleSlash);
    }
}