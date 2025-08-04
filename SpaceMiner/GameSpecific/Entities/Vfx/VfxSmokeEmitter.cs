using Engine.ECS.Components.VisualsHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Helpers;
using Microsoft.Xna.Framework;

namespace SpaceMiner.GameSpecific.Entities.Vfx;

public class VfxSmokeEmitter : Entity
{
    public VfxSmokeEmitter()
    {
        EntityKind = EntityKind.Vfx;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteCenteredOrigin("SpaceMinerPlayer", 32);
        Sprite.IsVisible = false;

        // Vfx
        VfxEmitter = new VfxEmitter(this, typeof(VfxSmoke), 10, 2, 32);
    }
}