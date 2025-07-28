using Engine.ECS.Components.ControlHandling.Behaviors;
using Engine.ECS.Components.ControlHandling.Conditions;
using Engine.ECS.Components.ControlHandling.StateGrouping;
using Engine.ECS.Components.ControlHandling.States;
using Engine.ECS.Entities.EntityCreation;
using Engine.Helpers;
using System.Collections.Generic;

namespace Engine.ECS.Components.ControlHandling;

public class AiControl : Component
{
    public Condition ConditionToTriggerDecision { get; set; }    // TODO: Different triggers could have different pool lists (so it could add a surprise attack mid-pattern, etc.)
    public List<PatternPool> PatternPools { get; } = new();
    public List<Behavior> PermanentBehaviors { get; } = new();

    // TODO: Implement a way to avoid repetition of patterns (same in a row, too much total repetitions, or too much percentile)

    public AiControl(Entity owner)
    {
        Owner = owner;
    }

    public void Update()
    {
        if (ConditionToTriggerDecision?.AllConditionsAreTrue() == true)
        {
            var filteredPatternPools = PatternPools.FindAll(patternPool => patternPool.Conditions.TrueForAll(condition => condition.AllConditionsAreTrue()));
            var patternPool = filteredPatternPools.GetRandom();
            var filteredPatterns = patternPool.Patterns.FindAll(pattern => pattern.Conditions.TrueForAll(condition => condition.AllConditionsAreTrue()));
            var pattern = filteredPatterns.GetRandom();
            Owner.StateManager.CommandedStatesQueue.AddRange(pattern.States);
        }

        foreach (var behavior in PermanentBehaviors)
            behavior.ExecuteIfConditionsAreMet();
    }

    public void AddPermanentBehavior(Behavior behavior)
    {
        behavior.Owner = Owner;
        PermanentBehaviors.Add(behavior);
    }

    public void SetConditionsToTriggerDecision(Condition condition)
    {
        ConditionToTriggerDecision = condition;
        ConditionToTriggerDecision.Owner = Owner;
    }

    public void SetConditionsToTriggerDecision(params Condition[] conditions) // For multiple conditions, first one is the main one, the rest are additional (internal)
    {
        ConditionToTriggerDecision = Owner.GroupedConditions(conditions);
    }

    public void AddSingleStatePool(State state)
    {
        var pattern = new Pattern(state);
        var patternPool = new PatternPool(pattern);
        PatternPools.Add(patternPool);
    }

    public void AddSinglePatternPool(params State[] states)
    {
        var pattern = new Pattern(states);
        var patternPool = new PatternPool(pattern);
        PatternPools.Add(patternPool);
    }
}
