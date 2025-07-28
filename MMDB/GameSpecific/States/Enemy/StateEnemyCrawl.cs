using Engine.ECS.Components.ControlHandling.States;

namespace MMDB.GameSpecific.States.Enemy;

public class StateEnemyCrawl : State
{
    public override MovementType MovementType { get; set; } = MovementType.Crawling;

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
