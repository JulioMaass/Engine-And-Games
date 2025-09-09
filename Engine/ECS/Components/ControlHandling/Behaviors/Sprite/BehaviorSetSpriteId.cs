using System;

namespace Engine.ECS.Components.ControlHandling.Behaviors.Sprite;

public class BehaviorSetSpriteId : Behavior
{
    public Func<int> IdGetter { get; set; }

    public BehaviorSetSpriteId(Func<int> idGetter)
    {
        IdGetter = idGetter;
    }

    public override void Action()
    {
        Owner.StateManager.CurrentState.SpriteId = IdGetter();
    }
}
