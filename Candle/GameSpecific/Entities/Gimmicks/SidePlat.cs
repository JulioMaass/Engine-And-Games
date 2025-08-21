using Engine.ECS.Components.ControlHandling.Behaviors.ComplexMovement;
using Engine.ECS.Components.PhysicsHandling;
using Engine.ECS.Components.Spawning;
using Engine.ECS.Entities.EntityCreation;

namespace Candle.GameSpecific.Entities.Gimmicks;

public class SidePlat : Entity
{
    public SidePlat()
    {
        EntityKind = EntityKind.Gimmick;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteCenteredOrigin("MovingPlat", 32, 8);

        // Gimmick components
        AddSpeed();
        AddGravity(0);
        AddSolidBehavior(SolidType.SolidTop);
        SpawnManager.EarlySpawn = true;
        AddFullCollisionBox();

        // States
        AddStateManager();
        var state = NewState()
            .AddBehavior(new BehaviorCircleMovement(Engine.Helpers.Axes.X, 64, 180, 180000));
        StateManager.AutomaticStatesList.Add(state);

        AddCustomValueHandler();
        CustomValueHandler.NewValueSetter(0, "frame", () => FrameHandler.FastForwardFrames = CustomValueHandler.CustomValues[0].Value);
    }
}