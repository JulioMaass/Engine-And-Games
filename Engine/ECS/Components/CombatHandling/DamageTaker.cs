using Engine.ECS.Entities;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;
using System;

namespace Engine.ECS.Components.CombatHandling;

public class DamageTaker : Component // TODO: ARCHITECTURE: Remove conditional compilation, and make it options based (invincibility, flickering, buffer). Make invincibility a component/class?
{
    // Hp
    public Resource CurrentHp { get; }

    // Damage taking
    private int DamageBuffer { get; set; }

    // Invincibility
    private bool InvincibleOnHit { get; set; }
    private int InvincibilityFrames { get; set; }
    private int InvincibilityCounter { get; set; }

    public DamageTaker(Entity owner, int maxHp)
    {
        Owner = owner;
        CurrentHp = Resource.NewFull(ResourceType.Hp, maxHp);
    }

    public void HealHp(int hp)
    {
        CurrentHp.Add(hp);
    }

    private void CheckForDeath()
    {
        if (CurrentHp.Amount > 0) return;
        EntityManager.TriggerDeath(Owner);
    }

    public void BufferDamage(int amount)
    {
        amount /= Owner.StatsManager.GetMultipliedStats(stats => stats.DefenseRatio);

        if (InvincibleOnHit)
            DamageBuffer = DamageBuffer == 0 ?
                amount :
                Math.Min(DamageBuffer, amount);
        else
            DamageBuffer += amount;
    }

    public void ApplyBufferedDamage()
    {
        InvincibilityCounter++;

        // Apply damage
        if (DamageBuffer == 0) return;
        CurrentHp.Amount -= DamageBuffer;
        DamageBuffer = 0;

        // Set hurt state
        Owner.PlayerControl?.GotHurt.TurnOn();

        // Set invincibility
        if (InvincibleOnHit)
            InvincibilityCounter = 0;

        // Death
        CheckForDeath();
    }

    public bool IsInvincible()
    {
        if (Owner.StateManager.CurrentState?.IsInvincible == true)
            return true;
        if (!InvincibleOnHit)
            return false;
        return InvincibilityCounter < InvincibilityFrames;
    }

    public bool IsFlickering()
    {
        if (!IsInvincible())
            return false;

        if (InvincibilityCounter >= InvincibilityFrames)
            return false;

        if (InvincibilityCounter < InvincibilityFrames / 3)
            return InvincibilityCounter % 6 < 3;
        return InvincibilityCounter % 4 < 2;
    }

    public void SetInvincibilityFrames(int invincibilityFrames)
    {
        InvincibilityFrames = invincibilityFrames;
        InvincibleOnHit = true;
        InvincibilityCounter = InvincibilityFrames;
    }
}
