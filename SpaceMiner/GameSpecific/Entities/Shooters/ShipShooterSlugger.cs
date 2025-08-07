using Engine.ECS.Components.ItemsHandling;
using Engine.ECS.Components.ShootingHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;
using SpaceMiner.GameSpecific.Entities.Shots;

namespace SpaceMiner.GameSpecific.Entities.Shooters;

public class ShipShooterSlugger : Shooter
{
    public ShipShooterSlugger(Entity owner) : base(owner)
    {
        Owner = owner;
        AddShootAction(ShootWithStatsModifiers);
        RelativeSpawnPosition = IntVector2.New(0, 0);
        EquipmentHolder = new EquipmentHolder(owner, false);
        EquipmentHolder.AddEquipmentSlot(EquipKind.WeaponUpgrade, SlotType.Stack);

        // Shot Properties
        ShotType = typeof(ResizableShot);
        EntityKind = EntityKind.PlayerShot;
        // Blue
        AutoFireRate = 60;
        ShotSpeed = 4.5f;
        // Green
        BaseDamage = 40;
        ShotSize = 16;
        SizeScaling = 6;
        // Yellow
        SpreadAngle = 45000;
        // Red
        BlastBaseSize = 20;
        BlastSizeScaling = 6;
        BlastBaseDamage = 20;
        BlastDamageScaling = 20;
        BlastDuration = 30;
        // Other
    }
}