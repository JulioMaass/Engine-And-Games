using Engine.ECS.Components.ControlHandling.Behaviors.DeathAndDestroy;
using Engine.ECS.Components.ControlHandling.Behaviors.Shoot;
using Engine.ECS.Components.ControlHandling.Behaviors.Speed;
using Engine.ECS.Components.ControlHandling.Conditions;
using Engine.ECS.Components.ShootingHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;
using Microsoft.Xna.Framework;

namespace Candle.GameSpecific.Entities.EnemyShots;

public class IceCrystalShot : Entity
{
    public IceCrystalShot()
    {
        EntityKind = EntityKind.EnemyShot;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteFullImageCenteredOrigin("IceCrystalShot");
        AddCandleEnemyShotComponents(10, 1);
        AddSolidBehavior();
        AddMoveDirection();

        // Shooter
        Shooter = new Shooter(this);
        Shooter.AddShootAction(() => Shooter.ShootInAllDirections(4, 0));
        Shooter.RelativeSpawnPosition = IntVector2.New(0, 0);
        Shooter.ShotType = typeof(IceCrystalShotSplit);
        Shooter.ShotModifiers.Add(e =>
        {
            e.Speed.MoveSpeed = 3f;
        });

        // States
        AddStateManager();
        var stateExploding = NewState()
            .AddStartCondition(new ConditionCustom(() => Speed.Value == Vector2.Zero))
            .AddBehaviorWithConditions(GroupedBehaviors(new BehaviorShoot(), new BehaviorDestroy()), new ConditionFrame(30, ComparisonType.GreaterOrEqual))
            .AddToAutomaticStatesList();
        var state = NewState()
            .AddBehaviorWithConditions(new BehaviorDecelerateMomentum(30), new ConditionFrame(45, ComparisonType.Greater))
            .AddToAutomaticStatesList();
    }
}