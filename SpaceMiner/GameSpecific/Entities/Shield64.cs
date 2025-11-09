using Engine.ECS.Entities.EntityCreation;

namespace SpaceMiner.GameSpecific.Entities;

public class Shield64 : Entity
{
    public Shield64()
    {
        EntityKind = EntityKind.EnemyEffect;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteFullImageCenteredOrigin("Shield64");
        AddCenteredOutlinedCollisionBox();
        AddSpaceMinerEnemyComponents(5, 0);
        DamageTaker.MaxDamagePerHit = 1;
    }
}