using Engine.ECS.Entities;
using Engine.ECS.Entities.EntityCreation;

namespace Engine.ECS.Components.Spawning;

public class FrameHandler : Component
{
    public int FastForwardFrames { get; set; } // Makes the entity run int frames on spawn (to sync gimmicks, etc)
    public int CurrentFrame { get; private set; }
    public int EntityDuration { get; private set; }
    private bool TriggerDeathAtDurationEnd { get; }

    public FrameHandler(Entity owner, int entityDuration, bool triggerDeathAtDurationEnd)
    {
        Owner = owner;
        EntityDuration = entityDuration;
        TriggerDeathAtDurationEnd = triggerDeathAtDurationEnd;
    }

    public void SetFrame(int frame) =>
        CurrentFrame = frame;

    public void AdvanceFrameCounter() =>
        CurrentFrame++;

    public void CheckDurationEnd()
    {
        if (EntityDuration == 0)
            return;

        if (CurrentFrame >= EntityDuration)
            TriggerDurationEnd();
    }

    private void TriggerDurationEnd()
    {
        if (TriggerDeathAtDurationEnd)
            EntityManager.TriggerDeath(Owner);
        else
            EntityManager.MarkEntityForDeletion(Owner);
    }
}
