using Engine.ECS.Components.ControlHandling.Behaviors;
using Engine.ECS.Entities;
using Engine.ECS.Entities.EntityCreation;
using System.Collections.Generic;

namespace Engine.ECS.Components.CombatHandling;

public class DamageDealer : Component
{
    // Damage dealing
    public int Damage => BaseDamage + ExtraDamage;
    public int BaseDamage { get; set; }
    public int ExtraDamage { get; private set; }
    // Hit behavior
    private PiercingType PiercingType { get; set; }
    public int PierceAmount { get; set; }
    private int PiercedAmount { get; set; }
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

    public bool CanDealDamage()
    {
        return Damage > 0
               && Owner.CollisionBox?.BodyTypeDealsDamage() == true;
    }

    public void RunEffects(Entity damagedEntity)
    {
        CheckToAddToHitList(damagedEntity);
        DeleteIfCantPierce(damagedEntity);
        RunOnHitBehaviors();
        RunOnHitTargetBehaviors(damagedEntity);
    }

    public void AddExtraDamage(int extraDamage)
    {
        ExtraDamage += extraDamage;
    }

    public void CheckToAddToHitList(Entity entity)
    {
        if (HitType != HitType.HitOnce)
            return;
        AddToHitList(entity);
    }

    public void AddToHitList(Entity entity)
    {
        HitList.Add(entity);
        entity.DamageTaker.HitterList.Add(Owner);
    }

    public void CopyHitListFrom(Entity entity)
    {
        foreach (var hitEntity in entity.DamageDealer.HitList)
            AddToHitList(hitEntity);
    }

    public bool IsInHitList(Entity entity)
    {
        if (HitType != HitType.HitOnce)
            return false;
        return HitList.Contains(entity);
    }

    private void DeleteIfCantPierce(Entity entity)
    {
        var entityHp = entity.DamageTaker.CurrentHp.Amount;

        if (PiercingType == PiercingType.PierceNone)
            EntityManager.TriggerDeath(Owner);
        if (PiercingType == PiercingType.PierceOnOverkill && entityHp >= Damage)
            EntityManager.TriggerDeath(Owner);
        if (PiercingType == PiercingType.PierceOnKill && entityHp > Damage)
            EntityManager.TriggerDeath(Owner);
        if (PiercingType == PiercingType.PierceAmount)
        {
            if (PiercedAmount >= PierceAmount)
                EntityManager.TriggerDeath(Owner);
            PiercedAmount++;
        }
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
    PierceOnKill,
    PierceAmount
}

public enum HitType
{
    HitDefault,
    HitOnce, // Hit each entity only once
    HitLoop // Hit entities every few frames
}