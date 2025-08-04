using Engine.ECS.Components.VisualsHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Helpers;
using Engine.Managers;
using Microsoft.Xna.Framework;

namespace SpaceMiner.GameSpecific.Entities.Vfx;

public class VfxSmokeCircle : Entity
{
    public VfxSmokeCircle()
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
        VfxAnimation.SetInitialSize(16);
        VfxAnimation.SetSpeed(-0.75f);
        //VfxAnimation.SetAcceleration(-0.2f);
        VfxAnimation.SetColors(4,CustomColor.Pico25, CustomColor.Pico20, CustomColor.Pico20, CustomColor.Pico21);
        DrawOrder = 0;

        // State
        AddStateManager();
        var state = NewState();
        StateManager.AutomaticStatesList.Add(state);
    }
}