using Candle.GameSpecific.States;
using Engine.ECS.Components.CombatHandling;
using Engine.ECS.Components.ItemsHandling;
using Engine.ECS.Components.PhysicsHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;


namespace Candle.GameSpecific.Entities;

public class Candle : Entity
{
    public Candle()
    {
        EntityKind = EntityKind.Player;
        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSprite("CandleChar", 20, 24, 10, 16);

        AddAlignment(AlignmentType.Friendly);
        AddDamageDealer(0);
        AddDamageTaker(0);
        DamageTaker.SetInvincibilityFrames(60);

        AddLightSource(IntVector2.Zero, 64, 64 + 32);

        AddPlayerComponents();
        AddCollisionBox(14, 20, 7, 13);
        AddMoveSpeed(1.4f);
        AddJumpSpeed(3.8f);
        AddDashSpeed(4f);
        AddGravity();
        Gravity.Force = 0.20f;
        AddSolidBehavior(SolidType.NotSolid, SolidInteractionType.StopOnSolids);

        Shooter = new CandleSmallSlashShooter(this);

        AddItemGetter();
        EquipmentHolder = new(this, true);
        EquipmentHolder.AddEquipmentGroup(EquipGroup.Weapon, false);
        EquipmentHolder.AddEquipmentGroup(EquipGroup.Armor, false);
        EquipmentHolder.AddEquipmentGroup(EquipGroup.Foot, false);

        AddStateManager();
        var state = NewState(new StatePlayerControl());
        StateManager.AutomaticStatesList.Add(state);
    }
}
