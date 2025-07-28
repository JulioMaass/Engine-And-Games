namespace Engine.ECS.Components.ControlHandling.States;

public class StateFall : State
{
    public override bool StartCondition()
    {
        return !Owner.Physics.SolidCollisionChecking.IsOnTopOfSolid();
    }

    public override bool KeepCondition()
    {
        return !Owner.Physics.SolidCollisionChecking.IsOnTopOfSolid();
    }

    public override bool PostProcessingKeepCondition()
    {
        return !Owner.Physics.SolidCollisionChecking.IsOnTopOfSolid();
    }

    public override void StateSettingBehavior()
    {
    }

    public override void Behavior()
    {
    }
}