using Engine.ECS.Components.CombatHandling;
using Engine.ECS.Components.ControlHandling.Behaviors.EntityCreation;
using Engine.ECS.Entities;
using Engine.ECS.Entities.EntityCreation;
using Engine.ECS.Entities.Shared;
using Engine.Helpers;
using Engine.Managers.GlobalManagement;
using Engine.Types;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Engine.ECS.Components.ShootingHandling;

public class Shooter : Component
{
    public Type ShotType { get; set; }
    public EntityKind EntityKind { get; set; }
    public List<Action<Entity>> ShotModifiers { get; } = new();
    private Action ShootAction { get; set; } // Keep this private so that it can only be called using CheckToShoot
    public IntVector2 RelativeSpawnPosition { get; set; }
    private IntVector2 SpawnPosition => Owner.Position.Pixel + RelativeSpawnPosition.MirrorX(Owner.Facing.IsXMirrored);
    // Ammo
    public ResourceType AmmoType { get; set; } = ResourceType.None;
    public int AmmoCost { get; set; }

    // Properties
    // Blue
    public int AutoFireRate { get; set; } // Frames between shots
    public float ShotSpeed { get; set; }
    // Green
    public int BaseDamage { get; set; }
    public int ShotSize { get; set; }
    public int SizeScaling { get; set; }
    // Yellow
    public int AmountOfShots { get; set; } = 1;
    public int SpreadAngle { get; set; } = 45000;
    // Red
    public int BlastBaseSize { get; set; }
    public int BlastSizeScaling { get; set; }
    public int BlastBaseDamage { get; set; }
    public int BlastDamageScaling { get; set; }
    public int BlastDuration { get; set; }
    // Other
    public (int Step, int MaxAngle) InaccuracyAngle { get; set; } // Step 5 max 15 means angles 0, 5, 10, 15 are allowed
    public int ShotDuration { get; set; }

    public Shooter(Entity owner) => Owner = owner;
    public void AddShootAction(Action shootAction) => ShootAction = shootAction;

    public void CheckToShoot()
    {
        if (AmmoType != ResourceType.None)
        {
            if (!GlobalManager.Values.Resources.HasResource(AmmoType, AmmoCost))
                return;
            GlobalManager.Values.Resources.AddAmount(AmmoType, -AmmoCost);
        }
        if (Owner.FrameHandler.CurrentFrame > 1) // Avoid shooting on spawn frame
            ShootAction.Invoke();
    }

    private Entity NewShot()
    {
        var shot = EntityManager.CreateEntityAt(ShotType, SpawnPosition);
        shot.AssignEntityKind(EntityKind);
        ApplyModifiers(shot);
        return shot;
    }

    private void ApplyModifiers(Entity shot)
    {
        // Damage
        if (BaseDamage > 0)
            shot.DamageDealer.BaseDamage = BaseDamage;
        var extraDamagePercentage = Owner.StatsManager.GetAddedFloatStats(stats => stats.ExtraDamagePercentage);
        shot.DamageDealer.AddExtraDamage((int)Math.Round(shot.DamageDealer.BaseDamage * extraDamagePercentage));

        // Size
        if (ShotSize != 0 && shot.Sprite.Resizable)
        {
            var size = ShotSize + Owner.StatsManager.GetAddedStats(stats => stats.ExtraSize) * SizeScaling;
            shot.Sprite.StretchedSize = new IntVector2(size, size);
            shot.AddCenteredOutlinedCollisionBox();
        }

        // Speed
        var speed = ShotSpeed == 0
            ? shot.Speed.MoveSpeed
            : ShotSpeed;
        speed += Owner.StatsManager.GetAddedFloatStats(stats => stats.ExtraSpeed);
        shot.Speed.MoveSpeed = speed;

        // Blast
        var blastLevel = Owner.StatsManager.GetAddedStats(stats => stats.AddedBlastLevel);
        if (blastLevel > 0)
        {
            var shotSize = ShotSize + Owner.StatsManager.GetAddedStats(stats => stats.ExtraSize) * SizeScaling; // Green size increase is applied to blast too (to avoid shot being bigger than blast)
            var blastSize = BlastBaseSize + BlastSizeScaling * (blastLevel - 1) + shotSize;
            var damage = shot.DamageDealer.Damage + BlastBaseDamage + BlastDamageScaling * (blastLevel - 1);
            var color = new Color(255, 127, 0, 255);
            shot.DamageDealer.AddOnHitBehavior(new BehaviorCreateBlast(typeof(ResizableBlast), EntityKind.PlayerShot, AlignmentType.Friendly, BlastDuration, damage, blastSize, color));
        }

        // Duration
        if (ShotDuration > 0)
            shot.AddFrameCounter(ShotDuration);

        // Other
        foreach (var modifier in ShotModifiers)
            modifier(shot);
    }

