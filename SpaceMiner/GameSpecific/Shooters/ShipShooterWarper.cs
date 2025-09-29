using Engine.ECS.Components.ControlHandling.Behaviors.ComplexMovement.Teleporting;
using Engine.ECS.Components.ShootingHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;
using SpaceMiner.GameSpecific.Entities.Shots;
using System.Linq;

namespace SpaceMiner.GameSpecific.Shooters;

public class ShipShooterWarper : Shooter
{
    public ShipShooterWarper(Entity owner) : base(owner)
    {
        Owner = owner;
        AddShootAction(ShootWithStatsModifiers);
        RelativeSpawnPosition = IntVector2.New(0, 0);

        // Shot Properties
        ShotType = typeof(ResizableShot);
        EntityKind = EntityKind.PlayerShot;
        // Blue
        AutoFireRate = 15;
        ShotSpeed = 5.5f;
        // Green
        BaseDamage = 15;
        ShotSize = IntVector2.Square(10);
        SizeScaling = 3;
        // Yellow
        SpreadAngle = 45000 / 2;
        // Red
        BlastBaseSize = 16;
        BlastSizeScaling = 6;
        BlastBaseDamage = 10;
        BlastDamageScaling = 10;
        BlastDuration = 10;
        // Other
        ShotModifiers.Add(e => e.Sprite.SetColor(0, 127, 255, 255));
        ShotModifiers.Add(e => e.StateManager.AutomaticStatesList.FirstOrDefault()?.AddBehavior(new BehaviorWarp(e, 2)));
    }
}