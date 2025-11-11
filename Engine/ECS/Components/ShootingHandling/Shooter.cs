using Engine.ECS.Components.ItemsHandling;
using Engine.ECS.Entities;
using Engine.ECS.Entities.EntityCreation;
using Engine.Helpers;
using Engine.Managers.Audio;
using Engine.Managers.GlobalManagement;
using Engine.Types;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Engine.ECS.Components.ShootingHandling;

public class Shooter : Component
{
    public Type ShotType { get; set; }
    public string SoundName { get; set; }
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
    public int ShotDuration { get; set; }
    // Green
    public int BaseDamage { get; set; }
    public IntVector2 ShotSize { get; set; }
    public int SizeScaling { get; set; }
    public int PierceAmount { get; set; }
    // Yellow
    // Multi angle
    public int AmountOfShots { get; set; } = 1;
    public int SpreadAngle { get; set; } = 45000;
    // Multi spawn
    public int TotalSpawnPoints => ExtraSpawnPoints + GetShooterAddedStats(stats => stats.ExtraSpawnPointShots) + 1;
    public int ExtraSpawnPoints { get; set; }
    public (int Angle, int Distance) ExtraSpawnAngleAndDistance { get; set; }
    private IntVector2 _extraSpawnPoint;
    // Split
    public int SplitLevel { get; set; }
    // Red
    public int BlastBaseSize { get; set; }
    public int BlastSizeScaling { get; set; }
    public int BlastBaseDamage { get; set; }
    public int BlastDamageScaling { get; set; }
    public int BlastDuration { get; set; }
    // Aim
    public (int Step, int MaxAngle) InaccuracyAngle { get; set; } // Step 5 max 15 means angles 0, 5, 10, 15 are allowed
    public int AngleRounding { get; set; } // Round to multiples of this angle
    public bool RoundSpeed { get; set; }

    public Shooter(Entity owner) => Owner = owner;
    public void AddShootAction(Action shootAction) => ShootAction = shootAction;

    public void CheckToShoot()
    {
        if (AmmoType != ResourceType.None)
        {
            if (!GlobalManager.Values.MainCharData.Resources.HasResource(AmmoType, AmmoCost))
                return;
            GlobalManager.Values.MainCharData.Resources.AddAmount(AmmoType, -AmmoCost);
        }
        if (Owner.FrameHandler.CurrentFrame > 1) // Avoid shooting on spawn frame
            ShootAction.Invoke();
    }

    private Entity NewShot()
    {
        var shot = EntityManager.CreateEntityAt(ShotType, SpawnPosition);
        shot.AddShotProperties();
        shot.ShotProperties.SetShotBasicData(EntityKind, Owner);
        ApplyModifiers(shot);

        // Multi Positioning
        shot.Position.Pixel += _extraSpawnPoint;

        if (SoundName != null)
            AudioManager.PlaySound("sndShot");
        return shot;
    }

