using Engine.ECS.Components.CombatHandling;
using Engine.ECS.Components.ControlHandling.Behaviors.EntityCreation;
using Engine.ECS.Components.VisualsHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Helpers;
using Microsoft.Xna.Framework;
using SpaceMiner.GameSpecific.Entities.Vfx;
using System.Collections.Generic;

namespace SpaceMiner.GameSpecific.Entities.Shots;

public class ResizableShot : Entity
{
    public ResizableShot()
    {
        EntityKind = EntityKind.None;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteFullImageCenteredOrigin("MegaCircle");
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
        VfxAnimation = new(this);
        VfxAnimation.SetInnerSizeReduction(1, 2);
        var colors1 = new List<Color> { CustomColor.PicoRed, CustomColor.PicoOrange, CustomColor.PicoYellow };
        var colors2 = new List<Color> { CustomColor.PicoOrange, CustomColor.PicoYellow, CustomColor.PicoWhite };
        VfxAnimation.SetNestedColorAnimation(4, colors1, colors2);
        VfxAnimation.LoopColors = true;
        VfxAnimation.TrailFrames = 2;

        // State
        AddStateManager();
        var state = NewState();
        StateManager.AutomaticStatesList.Add(state);
    }
}