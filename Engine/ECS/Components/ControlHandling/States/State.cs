using Engine.ECS.Components.ControlHandling.Behaviors;
using Engine.ECS.Components.ControlHandling.Conditions;
using Engine.Types;
using System.Collections.Generic;

namespace Engine.ECS.Components.ControlHandling.States;

public abstract class State : Component
{
    public string Name { get; protected init; }

    // Frames
    public int Frame { get; set; }
    public int StartUpDuration { get; set; }
    public bool UpdatesFrameOnTransitions { get; set; }

    // Animation
    public int AnimationFrame { get; set; }

    // Direction
    public virtual bool CanUpdateFacing { get; set; } = true;

    // Hitbox and collision
    public IntRectangle? CustomHitbox { get; private set; }
    public virtual bool IsInvincible { get; set; } = false;

    // Movement type
    public virtual MovementType MovementType { get; set; } // Special types of movement that can't be moved Speed.X then Speed.Y

    // Sprite
    public int StartUpSpriteId { get; set; }
    public int StartUpSpriteSpeed { get; set; } = 1;
    public int StartUpSpriteFrames { get; set; } = 1;
    public int SpriteId { get; set; }
    public int SpriteSpeed { get; set; } = 1;
    public int SpriteFrames { get; set; } = 1;
    public List<int> SpritePattern { get; set; }
    public int DirectionFrames { get; set; } // TODO: Move this inside sprite component?

    public virtual bool AllowSecondaryState { get; protected set; }

    // Conditions
    public abstract bool StartCondition();
    public abstract bool KeepCondition();
    public abstract bool PostProcessingKeepCondition();
    public List<Condition> AddedStartConditions { get; } = new(); // Behave like AND
    public List<Condition> OptionalStartConditions { get; } = new(); // Behave like OR
    public List<Condition> AddedKeepConditions { get; } = new();

    // Behaviors
    public virtual void StateSettingBehavior() { } // Frame 0
    public virtual void StartUpBehavior() { } // Frame < StartUpDuration
    public abstract void Behavior(); // Frame >= StartUpDuration
    public List<Behavior> AddedStateSettingBehaviors { get; } = new();
    public List<Behavior> AddedBehaviors { get; } = new();
    public List<Behavior> AddedStateExitBehaviors { get; } = new();
    // Post-processing behaviors - Behaviors that work better post movement (like making a vfx that takes entity's new position into account)
    public virtual void PostProcessingStateSettingBehavior() { }
    public virtual void PostProcessingBehavior() { }
    public List<Behavior> AddedPostProcessingStateSettingBehaviors { get; } = new();
    public List<Behavior> AddedPostProcessingBehaviors { get; } = new();

    public int GetSpriteId()
    {
        // Up and down directions // TODO: Test and improve when animations are implemented
        var directionId = 0;
        if (DirectionFrames > 0)
        {
            directionId = 2;
            if (Owner.Facing.Y == -1) directionId = 0;
            if (Owner.Facing.Y == 1) directionId = 1;
            directionId *= DirectionFrames;
        }

        // Get first frame of animation loop (start up or main), then calculate current frame
        int spriteId;
        if (AnimationFrame < StartUpDuration)
        {
            spriteId = StartUpSpriteId;
            spriteId += AnimationFrame / StartUpSpriteSpeed % StartUpSpriteFrames;
        }
        else
        {
            spriteId = SpriteId;
            var mainFrame = AnimationFrame - StartUpDuration;
            if (SpritePattern == null)
            {
                // Looping animation
                spriteId += mainFrame / SpriteSpeed % SpriteFrames;
            }
            else
            {
                // Pattern animation
                mainFrame = mainFrame / SpriteSpeed % SpritePattern.Count;
                spriteId = SpritePattern[mainFrame];
            }
        }
        spriteId += directionId;
        return spriteId;
    }

    public virtual bool GetFlipAnimation()
    {
        return false;
    }

    public virtual IntVector2 GetAnimationOffset()
    {
        return IntVector2.Zero;
    }

    public virtual void UpdateAnimationFrame()
    {
        AnimationFrame = Frame;
    }

    public void UpdateFacing()
    {
        if (Owner.PlayerControl == null)
            return;

        if (!CanUpdateFacing)
            return;

        Owner.Facing.UpdateFacingBasedOnControlDirection();
    }

    public int GetTotalFrames()
    {
        if (SpritePattern == null)
            return StartUpDuration * StartUpSpriteSpeed + SpriteFrames * SpriteSpeed;
        return StartUpDuration * StartUpSpriteSpeed + SpritePattern.Count * SpriteSpeed;
    }

    public State AddStartCondition(Condition condition)
    {
        AddedStartConditions.Add(condition);
        condition.Owner = Owner;
        return this;
    }

    public State AddOptionalStartCondition(Condition condition)
    {
        OptionalStartConditions.Add(condition);
        condition.Owner = Owner;
        return this;
    }

    public State AddKeepCondition(Condition condition)
    {
        AddedKeepConditions.Add(condition);
        condition.Owner = Owner;
        return this;
    }

    public State AddBehavior(Behavior behavior)
    {
        AddedBehaviors.Add(behavior);
        behavior.Owner = Owner;
        return this;
    }

    public State AddBehaviorWithConditions(Behavior behavior, params Condition[] conditions)
    {
        AddBehavior(behavior);
        foreach (var condition in conditions)
        {
            condition.Owner = Owner;
            behavior.AddCondition(condition);
        }
        return this;
    }

    public State AddPostBehavior(Behavior behavior)
    {
        AddedPostProcessingBehaviors.Add(behavior);
        behavior.Owner = Owner;
        return this;
    }

    public State AddPostProcessingBehaviorWithConditions(Behavior behavior, params Condition[] conditions)
    {
        AddPostBehavior(behavior);
        foreach (var condition in conditions)
        {
            condition.Owner = Owner;
            behavior.AddCondition(condition);
        }
        return this;
    }

    public State AddStateSettingBehavior(Behavior behavior)
    {
        AddedStateSettingBehaviors.Add(behavior);
        behavior.Owner = Owner;
        return this;
    }

    public State AddPostProcessingStateSettingBehavior(Behavior behavior)
    {
        AddedPostProcessingStateSettingBehaviors.Add(behavior);
        behavior.Owner = Owner;
        return this;
    }

    public State AddStateExitBehavior(Behavior behavior)
    {
        AddedStateExitBehaviors.Add(behavior);
        behavior.Owner = Owner;
        return this;
    }

    public State AddStateSettingBehaviorWithConditions(Behavior behavior, params Condition[] conditions)
    {
        AddStateSettingBehavior(behavior);
        foreach (var condition in conditions)
        {
            condition.Owner = Owner;
            behavior.AddCondition(condition);
        }
        return this;
    }

    public State AddToAutomaticStatesList()
    {
        Owner.StateManager.AutomaticStatesList.Add(this);
        return this;
    }

    public State AddToOverrideStatesList()
    {
        Owner.StateManager.OverrideStatesList ??= new List<State>();
        Owner.StateManager.OverrideStatesList.Add(this);
        return this;
    }

    public void SetCustomHitbox(IntRectangle hitboxRectangle) // Ensures entity has dynamic collision box
    {
        CustomHitbox = hitboxRectangle;
        Owner.CollisionBox.Dynamic = true;
    }
}

public enum MovementType
{
    Normal, // Move Y then X. Zeroes the speed axis that hits a wall (the other continues)
    StopOnLedges, // Avoids going beyond edges
    Diagonal, // Move Y and X at the same time (for bouncers, etc.). Zeroes both speeds if one hits a wall (or keep both and bounce?)
    Crawling
}