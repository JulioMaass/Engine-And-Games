namespace Engine.ECS.Components.ControlHandling.SecondaryStates;

public abstract class SecondaryState : Component
{
    // Frames
    public int Frame { get; set; }
    public int Duration { get; set; }

    // Sprite
    public int SpriteIdOffset { get; set; }

    // Conditions
    public abstract bool StartCondition();
    public abstract bool KeepCondition();

    // Behaviors
    public virtual void StateSettingBehavior() { } // Frame 0
    public virtual void PostProcessingStateSettingBehavior() { } // Frame 0
    public abstract void Behavior(); // Frame >= StartUpDuration
}