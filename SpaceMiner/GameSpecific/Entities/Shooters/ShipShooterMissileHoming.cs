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
        AddShootAction(NewShotInShootDirection);
        RelativeSpawnPosition = IntVector2.New(0, 0);

        // Ammo
        AmmoType = ResourceType.MissileHoming;
        AmmoCost = 1;

        // Shot Properties
        ShotType = typeof(HomingMissile);
        EntityKind = EntityKind.PlayerShot;
    }
}