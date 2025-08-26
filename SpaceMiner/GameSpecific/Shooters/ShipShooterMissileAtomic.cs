using Engine.ECS.Components.ShootingHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;
using SpaceMiner.GameSpecific.Entities.Shots;

namespace SpaceMiner.GameSpecific.Shooters;

public class ShipShooterMissileAtomic : Shooter
{
    public ShipShooterMissileAtomic(Entity owner) : base(owner)
    {
        Owner = owner;
        AddShootAction(NewShotInShootDirection);
        RelativeSpawnPosition = IntVector2.New(0, 0);

        // Ammo
        AmmoType = ResourceType.MissileAtomic;
        AmmoCost = 1;

        // Shot Properties
        ShotType = typeof(AtomicMissile);
        EntityKind = EntityKind.PlayerShot;
    }
}