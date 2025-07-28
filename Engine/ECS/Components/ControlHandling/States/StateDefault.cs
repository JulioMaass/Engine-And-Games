namespace Engine.ECS.Components.ControlHandling.States;

public class StateDefault : State
{
    public StateDefault(string name)
    {
        Name = name;
    }

    public override bool StartCondition()
    {
        return true;
    }

    public override bool KeepCondition()
    {
        return true;
    }

    public override bool PostProcessingKeepCondition()
    {
        return true;
    }

    public override void Behavior()
    {
    }
}
