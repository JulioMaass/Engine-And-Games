using Engine.ECS.Components.CombatHandling;
using Engine.ECS.Components.ControlHandling.Behaviors.EntityCreation;
using Engine.ECS.Components.ControlHandling.Behaviors.Shot;
using Engine.ECS.Entities.EntityCreation;
using Engine.ECS.Entities.Shared;
using Engine.Types;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Engine.ECS.Components.ShootingHandling;

public class ShotProperties : Component
{
    public int ShotScreenPrice { get; set; }
    public (int Duration, int Damage, int Size) BlastData { get; set; }
    public int SplitLevel { get; set; }
    public List<Action<Entity>> ShotModifiers { get; } = new();

    public ShotProperties(Entity owner)
    {
        Owner = owner;
    }

    public void SetShotBasicData(EntityKind entityKind, Entity owningEntity)
    {
        Owner.AssignEntityKind(entityKind);
        Owner.Alignment.OwningEntity = owningEntity;
    }

    public void ApplyShotModifiers(int baseDamage, int extraDamage, IntVector2 size, int pierce, float speed, (int Duration, int Damage, int Size) blastData, int splitLevel, int duration, List<Action<Entity>> shotModifiers)
    {
        // Damage
        Owner.DamageDealer.BaseDamage = baseDamage;
        Owner.DamageDealer.AddExtraDamage(extraDamage);

        // Size
        if (Owner.Sprite.Size != size)
        {
            Owner.Sprite.StretchedSize = size;
            Owner.AddCenteredOutlinedCollisionBox();
        }

        // Pierce
        Owner.DamageDealer.PierceAmount = pierce;

        // Speed
        Owner.Speed.MoveSpeed = speed;

        // Blast
        var color = new Color(255, 127, 0, 255);
        BlastData = blastData;
        if (blastData.Damage > 0 && blastData.Size > 0)
            Owner.DamageDealer.AddOnHitBehavior(new BehaviorCreateBlast(typeof(ResizableBlast), EntityKind.PlayerShot, AlignmentType.Friendly, blastData.Duration, blastData.Damage, blastData.Size, color));

        // Split
        SplitLevel = splitLevel;
        if (splitLevel > 0)
            Owner.DamageDealer.AddOnHitBehavior(new BehaviorSplit(splitLevel));

        // Duration
        if (duration > 0)
            Owner.AddFrameCounter(duration);

        // Other modifiers
        ShotModifiers.AddRange(shotModifiers);
        foreach (var modifier in shotModifiers)
            modifier(Owner);
    }

    public void CopyShotModifiers(Entity originalShot, int splitReduction = 0)
    {
        SetShotBasicData(originalShot.EntityKind, originalShot.Alignment.OwningEntity);

        // Damage
        var baseDamage = originalShot.DamageDealer.BaseDamage;
        var extraDamage = originalShot.DamageDealer.ExtraDamage;
        // Size
        var size = originalShot.Sprite.StretchedSize;
        // Pierce
        var pierce = originalShot.DamageDealer.PierceAmount;
        // Speed
        var speed = originalShot.Speed.MoveSpeed;
        // Blast
        var blastData = originalShot.ShotProperties.BlastData;
        // Split
        var splitLevel = originalShot.ShotProperties.SplitLevel - splitReduction;
        // Duration
        var duration = originalShot.FrameHandler.EntityDuration;
        // Other modifiers
        var shotModifiers = new List<Action<Entity>>();
        shotModifiers.AddRange(originalShot.ShotProperties.ShotModifiers);
        ApplyShotModifiers(baseDamage, extraDamage, size, pierce, speed, blastData, splitLevel, duration, shotModifiers);
    }
}