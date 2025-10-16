using Candle.GameSpecific.Entities.EnemyShots;
using Engine.ECS.Components.ControlHandling;
using Engine.ECS.Components.ControlHandling.Behaviors.Facing;
using Engine.ECS.Components.ControlHandling.Behaviors.Shoot;
using Engine.ECS.Components.ControlHandling.Conditions;
using Engine.ECS.Components.ShootingHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;

namespace Candle.GameSpecific.Entities.Enemies;

public class SpikeFlower : Entity
{
    public SpikeFlower()
    {
        EntityKind = EntityKind.Enemy;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSprite("SpikeFlower", 32, 34, 16, 17);
        AddCollisionBox(16, 30, 8, 14);
        AddCandleEnemyComponents(2, 10);
        AddSolidBehavior();

        // Enemy specific components
        Shooter = new Shooter(this);
        Shooter.AddShootAction(() => Shooter.ShootInAllDirections(8, 0, 7));
        Shooter.RelativeSpawnPosition = IntVector2.New(4, -5);
        Shooter.ShotType = typeof(SpikeFlowerShot);
        Shooter.ShotModifiers.Add(e => e.Speed.MoveSpeed = 3f);

        // States
        AddStateManager();
        var stateHurt = NewState(default, 1);
        var stateIdle = NewState();
        var stateFocus = NewState(default, 1);
        var stateShoot = NewState(default, 2);
        // Override States
        stateHurt.AddStartCondition(new ConditionKnockedBack())
            .AddKeepCondition(new ConditionFrameSmaller(20))
            .AddToOverrideStatesList();
        // Auto States
        stateIdle.AddToAutomaticStatesList();
        stateFocus.AddToAutomaticStatesList();
        stateShoot.AddPostProcessingStateSettingBehavior(new BehaviorShoot())
            .AddToAutomaticStatesList();
        MakeStateDurationLoop(true, (stateIdle, 120),
            (stateFocus, 30),
            (stateShoot, 30));
        StateManager.DefaultState = (stateIdle, 90);

        // Ai Control
        AiControl.AddPermanentBehavior(new BehaviorFacePlayer());
    }
}