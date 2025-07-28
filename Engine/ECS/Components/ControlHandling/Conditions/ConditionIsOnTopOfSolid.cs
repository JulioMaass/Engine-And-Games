namespace Engine.ECS.Components.ControlHandling.Conditions;

public class ConditionIsOnTopOfSolid : Condition
{
    protected override bool IsTrue()
    {
        return Owner.Physics.SolidCollisionChecking.IsOnTopOfSolid();
    }
}
