using Engine.ECS.Components.CombatHandling;
using Engine.ECS.Components.ControlHandling;
using Engine.ECS.Components.ControlHandling.Behaviors.ComplexMovement.Direction;
using Engine.ECS.Components.ControlHandling.Behaviors.Directions;
using Engine.ECS.Components.ControlHandling.Behaviors.Shoot;
using Engine.ECS.Components.ControlHandling.Behaviors.Speed;
using Engine.ECS.Components.ControlHandling.Behaviors.Sprite;
using Engine.ECS.Components.ControlHandling.Behaviors.Targeting;
using Engine.ECS.Components.ControlHandling.Conditions;
using Engine.ECS.Components.ShootingHandling;
using Engine.ECS.Entities;
using Engine.ECS.Entities.EntityCreation;
using Engine.Managers.StageHandling;
using Engine.Types;
using SpaceMiner.GameSpecific.Entities.Asteroids;

namespace SpaceMiner.GameSpecific.Entities.Enemies;

public class EnemyShip : Entity
{
    public EnemyShip()
    {
        EntityKind = EntityKind.Enemy;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteCenteredOrigin("EnemyShip", 22);

        // Enemy specific components
        AddSpaceMinerEnemyComponents(100, 50);
        DamageDealer.SetPiecingType(PiercingType.PierceAll);
        AddCenteredOutlinedCollisionBox();
        AddMoveDirection();
        AddMoveSpeed(1.5f);
        AddTurnSpeed(2000);
        AddMoveDirection();
        TargetPool = new TargetPool(this);

        // Shooter
        Shooter = new Shooter(this);
        Shooter.AddShootAction(() => Shooter.ShootAimedSpread(72000));
        Shooter.AmountOfShots = 5;
        Shooter.RelativeSpawnPosition = IntVector2.New(0, 0);
        Shooter.ShotType = typeof(AsteroidPurpleShot);
        Shooter.ShotModifiers.Add(e => e.Speed.MoveSpeed = 2.5f);
        AddShootDirection();

        // States
        AddStateManager();
        // Auto States
        var relativePositionGetter = () => new IntVector2(0, -(EntityManager.PlayerEntity.Position.Pixel.Y - StageManager.CurrentRoom.PositionInPixels.Y) / 2);
        var stateDash = NewState(default, 4)
            .AddStateSettingBehavior(new BehaviorTargetNearestEntity(AlignmentType.Friendly, EntityKind.Player))
            .AddStateSettingBehavior(new BehaviorSetDirectionToTarget(MoveDirection, 8, relativePositionGetter))
            .AddStateSettingBehavior(new BehaviorMoveToCurrentDirection())
            .AddBehaviorWithConditions(new BehaviorDecelerateMomentum(40), new ConditionFrame(2, ComparisonType.Greater))
            .AddToAutomaticStatesList();
        // Command States
        var shootFrame = 50;
        var shootStateDuration = 80;
        var stateShootPerfect = NewState()
            .AddStateSettingBehavior(new BehaviorSetDirectionToTarget(ShootDirection, 20))
            .AddStateSettingBehavior(new BehaviorSetSpriteId(() => (ShootDirection.Angle.Value - 270000 + 360000) / 18000 % 4))
            .AddBehaviorWithConditions(new BehaviorShoot(), new ConditionFrameEqual(shootFrame))
            .AddKeepCondition(new ConditionFrameSmaller(shootStateDuration));
        var stateShootFront = NewState()
            .AddStateSettingBehavior(new BehaviorSetDirectionToTarget(ShootDirection, 20, (0, -64)))
            .AddStateSettingBehavior(new BehaviorSetSpriteId(() => (ShootDirection.Angle.Value - 270000 + 360000) / 18000 % 4))
            .AddBehaviorWithConditions(new BehaviorShoot(), new ConditionFrameEqual(shootFrame))
            .AddKeepCondition(new ConditionFrameSmaller(shootStateDuration));

        // Ai Control
        // Get up
        AiControl = new(this);
        AiControl.SetConditionsToTriggerDecision(new ConditionStateFrame(stateDash, 45));
        AiControl.AddSingleStatePool(stateShootPerfect);
        AiControl.AddSingleStatePool(stateShootFront);
    }
}