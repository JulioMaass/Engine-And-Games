using Engine.Managers.GlobalManagement;

namespace Engine.ECS.Components.ControlHandling.Behaviors.Death; // TODO: Move to game specific folder?

public class BehaviorAddScore : Behavior
{
    private int Amount { get; }

    public BehaviorAddScore(int amount)
    {
        Amount = amount;
    }

    public override void Action()
    {
        GlobalManager.Values.Resources.AddAmount(Types.ResourceType.Score, Amount);
    }
}
