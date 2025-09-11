using Engine.ECS.Components.ShootingHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;
using SpaceMiner.GameSpecific.Entities.Shots;

namespace SpaceMiner.GameSpecific.Shooters;

public class ShipShooterMissileMine : Shooter
{
    public ShipShooterMissileMine(Entity owner) : base(owner)
    {
        Owner = owner;
        AddShootAction(NewShotInShootDirection);
        RelativeSpawnPosition = IntVector2.New(0, 0);

        // Ammo
        AmmoType = ResourceType.MissileMine;
        AmmoCost = 1;

        // Shot Properties
        ShotType = typeof(MineMissile);
        EntityKind = EntityKind.PlayerShot;
    }
}