namespace Engine.ECS.Components.ControlHandling.Conditions;

public class ConditionIsDamaged : Condition
{
    protected override bool IsTrue()
    {
        return Owner.DamageTaker.CurrentHp.Amount < Owner.DamageTaker.CurrentHp.MaxAmount;
    }
}
