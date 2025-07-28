using Engine.ECS.Components.CombatHandling;
using Engine.ECS.Entities.EntityCreation;

namespace MMDB.GameSpecific.Entities.Weapons;

public class Lemon : Entity
{
    public Lemon()
    {
        EntityKind = EntityKind.PlayerShot;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteCenteredOrigin("Lemon", 14, 8);
        AddPlayerShotComponents(1, PiercingType.PierceNone, 1);

        // Custom components
        AddSpeed(6.5f);
        AddCenteredCollisionBox(8);

        // States
        AddStateManager();
        var state = NewStateWithStartUp(default, 1, 2, 4, 2, 0);
        StateManager.AutomaticStatesList.Add(state);
    }
}