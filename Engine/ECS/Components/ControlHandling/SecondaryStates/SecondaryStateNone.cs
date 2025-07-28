namespace Engine.ECS.Components.ControlHandling.SecondaryStates;

public class SecondaryStateNone : SecondaryState
{
    public override bool StartCondition()
    {
        return true;
    }
    public override bool KeepCondition()
    {
        return true;
    }

    public override void Behavior() { }
}