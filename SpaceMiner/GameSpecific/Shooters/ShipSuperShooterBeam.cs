using Engine.ECS.Components.ShootingHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;
using SpaceMiner.GameSpecific.Entities.Shots;

namespace SpaceMiner.GameSpecific.Shooters;

public class ShipSuperShooterBeam : Shooter
{
    public ShipSuperShooterBeam(Entity owner) : base(owner)
    {
        Owner = owner;
        AddShootAction(ShootStill);
        RelativeSpawnPosition = IntVector2.New(0, 0);

        // Shot Properties
        ShotType = typeof(SuperBeam);
        EntityKind = EntityKind.PlayerShot;
    }
}