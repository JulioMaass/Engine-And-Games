using Engine.ECS.Components.CombatHandling;
using Engine.ECS.Entities.EntityCreation;
using MMDB.GameSpecific.States;

namespace MMDB.GameSpecific.Entities.Weapons;

public class BigCharge : Entity
{
    public BigCharge()
    {
        EntityKind = EntityKind.PlayerShot;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteCenteredOrigin("BigCharge", 32);
        AddPlayerShotComponents(3, PiercingType.PierceOnKill, 1);

        // Custom components
        AddSpeed(2f);
        AddCenteredCollisionBox(20);

        // States
        AddStateManager();
        var state = NewStateWithStartUp(new StateSpeedUpAfterDuration(), 3, 2, 4, 6, 0, 2, 3);
        StateManager.AutomaticStatesList.Add(state);
    }
}