using Engine.ECS.Components.ControlHandling.Behaviors.ComplexMovement.Direction;
using Engine.ECS.Components.VisualsHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Helpers;

namespace SpaceMiner.GameSpecific.Entities.Vfx;

public class VfxDebrisCircle : Entity
{
    public VfxDebrisCircle()
    {
        EntityKind = EntityKind.Vfx;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteFullImageCenteredOrigin("MegaCircle");

        // Vfx
        //AddVfxComponents(4);
        //Sprite.Color = CustomColor.White;
        //Sprite.StretchedSize = (32, 32);
        VfxAnimation = new(this);
        VfxAnimation.SetInitialSize(8);
        VfxAnimation.SetSpeed(-0f);
        VfxAnimation.SetAcceleration(-0.20f);
        VfxAnimation.SetSingleColorAnimation(2, CustomColor.PicoDarkGray);
        DrawOrder = 0;

        // State
        AddStateManager();
        var state = NewState()
            .AddBehavior(new BehaviorMoveToCurrentDirection());
        StateManager.AutomaticStatesList.Add(state);
    }
}