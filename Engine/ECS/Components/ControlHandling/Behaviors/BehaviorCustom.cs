using System;

namespace Engine.ECS.Components.ControlHandling.Behaviors;

public class BehaviorCustom : Behavior
{
    private Action AddedAction { get; }

    public BehaviorCustom(Action action)
    {
        AddedAction = action;
    }

    public override void Action()
    {
        AddedAction();
    }
}