    private Entity NewShotWithInheritedDirection()
    {
        var shot = NewShot();
        shot.Facing.CopyFacingAndMirrorDirection(Owner);
        return shot;
    }

    public void NewShotInShootDirection()
    {
        NewShotMovingToDirection(Owner.ShootDirection.Angle.Value);
    }

    public Entity NewShotMovingToDirection(int angle, int spawnOffset = 0)
    {
        var shot = NewShot();
        shot.AddDirectionAndMove(angle);
        if (spawnOffset != 0)
            shot.Position.Pixel += shot.MoveDirection.GetVectorLength() * spawnOffset;
        return shot;
    }

    public void ShootStill() => NewShotWithInheritedDirection();

    public void ShootStraight()
    {
        var shot = NewShotWithInheritedDirection();
        shot.Speed.SetMoveSpeedToCurrentDirection();
    }

    public void ShootAtSpeed(Vector2 speed)
    {
        if (Owner.Facing.IsXMirrored)
            speed = new Vector2(-speed.X, speed.Y);
        var shot = NewShotWithInheritedDirection();
        shot.Speed.SetSpeed(speed);
    }

    public void ShootAtPosition(IntVector2 position)
    {
        var shot = NewShot();
        shot.AddMoveDirection();
        shot.MoveDirection.SetAngleDirectionToPosition(position);
        shot.Facing.CopyFacingFrom(Owner);
        shot.Speed.SetMoveSpeedToCurrentDirection();
    }

    public void ShootAtPlayer()
    {
        if (EntityManager.PlayerEntity != null)
            ShootAtPosition(EntityManager.PlayerEntity.Position.Pixel);
        else
            ShootStraight();
    }

    public void ShootParabolic(Action<Entity> targetingAction)
    {
        var shot = NewShotWithInheritedDirection();
        targetingAction?.Invoke(shot);
    }

    public void ShootSpread(int middleAngle, int angleBetweenShots, int spawnOffset = 0)
    {
        var amountOfShots = AmountOfShots * (Owner.StatsManager.GetAddedStats(stats => stats.ExtraShots) + 1);
        var initialAngle = middleAngle - (amountOfShots - 1) / 2 * angleBetweenShots;
        if (amountOfShots % 2 == 0)
            initialAngle -= angleBetweenShots / 2;
        for (var i = 0; i < amountOfShots; i++)
        {
            var shot = NewShotMovingToDirection(initialAngle + i * angleBetweenShots, spawnOffset);
            shot.Facing.CopyFacingFrom(Owner);
        }
    }

    public void ShootWithStatsModifiers()
    {
        var inaccuracyAngle = GetInaccuracyAngle();
        var shotAngle = Owner.ShootDirection.Angle.Value + inaccuracyAngle.Value;
        ShootSpread(shotAngle, SpreadAngle);
    }

    private Angle GetInaccuracyAngle()
    {
        if (InaccuracyAngle.Step <= 0 || InaccuracyAngle.MaxAngle <= 0)
            return 0;
        var maxSteps = InaccuracyAngle.MaxAngle / InaccuracyAngle.Step;
        var step = GetRandom.UnseededInt(0, maxSteps + 1);
        var angle = step * InaccuracyAngle.Step;
        // randomize side
        if (GetRandom.UnseededBool())
            angle *= -1;
        return new Angle(angle);
    }

    public void ShootInAllDirections(int shotAmount, int initialAngle = 0, int spawnOffset = 0)
    {
        for (var i = 0; i < shotAmount; i += 1)
            NewShotMovingToDirection(initialAngle + i * 360000 / shotAmount, spawnOffset);
    }

    public void ShootLeftAndRight()
    {
        foreach (var (direction, crawl) in new[] { (180000, 1), (0, -1) })
        {
            var shot = NewShot();
            shot.AddMoveDirection(direction);
            shot.Speed.SetMoveSpeedToCurrentDirection();
            shot.Speed.CrawlTurnDirection = crawl;
        }
    }
}