using Engine.ECS.Components.ControlHandling.Behaviors.Shoot;
using Engine.ECS.Components.ControlHandling.Conditions;
using Engine.ECS.Components.PhysicsHandling;
using Engine.ECS.Components.ShootingHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;
using MMDB.GameSpecific.Entities.Enemies.GasEnemies;

namespace MMDB.GameSpecific.Entities.Gimmicks;

public class FireBouncerSpawner : Entity
{
    public FireBouncerSpawner()
    {
        EntityKind = EntityKind.Gimmick;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteCenteredOrigin("FireBouncerSpawner", 16, 16);
        AddGimmickComponents(0, SolidType.Solid);

        // Shooter Manager
        Shooter = new Shooter(this);
        Shooter.AddShootAction(() => Shooter.ShootAtPlayer());
        Shooter.RelativeSpawnPosition = IntVector2.New(8, 0);
        Shooter.ShotType = typeof(FireBouncer);

        // States
        AddStateManager();
        // Auto States
        var state = NewState()
            .AddBehaviorWithConditions(new BehaviorShoot(), new ConditionFrameLoop(180))
            .AddToAutomaticStatesList();

        AddCustomValueHandler();
        CustomValueHandler.NewValueSetter(0, "facing", () => Facing.SetXIfNotZero(CustomValueHandler.CustomValues[0].Value));
        CustomValueHandler.NewValueSetter(1, "frame", () => FrameHandler.FastForwardFrames = CustomValueHandler.CustomValues[1].Value);
    }
}