using Engine.ECS.Entities.EntityCreation;

namespace MMDB.GameSpecific.Entities;

public class ExplosionRobotMaster : Entity
{
    public ExplosionRobotMaster()
    {
        EntityKind = EntityKind.Vfx;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteCenteredOrigin("SmallExplosion", 24);
        AddSpeed();

        // States
        AddStateManager();
        var state = NewState(default, 0, 3, 5);
        StateManager.AutomaticStatesList.Add(state);
    }
}