using Engine.ECS.Components.CombatHandling;
using Engine.ECS.Components.ControlHandling.Behaviors;
using Engine.ECS.Entities.EntityCreation;

namespace SpaceMiner.GameSpecific.Entities.Shots;

public class SuperBeam : Entity
{
    public SuperBeam()
    {
        EntityKind = EntityKind.None;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        //AddSpriteFullImageCenteredOrigin("WhitePixel");
        AddSprite("WhitePixel", 2, 2, 2 / 2, 2 / 2);
        Sprite.StretchedSize = (32, 400);
        Sprite.CustomStretchedOrigin = (16, 400 - 16);
        Sprite.SetColor(0, 127, 255, 255);
        DrawAs = EntityKind.DecorationVfx;
        AddCollisionBox(Sprite.StretchedSize.X, Sprite.StretchedSize.Y, Sprite.CustomStretchedOrigin.X, Sprite.CustomStretchedOrigin.Y);

        AddAlignment(AlignmentType.Friendly);
        AddDamageDealer(1000, PiercingType.PierceAll);
        AddFrameCounter(180);

        // State
        AddStateManager();
        var state = NewState()
            .AddBehavior(new BehaviorSetPositionTo(() => Alignment.OwningEntity));
        StateManager.AutomaticStatesList.Add(state);
    }
}