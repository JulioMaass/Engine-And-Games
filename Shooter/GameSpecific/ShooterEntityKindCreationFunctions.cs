using Engine.ECS.Components.CombatHandling;

namespace ShooterGame.GameSpecific;

public abstract class Entity : Engine.ECS.Entities.EntityCreation.Entity
{
    public void AddShooterEnemyComponents(int hp, int damage)
    {
        AiControl = new(this);
        AddAlignment(AlignmentType.Hostile);
        AddDamageTaker(hp);
        AddDamageDealer(damage);
    }

    public void AddShooterEnemyShotComponents(int damage, int hp = 0)
    {
        // Position components
        AddCenteredOutlinedCollisionBox();
        AddSpeed();

        // Combat components
        AddAlignment(AlignmentType.Hostile);
        AddDamageDealer(damage);
        if (hp > 0)
            AddDamageTaker(hp);
    }
}