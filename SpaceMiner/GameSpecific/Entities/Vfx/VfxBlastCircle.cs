using Engine.ECS.Components.VisualsHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Helpers;

namespace SpaceMiner.GameSpecific.Entities.Vfx;

public class VfxBlastCircle : Entity
{
    public VfxBlastCircle()
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
        VfxAnimation.SetInitialSize(32);
        VfxAnimation.SetSpeed(3);
        VfxAnimation.SetAcceleration(-1f);
        VfxAnimation.SetSingleColorAnimation(2, CustomColor.PicoWhite, CustomColor.PicoYellow,
            CustomColor.PicoOrange, CustomColor.Pico25, CustomColor.Pico20, CustomColor.Pico21);
        DrawOrder = 1;

        // State
        AddStateManager();
        var state = NewState();
        StateManager.AutomaticStatesList.Add(state);
    }
}