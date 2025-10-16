using Engine.ECS.Components.ControlHandling.Behaviors.Shoot;
using Engine.ECS.Components.ControlHandling.Conditions;
using Engine.ECS.Components.ControlHandling.States;
using Engine.ECS.Components.PhysicsHandling;
using Engine.ECS.Components.ShootingHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;

namespace Mole.GameSpecific.Entities;

public class TurretBot : Entity
{
    public TurretBot()
    {
        EntityKind = EntityKind.Enemy;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteCenteredOrigin("TurretBot", 16);
        AddMoleEnemyComponents(3, 1);
        AddSolidBehavior(SolidType.NotSolid, SolidInteractionType.StopOnSolids);

        // Enemy specific components

        // Shooter Manager
        Shooter = new Shooter(this);
        Shooter.AddShootAction(() => Shooter.ShootInAllDirections(8));
        Shooter.RelativeSpawnPosition = IntVector2.New(0, 0);
        Shooter.ShotType = typeof(BotBullet);
        Shooter.ShotModifiers.Add(e => e.Speed.MoveSpeed = 1f);

        // States
        AddStateManager();
        // Auto States
        var stateIdle = NewStateWithTimedPattern(new StateDefault("Idle"), (0, 180), (1, 5), (0, 5), (1, 5), (0, 5), (1, 5), (0, 5))
            .AddPostProcessingBehaviorWithConditions(new BehaviorShoot(), new ConditionFrameLoop(210))
            .AddToAutomaticStatesList();
    }
}