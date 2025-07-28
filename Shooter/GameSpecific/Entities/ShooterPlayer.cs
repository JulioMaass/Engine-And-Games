using Engine.ECS.Components.CombatHandling;
using Engine.ECS.Components.ControlHandling.Behaviors.Death;
using Engine.ECS.Components.PhysicsHandling;
using Engine.ECS.Entities.EntityCreation;
using ShooterGame.GameSpecific.States;


namespace ShooterGame.GameSpecific.Entities;

public class ShooterPlayer : Entity
{
    public ShooterPlayer()
    {
        EntityKind = EntityKind.Player;
        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSprite("ShooterPlayer", 16, 16, 8, 8);

        AddAlignment(AlignmentType.Friendly);
        AddDamageDealer(0);
        AddDamageTaker(5);
        DamageTaker.SetInvincibilityFrames(60);
        AddDeathHandler(new BehaviorResetScore());

        AddPlayerComponents();
        AddCollisionBox(16, 16, 8, 8);
        AddMoveSpeed(1.4f);
        AddJumpSpeed(3.8f);
        AddDashSpeed(4f);
        AddGravity();
        Gravity.Force = 0.20f;
        AddSolidBehavior(SolidType.NotSolid, SolidInteractionType.StopOnSolids);

        Shooter = new ShooterPlayerShooter(this);

        AddItemGetter();
        EquipmentHolder = new(this, true);

        AddStateManager();
        var state = NewState(new StatePlayerControl());
        StateManager.AutomaticStatesList.Add(state);
    }
}
