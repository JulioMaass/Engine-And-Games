using Engine.ECS.Components.VisualsHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Helpers;
using Microsoft.Xna.Framework;

namespace SpaceMiner.GameSpecific.Entities.Vfx;

public class VfxDebris : Entity
{
    public VfxDebris()
    {
        EntityKind = EntityKind.Vfx;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteCenteredOrigin("SpaceMinerPlayer", 32);
        Sprite.IsVisible = false;

        // Vfx
        VfxEmitter = new VfxEmitter(this, typeof(VfxDebrisCircle), 4, 3, 4);
        VfxEmitter.DistanceToSpeedMultiplier = 0.25f;
    }
}