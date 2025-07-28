namespace Engine.ECS.Components.ControlHandling.Conditions;

public class ConditionFrameLoop : Condition
{
    private int InitialDelay { get; }
    private int Repetitions { get; }
    private int RepetitionDelay { get; }

    public ConditionFrameLoop(int initialDelay, int repetitions = 0, int repetitionDelay = 0)
    {
        InitialDelay = initialDelay;
        Repetitions = repetitions;
        RepetitionDelay = repetitionDelay;
    }

    protected override bool IsTrue()
    {
        // Shoot on frames 60, 90 and 120, for example
        var currentFrame = Owner.StateManager.CurrentState.Frame;
        var cycleLength = InitialDelay + Repetitions * RepetitionDelay; // 120

        if (currentFrame % cycleLength == 0 && currentFrame != 0) // 120 (last frame of the cycle)
            return true;

        if (RepetitionDelay == 0) // if there's no repetition, return
            return false;

        // repetitions
        for (var i = InitialDelay; i <= cycleLength; i += RepetitionDelay)
        {
            if (currentFrame % cycleLength == i) // 60, 90 (120 doesn't work because it resets to 0)
                return true;
        }
        return false;
    }
}
