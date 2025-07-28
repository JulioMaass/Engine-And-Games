namespace Engine.ECS.Components.CombatHandling;

public class KnockbackReceiver
{
    public bool Trigger { get; private set; }

    public void TriggerKnockback()
    {
        Trigger = true;
    }

    public void ResetKnockbackTrigger()
    {
        Trigger = false;
    }
}
