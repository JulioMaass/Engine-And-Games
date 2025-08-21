using Engine.Managers;

namespace Engine.ECS.Components.ControlHandling.Behaviors.DeathAndDestroy;

public class BehaviorPlayerStartDeathCounter : Behavior
{
    public override void Action()
    {
        PlayerSpawnManager.StartDeathCounter();
    }
}
