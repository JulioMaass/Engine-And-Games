using Engine.ECS.Components.ShootingHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;

namespace Candle.GameSpecific.Entities;

public class CandleLanceShooter : Shooter
{
    public CandleLanceShooter(Entity owner) : base(owner)
    {
        Owner = owner;
        AddShootAction(ShootStill);
        RelativeSpawnPosition = IntVector2.New(37, 0);
        ShotType = typeof(CandleLance);
    }
}