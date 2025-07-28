using Engine.ECS.Components.ControlHandling.Behaviors;
using Engine.ECS.Components.ControlHandling.Conditions;
using Engine.ECS.Components.ShootingHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.ECS.Components.ControlHandling.States;
using Engine.Types;

namespace Candle.GameSpecific.Entities.Enemies;

public class DrippingCeiling : Entity
{
    public DrippingCeiling()
    {
        EntityKind = EntityKind.Enemy;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteFullImageCenteredOrigin("DrippingCeiling");
        AddCenteredOutlinedCollisionBox();
        AddCandleEnemyComponents(2, 10);
        AddSolidBehavior();

        // Enemy specific components
        Shooter = new Shooter(this);
        Shooter.AddShootAction(() => Shooter.ShootStill());
        Shooter.RelativeSpawnPosition = IntVector2.New(0, 0);
        Shooter.ShotType = typeof(CeilingDrop);

        // States
        AddStateManager();
        // Auto States
        var idleState = NewState(new StateIdleAndFall())
            .AddBehavior(new BehaviorFacePlayer())
            .AddBehaviorWithConditions(GroupedBehaviors(new BehaviorShoot(), new BehaviorResetFrame()),
                new ConditionFrame(new RandomInt(150, 180), ComparisonType.Equal))
            .AddToAutomaticStatesList();
    }
}