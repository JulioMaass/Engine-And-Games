using Engine.ECS.Components.ShootingHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;
using SpaceMiner.GameSpecific.Entities.Shots;

namespace SpaceMiner.GameSpecific.Shooters;

public class ShipShooterBlaster : Shooter
{
    public ShipShooterBlaster(Entity owner) : base(owner)
    {
        Owner = owner;
        AddShootAction(ShootWithStatsModifiers);
        RelativeSpawnPosition = IntVector2.New(0, 0);

        // Shot Properties
        ShotType = typeof(ResizableShot);
        EntityKind = EntityKind.PlayerShot;
        // Blue
        AutoFireRate = 45;
        ShotSpeed = 6.0f;
        // Green
        BaseDamage = 30;
        ShotSize = IntVector2.Square(8);
        SizeScaling = 3;
        // Yellow
        SpreadAngle = 30000;
        // Red
        BlastBaseSize = 48;
        BlastSizeScaling = 8;
        BlastBaseDamage = 70;
        BlastDamageScaling = 10;
        BlastDuration = 30;
        // Other
    }
}