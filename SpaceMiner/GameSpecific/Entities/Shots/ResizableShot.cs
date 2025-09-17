using Engine.ECS.Components.CombatHandling;
using Engine.ECS.Components.ControlHandling.Behaviors.EntityCreation;
using Engine.ECS.Components.VisualsHandling;
using Engine.ECS.Entities.EntityCreation;
using SpaceMiner.GameSpecific.Entities.Vfx;

namespace SpaceMiner.GameSpecific.Entities.Shots;

public class ResizableShot : Entity
{
    public ResizableShot()
    {
        EntityKind = EntityKind.None;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteFullImageCenteredOrigin("MegaCircle");
        Sprite.SetColor(255, 127, 0, 255);
        Sprite.Resizable = true;
        AddSolidBehavior();
        AddCenteredOutlinedCollisionBox();

        AddMoveDirection();
        AddMoveSpeed(4.5f);
        AddAlignment(AlignmentType.Friendly);
        AddDamageDealer(10, PiercingType.PierceOnOverkill);
        //DamageDealer.AddOnHitTargetBehavior(new BehaviorAddSpeed(0f, -0.25f));
        AddDeathHandler(new BehaviorCreateEntity(typeof(VfxShotSplash)));

        // Vfx
        VfxEmitter = new VfxEmitter(this, typeof(VfxShotSplashCircle), 0, 1, 2);
        VfxEmitter.DistanceToSpeedMultiplier = 0.25f;

        // State
        AddStateManager();
        var state = NewState();
        StateManager.AutomaticStatesList.Add(state);
    }
}