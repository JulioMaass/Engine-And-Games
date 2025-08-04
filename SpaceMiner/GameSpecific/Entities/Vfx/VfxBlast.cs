using Engine.ECS.Components.VisualsHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Helpers;
using Microsoft.Xna.Framework;

namespace SpaceMiner.GameSpecific.Entities.Vfx;

public class VfxBlast : Entity
{
    public VfxBlast()
    {
        EntityKind = EntityKind.Vfx;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteCenteredOrigin("SpaceMinerPlayer", 32);
        Sprite.IsVisible = false;

        // Vfx
        VfxEmitter = new VfxEmitter(this, typeof(VfxBlastCircle), 10, 2, 32);
    }
}