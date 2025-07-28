using Engine.ECS.Components.ControlHandling.Behaviors;
using Engine.ECS.Components.ControlHandling.States;
using Engine.ECS.Components.PhysicsHandling;
using Engine.ECS.Entities.EntityCreation;

namespace Candle.GameSpecific.Entities.BgObjects;

public class FxWaxDrip : Entity
{
    public FxWaxDrip()
    {
        EntityKind = EntityKind.Vfx;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSprite("FxWaxDrip", 16, 16, 8, 8);
        AddCollisionBox(2, 2, 1, 1);
        AddSolidBehavior(SolidType.NotSolid, SolidInteractionType.StopOnSolids);

        SpawnManager.DespawnOnScreenExit = false;
        SpawnManager.PersistsOnTransitions = true;
        AddGravity();

        // States
        AddStateManager();
        var stateFall = NewState(new StateFall())
        .AddToAutomaticStatesList();
        var stateStand = NewState(default, 1)
        .AddStateSettingBehavior(new BehaviorCustom(() => ChangeKind(EntityKind.DecorationVfx)))
        .AddToAutomaticStatesList();
    }
}