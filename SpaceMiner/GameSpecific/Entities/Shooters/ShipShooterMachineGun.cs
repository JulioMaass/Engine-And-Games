using Engine.ECS.Components.ShootingHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;
using SpaceMiner.GameSpecific.Entities.Shots;

namespace SpaceMiner.GameSpecific.Entities.Shooters;

public class ShipShooterMachineGun : Shooter
{
    public ShipShooterMachineGun(Entity owner) : base(owner)
    {
        Owner = owner;
        AddShootAction(ShootWithStatsModifiers);
        RelativeSpawnPosition = IntVector2.New(0, 0);

        // Shot Properties
        ShotType = typeof(ResizableShot);
        EntityKind = EntityKind.PlayerShot;
        // Blue
        AutoFireRate = 5;
        ShotSpeed = 5.5f;
        // Green
        BaseDamage = 4;
        ShotSize = 6;
        SizeScaling = 2;
        // Yellow
        SpreadAngle = 10000;
        // Red
        BlastBaseSize = 12;
        BlastSizeScaling = 4;
        BlastBaseDamage = 6;
        BlastDamageScaling = 5;
        BlastDuration = 10;
        // Other
        InaccuracyAngle = (2500, 5000);
    }
}