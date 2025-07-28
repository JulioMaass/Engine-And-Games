using Engine.ECS.Components.ControlHandling.Behaviors;
using Engine.ECS.Components.ControlHandling.Conditions;
using Engine.ECS.Entities.EntityCreation;
using Engine.Helpers;

namespace Candle.GameSpecific.Entities.Enemies;

public class SparkBall : Entity
{
    public SparkBall()
    {
        EntityKind = EntityKind.Enemy;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSprite("SparkBall", 18, 18, 9, 9);
        AddCenteredOutlinedCollisionBox();
        AddCandleEnemyComponents(2, 10);
        AddSolidBehavior();

        // Enemy Specific Components
        AddMoveSpeed(1f);
        Speed.KnockbackSpeed = 2;

        // States
        AddStateManager();

        // Override States
        var stateHurt = NewState(default, 0)
            .AddStartCondition(new ConditionKnockedBack())
            .AddKeepCondition(new ConditionFrameSmaller(20))
            .AddStateSettingBehavior(new BehaviorStop())
            .AddStateSettingBehavior(new BehaviorFacePlayer())
            .AddBehavior(new BehaviorKnockbackMovement())
            .AddBehavior(new BehaviorDecelerateSpeed(20))
            .AddToOverrideStatesList();

        // Auto States
        var stateWave = NewState(default, 0, 6, 4)
            .AddStateSettingBehavior(new BehaviorFacePlayer())
            .AddBehavior(new BehaviorMoveToXFacing())
            .AddBehavior(new BehaviorCircleMovement(Axes.Y, 32, 90, GetRandom.UnseededInt(360000)))
            .AddBehavior(new BehaviorAccelerateForFrames(15))
            .AddToAutomaticStatesList();

        // Ai Control
        //AiControl.AddPermanentBehavior(new BehaviorFacePlayer());
    }
}