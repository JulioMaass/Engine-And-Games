using Engine.ECS.Components.ControlHandling.Behaviors.Directions;
using Engine.ECS.Components.ControlHandling.Behaviors.Facing;
using Engine.ECS.Components.ControlHandling.Behaviors.Shoot;
using Engine.ECS.Components.ControlHandling.Behaviors.Speed;
using Engine.ECS.Components.ControlHandling.Conditions;
using Engine.ECS.Components.ShootingHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;
using Microsoft.Xna.Framework;
using MMDB.GameSpecific.Entities.EnemyShots;

namespace MMDB.GameSpecific.Entities.Enemies;

public class Cannopeller : Entity
{
    public Cannopeller()
    {
        EntityKind = EntityKind.Enemy;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSprite("Cannopeller", 26, 25, 14, 9);
        AddMmdbEnemyComponents(4, 4);
        AddSolidBehavior();
        AddCollisionBox(20, 21, 10, 8);

        // Enemy specific components
        AddMoveDirection(90000);
        AddSpeed();
        Speed.Acceleration = 0.035f;
        Speed.MaxSpeed = 1.3f;

        // Shooter Manager
        Shooter = new Shooter(this);
        Shooter.AddShootAction(() => Shooter.ShootAtSpeed(new Vector2(3f, -0.5f)));
        Shooter.RelativeSpawnPosition = IntVector2.New(8, -2);
        Shooter.ShotType = typeof(CannonBomb);
        Shooter.ShotModifiers.Add(e => e.Speed.MoveSpeed = 3f);

        // States
        AddStateManager();
        // Auto States
        var state = NewState()
            .AddBehavior(new BehaviorAccelerateToDirection())
            .AddBehaviorWithConditions(new BehaviorReverseAngleDirection(MoveDirection), new ConditionCollidesAtDistanceWithCurrentDirection(48),
                new ConditionYSpeedBiggerThan(1.0f))
            .AddPostProcessingBehaviorWithConditions(new BehaviorShoot(), new ConditionFrameLoop(90))
            .AddToAutomaticStatesList();

        // Mega Man 6 Cannopeller:
        // 208 frames to go up n down
        // 64 to 82 frames to shoot (average 73)

        // Ai Control
        AiControl.AddPermanentBehavior(new BehaviorFacePlayer());


        //// Auto States
        //var _stateMove = NewState(new StateDefault("Move"), 0, 4, 3)
        //    .AddBehavior(new BehaviorMoveToXFacing())
        //    .AddBehaviorWithConditions(new BehaviorDashToXFacing(), new ConditionPlayerYDistanceRange(0, 16))
        //    .AddBehaviorWithConditions(new BehaviorMirrorXFacing(), new ConditionFacingCollides())
        //    .AddToAutomaticStatesList();
    }
}