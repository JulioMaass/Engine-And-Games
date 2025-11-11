using Engine.ECS.Components.CombatHandling;
using Engine.ECS.Components.PhysicsHandling;
using Engine.ECS.Entities.EntityCreation;
using SpaceMiner.GameSpecific.States;


namespace SpaceMiner.GameSpecific.Entities.Player;

public class Ship : Entity
{
    public Ship()
    {
        EntityKind = EntityKind.Player;
        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteFullImageCenteredOrigin("SpaceMinerPlayer");
        ColorShader = new(this);
        ColorShader.FlickerWhiteOn = true;

        AddAlignment(AlignmentType.Friendly);
        AddDamageDealer(10);
        AddDamageTaker(250);
        DamageTaker.SetInvincibilityFrames(60);

        AddPlayerComponents();
        AddCollisionBox(12, 12, 6, 6);
        AddMoveSpeed(1.25f);
        Speed.DashSpeed = 0f;

        //Shooter = new ShipShooterBasic(this);
        AddShootDirection(270000);

        AddItemGetter(32);
        EquipmentHolder = new(this);

        AddStateManager();
        var state = NewState(new StatePlayerControl());
        StateManager.AutomaticStatesList.Add(state);
    }
}