    private void ApplyModifiers(Entity shot)
    {
        // Damage
        var baseDamage = BaseDamage == 0 ? shot.DamageDealer.BaseDamage : BaseDamage;
        var extraDamagePercentage = GetShooterAddedFloatStats(stats => stats.ExtraDamagePercentage);
        var extraDamage = (int)Math.Round(baseDamage * extraDamagePercentage);

        // Size
        var size = shot.Sprite.Size;
        if (ShotSize != IntVector2.Zero && shot.Sprite.Resizable)
            size = ShotSize + GetShooterAddedStats(stats => stats.ExtraSize) * SizeScaling;

        // Pierce
        var pierce = PierceAmount + GetShooterAddedStats(stats => stats.ExtraPierceAmount);

        // Speed
        var speed = ShotSpeed == 0
            ? shot.Speed.MoveSpeed
            : ShotSpeed;
        speed += GetShooterAddedFloatStats(stats => stats.ExtraSpeed);

        // Duration
        var duration = ShotDuration + GetShooterAddedStats(stats => stats.ExtraDuration);

        // Blast
        var blastData = (Duration: BlastDuration, Damage: 0, Size: 0);
        var blastLevel = GetShooterAddedStats(stats => stats.AddedBlastLevel);
        if (blastLevel > 0)
        {
            var shotSize = ShotSize + GetShooterAddedStats(stats => stats.ExtraSize) * SizeScaling; // Green size increase is applied to blast too (to avoid shot being bigger than blast)
            var shotBiggerAxisSize = Math.Max(shotSize.X, shotSize.Y);
            blastData.Size = BlastBaseSize + BlastSizeScaling * (blastLevel - 1) + shotBiggerAxisSize;
            blastData.Damage = shot.DamageDealer.Damage + BlastBaseDamage + BlastDamageScaling * (blastLevel - 1);
        }

        // Split
        var splitLevel = SplitLevel + GetShooterAddedStats(stats => stats.ExtraSplitLevel);

        // Apply modifiers
        shot.ShotProperties.ApplyShotModifiers(baseDamage, extraDamage, size, pierce, speed, blastData, splitLevel, duration, ShotModifiers);
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

    public void ShootHorizontalAimable(int aimingAngle)
    {
        var shot = NewShotWithInheritedDirection();
        if (Owner.PlayerControl == null)
            return;
        if (Owner.PlayerControl.Up)
            shot.MoveDirection.TurnTowardsAngle(270000, aimingAngle);
        if (Owner.PlayerControl.Down)
            shot.MoveDirection.TurnTowardsAngle(90000, aimingAngle);
        shot.Speed.SetMoveSpeedToCurrentDirection();
    }

    public void ShootEightDirectionAim()
    {
        var shot = NewShotWithInheritedDirection();
        if (Owner.PlayerControl == null)
            return;
        if (Owner.PlayerControl.Up)
            shot.MoveDirection.TurnTowardsAngle(270000, 90000);
        if (Owner.PlayerControl.Down)
            shot.MoveDirection.TurnTowardsAngle(90000, 90000);
        if (Owner.PlayerControl.Left)
            shot.MoveDirection.TurnTowardsAngle(180000, 45000);
        if (Owner.PlayerControl.Right)
            shot.MoveDirection.TurnTowardsAngle(0, 45000);
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
        if (AngleRounding != 0)
            shot.MoveDirection.Angle = (shot.MoveDirection.Angle.Value + AngleRounding / 2) / AngleRounding * AngleRounding;
        shot.Facing.CopyFacingFrom(Owner);
        shot.Speed.SetMoveSpeedToCurrentDirection();
        if (RoundSpeed)
        {
            shot.Speed.SetXSpeed((int)Math.Round(shot.Speed.X));
            shot.Speed.SetYSpeed((int)Math.Round(shot.Speed.Y));
        }
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
        var shotMultiplier = GetShooterAddedStats(stats => stats.ExtraAngleShots) + 1;
        var amountOfShots = AmountOfShots * shotMultiplier;
        var initialAngle = GetInitialAngle(middleAngle, angleBetweenShots, amountOfShots);
        for (var i = 0; i < amountOfShots; i++)
        {
            var angle = new Angle(initialAngle + i * angleBetweenShots);
            if (Owner.Facing.IsXMirrored) angle.MirrorX();
            var shot = NewShotMovingToDirection(angle.Value, spawnOffset);
            shot.Facing.CopyFacingFrom(Owner);
        }
    }

    public void ShootAimedSpread(int angleBetweenShots, int spawnOffset = 0) =>
        ShootSpread(Owner.ShootDirection.Angle.Value, angleBetweenShots, spawnOffset);

    private int GetShooterAddedStats(Func<Stats, int?> statSelector)
    {
        var isPrimaryWeapon = Owner.Shooter == this;
        var isSecondaryWeapon = Owner.SecondaryShooter == this;
        return StatsManager.GetAddedStats(Owner, statSelector, false, isPrimaryWeapon, isSecondaryWeapon);
    }

    private float GetShooterAddedFloatStats(Func<Stats, float?> statSelector)
    {
        var isPrimaryWeapon = Owner.Shooter == this;
        var isSecondaryWeapon = Owner.SecondaryShooter == this;
        return StatsManager.GetAddedFloatStats(Owner, statSelector, false, isPrimaryWeapon, isSecondaryWeapon);
    }

    public int GetInitialAngle(int middleAngle, int angleBetweenShots, int amountOfShots)
    {
        var initialAngle = middleAngle - (amountOfShots - 1) / 2 * angleBetweenShots;
        if (amountOfShots % 2 == 0)
            initialAngle -= angleBetweenShots / 2;
        return initialAngle;
    }

    public void ShootWithStatsModifiers()
    {
        var inaccuracyAngle = GetInaccuracyAngle();
        var shotAngle = Owner.ShootDirection.Angle.Value + inaccuracyAngle.Value;
        if (TotalSpawnPoints <= 1)
            ShootSpread(shotAngle, SpreadAngle);
        else
            ShootFromExtraSpawnPoints(shotAngle);
    }

    private void ShootFromExtraSpawnPoints(int shotAngle)
    {
        if (ExtraSpawnAngleAndDistance.Angle == 0)
        {
            Debugger.Break(); // Not implemented for this weapon, need to check it
            ExtraSpawnAngleAndDistance = (45000, 10);
        }

        var initialAngle = GetInitialAngle(shotAngle, ExtraSpawnAngleAndDistance.Angle, TotalSpawnPoints);
        for (var i = 0; i < TotalSpawnPoints; i++)
        {
            _extraSpawnPoint = Angle.GetVectorLength(initialAngle + i * ExtraSpawnAngleAndDistance.Angle) * ExtraSpawnAngleAndDistance.Distance;
            _extraSpawnPoint.Y += ExtraSpawnAngleAndDistance.Distance;
            ShootSpread(shotAngle, SpreadAngle);
        }
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