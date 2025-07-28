using Engine.ECS.Components.PositionHandling;

namespace Engine.ECS.Components.ControlHandling.Behaviors;

public class BehaviorChangeBodyType : Behavior
{
    private BodyType BodyType { get; }

    public BehaviorChangeBodyType(BodyType bodyType)
    {
        BodyType = bodyType;
    }

    public override void Action()
    {
        Owner.CollisionBox.BodyType = BodyType;
    }
}
