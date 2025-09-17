using Engine.ECS.Components.VisualsHandling;
using Engine.ECS.Entities.EntityCreation;

namespace SpaceMiner.GameSpecific.Entities.Vfx;

public class VfxShotSplash : Entity
{
    public VfxShotSplash()
    {
        EntityKind = EntityKind.Vfx;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteCenteredOrigin("SpaceMinerPlayer", 32);
        Sprite.IsVisible = false;

        // Vfx
        VfxEmitter = new VfxEmitter(this, typeof(VfxShotSplashCircle), 4, 3, 4);
        VfxEmitter.DistanceToSpeedMultiplier = 0.25f;
    }
}