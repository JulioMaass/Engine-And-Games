using Engine.ECS.Components.ControlHandling.Behaviors.Shoot;
using Engine.ECS.Components.ControlHandling.Conditions;
using Engine.ECS.Components.PhysicsHandling;
using Engine.ECS.Components.ShootingHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;
using Mole.GameSpecific.States;

namespace Mole.GameSpecific.Entities;

public class SpiderBot : Entity
{
    public SpiderBot()
    {
        EntityKind = EntityKind.Enemy;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteCenteredOrigin("SpiderBot", 16, 18);
        AddMoleEnemyComponents(3, 1);
        AddSolidBehavior(SolidType.NotSolid, SolidInteractionType.StopOnSolids);
        AddCollisionBox(16, 16, 8, 8);

        // Enemy specific components
        AddMoveDirection();

        // Shooter Manager
        Shooter = new Shooter(this);
        Shooter.AddShootAction(() => Shooter.ShootAtPlayer());
        Shooter.RelativeSpawnPosition = IntVector2.New(0, 0);
        Shooter.ShotType = typeof(BotBullet);
        Shooter.ShotModifiers.Add(e => e.Speed.MoveSpeed = 1.25f);

        // States // TODO: Walk randomly when player is unreachable and alternate between walking and idle
        AddStateManager();
        // Auto States
        var state = NewStateWithTimedPattern(new StateWalkAwayFromPlayer(1.0f), (0, 150), (1, 5), (0, 5), (1, 5), (0, 5), (1, 5), (0, 5))
            .AddBehaviorWithConditions(new BehaviorShoot(), new ConditionFrameLoop(180))
            .AddToAutomaticStatesList();
    }
}