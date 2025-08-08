using Engine.ECS.Components.ItemsHandling;
using Engine.ECS.Components.ShootingHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;
using SpaceMiner.GameSpecific.Entities.Shots;

namespace SpaceMiner.GameSpecific.Entities.Shooters;

public class ShipShooterMissileDrill : Shooter
{
    public ShipShooterMissileDrill(Entity owner) : base(owner)
    {
        Owner = owner;
        AddShootAction(NewShotInShootDirection);
        RelativeSpawnPosition = IntVector2.New(0, 0);
        EquipmentHolder = new EquipmentHolder(owner);

        // Ammo
        AmmoType = ResourceType.MissileDrill;
        AmmoCost = 1;

        // Shot Properties
        ShotType = typeof(DrillMissile);
        EntityKind = EntityKind.PlayerShot;
    }
}