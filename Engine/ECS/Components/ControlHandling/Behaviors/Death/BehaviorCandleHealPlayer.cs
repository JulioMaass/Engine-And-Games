using Engine.ECS.Entities;

namespace Engine.ECS.Components.ControlHandling.Behaviors.Death; // TODO: Move to game specific folder?

public class BehaviorCandleHealPlayer : Behavior
{
    private int HealAmount { get; }

    public BehaviorCandleHealPlayer(int healAmount)
    {
        HealAmount = healAmount;
    }

    public override void Action()
    {
        var player = EntityManager.PlayerEntity;
        if (player == null)
            return;

        var multiplier = player.StatsManager.GetMultipliedStats(stats => stats.HealOnKillMultiplier);
        player.DamageTaker.HealHp(HealAmount * multiplier);
    }
}
