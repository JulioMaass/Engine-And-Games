using Engine.ECS.Components.ShootingHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;
using SpaceMiner.GameSpecific.Entities.Shots;

namespace SpaceMiner.GameSpecific.Entities.Shooters;

public class ShipShooterMissileHoming : Shooter
{
    public ShipShooterMissileHoming(Entity owner) : base(owner)
    {
        Owner = owner;
        AddShootAction(() => ShootSpread(Owner.ShootDirection.Angle.Value, 10000));
        RelativeSpawnPosition = IntVector2.New(0, 0);
        AmountOfShots = 4;

        // Ammo
        AmmoType = ResourceType.MissileHoming;
        AmmoCost = 1;

        // Shot Properties
        ShotType = typeof(HomingMissile);
        EntityKind = EntityKind.PlayerShot;
    }
}