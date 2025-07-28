namespace Engine.ECS.Components.ControlHandling.Conditions;

public class ConditionKnockedBack : Condition
{
    protected override bool IsTrue()
    {
        var trigger = Owner.KnockbackReceiver?.Trigger == true;
        Owner.KnockbackReceiver?.ResetKnockbackTrigger();
        return trigger;
    }
}
