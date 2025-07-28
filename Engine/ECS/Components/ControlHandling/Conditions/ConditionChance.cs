using Engine.Helpers;

namespace Engine.ECS.Components.ControlHandling.Conditions;

public class ConditionChance : Condition
{
    private int Chance { get; }

    public ConditionChance(int chance)
    {
        Chance = chance;
    }

    protected override bool IsTrue()
    {
        return GetRandom.UnseededInt(100) < Chance;
    }
}
