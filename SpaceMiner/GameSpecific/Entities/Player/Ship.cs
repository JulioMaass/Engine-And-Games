using Engine.ECS.Components.CombatHandling;
using Engine.ECS.Components.ItemsHandling;
using Engine.ECS.Components.PhysicsHandling;
using Engine.ECS.Entities.EntityCreation;
using SpaceMiner.GameSpecific.Entities.Shooters;
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
        WhiteShader = new(this);

        AddAlignment(AlignmentType.Friendly);
        AddDamageDealer(10);
        AddDamageTaker(5);
        DamageTaker.SetInvincibilityFrames(60);

        AddPlayerComponents();
        AddCollisionBox(16, 16, 8, 8);
        AddMoveSpeed(2f);
        AddSolidBehavior(SolidType.NotSolid, SolidInteractionType.StopOnSolids);

        Shooter = new ShipShooterBasic(this);
        AddShootDirection(270000);

        AddItemGetter();
        EquipmentHolder = new(this, true);
        EquipmentHolder.AddEquipmentGroup(EquipGroup.Weapon, true);
        EquipmentHolder.AddEquipmentGroup(EquipGroup.SecondaryWeapon, true);

        AddStateManager();
        var state = NewState(new StatePlayerControl());
        StateManager.AutomaticStatesList.Add(state);
    }

}
