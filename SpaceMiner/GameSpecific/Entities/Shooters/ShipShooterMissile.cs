using Engine.ECS.Components.ShootingHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;
using SpaceMiner.GameSpecific.Entities.Shots;

namespace SpaceMiner.GameSpecific.Entities.Shooters;

public class ShipShooterMissile : Shooter
{
    public ShipShooterMissile(Entity owner) : base(owner)
    {
        Owner = owner;
        AddShootAction(NewShotInShootDirection);
        RelativeSpawnPosition = IntVector2.New(0, 0);

        // Shot Properties
        ShotType = typeof(HomingMissile);
        EntityKind = EntityKind.PlayerShot;
        AutoFireRate = 20;
    }
}