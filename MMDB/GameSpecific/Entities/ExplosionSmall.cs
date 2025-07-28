using Engine.ECS.Entities.EntityCreation;

namespace MMDB.GameSpecific.Entities;

public class ExplosionSmall : Entity
{
    public ExplosionSmall()
    {
        EntityKind = EntityKind.Vfx;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteCenteredOrigin("SmallExplosion", 24);
        AddVfxComponents(15);

        // States
        AddStateManager();
        var state = NewState(default, 0, 3, 5);
        StateManager.AutomaticStatesList.Add(state);
    }
}