using Engine.ECS.Components.CombatHandling;
using Engine.ECS.Entities.EntityCreation;

namespace MMDB.GameSpecific.Entities.Weapons;

public class MediumCharge : Entity
{
    public MediumCharge()
    {
        EntityKind = EntityKind.PlayerShot;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteCenteredOrigin("MediumCharge", 20, 14);
        AddPlayerShotComponents(2, PiercingType.PierceOnOverkill, 1);

        // Custom components
        AddSpeed(6.5f);
        AddCenteredCollisionBox(12);

        // States
        AddStateManager();
        var state = NewStateWithStartUp(default, 1, 3, 4, 3, 0);
        StateManager.AutomaticStatesList.Add(state);
    }
}