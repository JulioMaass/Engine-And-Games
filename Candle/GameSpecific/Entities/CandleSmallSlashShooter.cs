using Engine.ECS.Components.ShootingHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;

namespace Candle.GameSpecific.Entities;

public class CandleSmallSlashShooter : Shooter
{
    public CandleSmallSlashShooter(Entity owner) : base(owner)
    {
        Owner = owner;
        AddShootAction(ShootStill);
        RelativeSpawnPosition = IntVector2.New(18, -5);
        ShotType = typeof(CandleSmallSlash);
    }
}