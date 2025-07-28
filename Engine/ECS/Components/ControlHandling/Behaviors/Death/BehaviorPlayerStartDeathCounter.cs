using Engine.Managers;

namespace Engine.ECS.Components.ControlHandling.Behaviors.Death;

public class BehaviorPlayerStartDeathCounter : Behavior
{
    public override void Action()
    {
        PlayerSpawnManager.StartDeathCounter();
    }
}
