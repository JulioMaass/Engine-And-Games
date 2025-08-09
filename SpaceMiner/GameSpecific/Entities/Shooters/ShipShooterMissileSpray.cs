using Engine.ECS.Components.ShootingHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;
using SpaceMiner.GameSpecific.Entities.Shots;

namespace SpaceMiner.GameSpecific.Entities.Shooters;

public class ShipShooterMissileSpray : Shooter
{
    public ShipShooterMissileSpray(Entity owner) : base(owner)
    {
        Owner = owner;
        AddShootAction(() => ShootSpread(Owner.ShootDirection.Angle.Value, 15000));
        RelativeSpawnPosition = IntVector2.New(0, 0);
        AmountOfShots = 7;

        // Ammo
        AmmoType = ResourceType.MissileSpray;
        AmmoCost = 1;

        // Shot Properties
        ShotType = typeof(SprayMissile);
        EntityKind = EntityKind.PlayerShot;
    }
}