using Engine.Managers.GlobalManagement;

namespace Engine.ECS.Components.ControlHandling.Behaviors.DeathAndDestroy; // TODO: Move to game specific folder?

public class BehaviorResetScore : Behavior
{
    public override void Action()
    {
        GlobalManager.Values.MainCharData.Resources.SetAmount(Types.ResourceType.Score, 0);
    }
}
