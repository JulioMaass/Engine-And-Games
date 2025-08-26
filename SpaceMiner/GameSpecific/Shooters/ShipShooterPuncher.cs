using Engine.ECS.Components.ShootingHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;
using SpaceMiner.GameSpecific.Entities.Shots;

namespace SpaceMiner.GameSpecific.Shooters;

public class ShipShooterPuncher : Shooter
{
    public ShipShooterPuncher(Entity owner) : base(owner)
    {
        Owner = owner;
        AddShootAction(ShootWithStatsModifiers);
        RelativeSpawnPosition = IntVector2.New(0, 0);

        // Shot Properties
        ShotType = typeof(ResizableShot);
        EntityKind = EntityKind.PlayerShot;
        // Blue
        AutoFireRate = 20;
        ShotSpeed = 5.5f;
        // Green
        BaseDamage = 10;
        ShotSize = 8;
        SizeScaling = 3;
        // Yellow
        SpreadAngle = 45000 / 2;
        ExtraSpawnPoints = 2;
        ExtraSpawnAngleAndDistance = new IntVector2(45000, 16);
        // Red
        BlastBaseSize = 16;
        BlastSizeScaling = 6;
        BlastBaseDamage = 10;
        BlastDamageScaling = 10;
        BlastDuration = 10;
        // Other
    }
}