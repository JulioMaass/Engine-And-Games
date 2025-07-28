using Engine.ECS.Components.CombatHandling;

namespace Mole.GameSpecific;

public abstract class Entity : Engine.ECS.Entities.EntityCreation.Entity
{
    public void AddMoleEnemyComponents(int hp, int damage)
    {
        // Control components
        AiControl = new(this);

        // Position components
        AddCenteredCollisionBox();
        AddSpeed();

        // Combat components
        AddAlignment(AlignmentType.Hostile);
        AddDamageTaker(hp);
        AddDamageDealer(damage);
    }

    public void AddMoleEnemyShotComponents(int damage)
    {
        // Position components
        AddCenteredOutlinedCollisionBox();
        AddSpeed();

        // Combat components
        AddAlignment(AlignmentType.Hostile);
        AddDamageDealer(damage);
    }
}