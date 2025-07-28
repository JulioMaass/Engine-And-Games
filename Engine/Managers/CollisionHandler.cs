using Engine.ECS.Components.CombatHandling;
using Engine.ECS.Components.PositionHandling;
using Engine.ECS.Entities;
using Engine.ECS.Entities.EntityCreation;
using Engine.Managers.StageHandling;
using System;
using System.Linq;

namespace Engine.Managers;

public static class CollisionHandler
{
    public static void AlignedEntitiesDealDamage(AlignmentType damageDealerAlignment) // TODO: Simplify this function
    {
        foreach (var damagingEntity in EntityManager.GetAllEntities())
        {
            if (damagingEntity.CollisionBox == null) continue;
            if (damagingEntity.CollisionBox?.BodyType == BodyType.Bypass) continue;
            if (damagingEntity.Alignment?.Type != damageDealerAlignment) continue;
            if (damagingEntity.DamageDealer == null) continue;
            if (damagingEntity.DamageDealer?.DealsDamage == false) continue;

            foreach (var damagedEntity in EntityManager.GetAllEntities())
            {
                if (damagedEntity.CollisionBox == null) continue;
                if (damagedEntity.CollisionBox?.BodyType == BodyType.Bypass) continue;
                if (damagedEntity.CollisionBox?.BodyType == BodyType.Invincible) continue;
                if (damagedEntity.Alignment == null) continue;
                if (damagedEntity.Alignment?.Type == damageDealerAlignment) continue;
                if (damagingEntity.DamageDealer?.HitType == HitType.HitOnce
                    && damagingEntity.DamageDealer.IsInHitList(damagedEntity)) continue;
                if (damagedEntity.DamageTaker == null) continue;
                if (damagedEntity.DamageTaker?.IsInvincible() == true) continue;
                if (damagingEntity.CollisionBox?.CollidesWithEntityPixel(damagedEntity) != true) continue;

                // TODO: Move shield checks elsewhere
                if (damagedEntity.CollisionBox.BodyType == BodyType.Shield)
                {
                    EntityManager.DeleteEntity(damagingEntity);
                    continue;
                }
                if (damagedEntity.CollisionBox.BodyType == BodyType.Shield)
                {
                    var damageDirection = Math.Sign(damagedEntity.Position.Pixel.X - damagingEntity.Position.Pixel.X);
                    if (damageDirection != damagedEntity.Facing.X)
                    {
                        EntityManager.DeleteEntity(damagingEntity);
                        continue;
                    }
                }

                damagingEntity.DamageDealer.RunEffects(damagedEntity);
                damagedEntity.DamageTaker!.BufferDamage(damagingEntity.DamageDealer.Damage);
                damagedEntity.KnockbackReceiver?.TriggerKnockback();
            }
        }
    }

    public static void EntitiesCollideWithTiles()
    {
        foreach (var damagedEntity in EntityManager.GetAllEntities())
        {
            if (damagedEntity.CollisionBox == null) continue; // TODO: Move these checks inside a function, so it's reusable
            if (damagedEntity.CollisionBox?.BodyType == BodyType.Bypass) continue;
            if (damagedEntity.CollisionBox?.BodyType == BodyType.Invincible) continue;
            if (damagedEntity.DamageTaker == null) continue;
            if (damagedEntity.DamageTaker?.IsInvincible() == true) continue;

            var position = damagedEntity.Position.Pixel;
            if (damagedEntity.Physics.TileCollisionChecking.OverlapsWithTileWithPropertyAtPixel(position, TileProperty.Spikes, StageManager.CurrentRoom))
            {
                damagedEntity.DamageTaker?.BufferDamage(10);
                damagedEntity.KnockbackReceiver?.TriggerKnockback();
            }
        }
    }

    public static void EntityTypeGetItems(EntityKind entityType)
    {
        // TODO: Check item getter and item stats instead. Check if ItemGetter has the type of resource before deleting the entity.
        foreach (var entity in EntityManager.GetFilteredEntitiesFrom(entityType))
        {
            foreach (var entity2 in EntityManager.GetFilteredEntitiesFrom(EntityKind.Item).ToList())
            {
                if (entity.CollisionBox?.CollidesWithEntityPixel(entity2) != true) continue;

                entity.ItemGetter.GetItem(entity2);
                EntityManager.DeleteEntity(entity2);
            }
        }
    }
}
