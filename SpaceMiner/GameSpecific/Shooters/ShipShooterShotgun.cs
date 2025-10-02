using Engine.ECS.Components.ShootingHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;
using SpaceMiner.GameSpecific.Entities.Shots;

namespace SpaceMiner.GameSpecific.Shooters;

public class ShipShooterShotgun : Shooter
{
    public ShipShooterShotgun(Entity owner) : base(owner)
    {
        Owner = owner;
        AddShootAction(ShootWithStatsModifiers);
        RelativeSpawnPosition = IntVector2.New(0, 0);

        // Shot Properties
        ShotType = typeof(ResizableShot);
        EntityKind = EntityKind.PlayerShot;
        // Blue
        AutoFireRate = 30;
        ShotSpeed = 5.5f;
        ShotDuration = 30;
        // Green
        BaseDamage = 20;
        ShotSize = IntVector2.Square(8);
        SizeScaling = 4;
        // Yellow
        AmountOfShots = 3;
        SpreadAngle = 15000;
        // Red
        BlastBaseSize = 18;
        BlastSizeScaling = 4;
        BlastBaseDamage = 10;
        BlastDamageScaling = 10;
        BlastDuration = 20;
        // Other
    }
}