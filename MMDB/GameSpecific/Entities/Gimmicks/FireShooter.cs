using Engine.ECS.Components.ControlHandling.Behaviors.Shoot;
using Engine.ECS.Components.ControlHandling.Conditions;
using Engine.ECS.Components.PhysicsHandling;
using Engine.ECS.Components.ShootingHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;
using MMDB.GameSpecific.Entities.EnemyShots;

namespace MMDB.GameSpecific.Entities.Gimmicks;

public class FireShooter : Entity
{
    public FireShooter()
    {
        EntityKind = EntityKind.Gimmick;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteCenteredOrigin("FireSpitter", 16, 16);
        AddGimmickComponents(0, SolidType.Solid);

        // Shooter Manager
        Shooter = new Shooter(this);
        Shooter.AddShootAction(() => Shooter.ShootStraight());
        Shooter.RelativeSpawnPosition = IntVector2.New(0, 0);
        Shooter.ShotType = typeof(SpitFire);
        Shooter.ShotModifiers.Add(e => e.Speed.MoveSpeed = 2.25f);

        // States
        AddStateManager();
        // Auto States
        var state = NewState()
            .AddPostProcessingBehaviorWithConditions(new BehaviorShoot(), new ConditionFrameLoop(100))
            .AddToAutomaticStatesList();

        AddCustomValueHandler();
        CustomValueHandler.NewValueSetter(0, "facing", () => Facing.SetXIfNotZero(CustomValueHandler.CustomValues[0].Value));
        CustomValueHandler.NewValueSetter(1, "frame", () => FrameHandler.FastForwardFrames = CustomValueHandler.CustomValues[1].Value);
    }
}