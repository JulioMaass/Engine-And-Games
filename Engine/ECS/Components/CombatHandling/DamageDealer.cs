using Engine.ECS.Components.ControlHandling.Behaviors;
using Engine.ECS.Entities;
using Engine.ECS.Entities.EntityCreation;
using System.Collections.Generic;

namespace Engine.ECS.Components.CombatHandling;

public class DamageDealer : Component
{
    // Damage dealing
    public bool DealsDamage => Damage > 0;
    public int Damage => BaseDamage + ExtraDamage;
    public int BaseDamage { get; set; }
    public int ExtraDamage { get; private set; }
    // Hit behavior
    private PiercingType PiercingType { get; set; }
    public HitType HitType { get; set; }
    private List<Entity> HitList { get; } = new();
    private List<Behavior> OnHitBehaviors { get; } = new();
    private List<Behavior> OnHitTargetBehaviors { get; } = new(); // runs behavior on the target that was hit

    public DamageDealer(Entity owner, int damage, PiercingType piercingType, HitType hitType)
    {
        Owner = owner;
        BaseDamage = damage;
        PiercingType = piercingType;
        HitType = hitType;
    }

    public void RunEffects(Entity damagedEntity)
    {
        AddToHitList(damagedEntity);
        DeleteIfCantPierce(damagedEntity);
        RunOnHitBehaviors();
        RunOnHitTargetBehaviors(damagedEntity);
    }

    public void AddExtraDamage(int extraDamage)
    {
        ExtraDamage += extraDamage;
    }

    public void AddToHitList(Entity entity)
    {
        if (HitType == HitType.HitOnce)
            HitList.Add(entity);
    }

    public void CopyHitListFrom(Entity entity)
    {
        if (HitType == HitType.HitOnce)
            HitList.AddRange(entity.DamageDealer.HitList);
    }

    public bool IsInHitList(Entity entity)
    {
        if (HitType != HitType.HitOnce)
            return false;
        return HitList.Contains(entity);
    }

    public void DeleteIfCantPierce(Entity entity)
    {
        var entityHp = entity.DamageTaker.CurrentHp.Amount;

        if (PiercingType == PiercingType.PierceNone)
            EntityManager.TriggerDeath(Owner);
        if (PiercingType == PiercingType.PierceOnOverkill && entityHp >= Damage)
            EntityManager.TriggerDeath(Owner);
        if (PiercingType == PiercingType.PierceOnKill && entityHp > Damage)
            EntityManager.TriggerDeath(Owner);
    }

    public void SetPiecingType(PiercingType piercingType)
    {
        PiercingType = piercingType;
    }

    public void AddOnHitBehavior(Behavior behavior)
    {
        OnHitBehaviors.Add(behavior);
        behavior.Owner = Owner;
    }

    public void RunOnHitBehaviors()
    {
        foreach (var behavior in OnHitBehaviors)
            behavior.Action();
    }

    public void AddOnHitTargetBehavior(Behavior behavior)
    {
        OnHitTargetBehaviors.Add(behavior);
        behavior.Owner = Owner;
    }

    public void RunOnHitTargetBehaviors(Entity target)
    {
        foreach (var behavior in OnHitTargetBehaviors)
        {
            behavior.Owner = target;
            behavior.Action();
            behavior.Owner = Owner;
        }
    }
}

public enum PiercingType
{
    PierceAll,
    PierceNone,
    PierceOnOverkill,
    PierceOnKill
}

public enum HitType
{
    HitDefault,
    HitOnce, // Hit each entity only once
    HitLoop // Hit entities every few frames
}