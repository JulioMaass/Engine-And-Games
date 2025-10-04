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
        var allEntities = EntityManager.GetAllEntities().ToList();

        var damagingEntities = allEntities
            .Where(e => e.CollisionBox?.BodyTypeDealsDamage() == true)
            .Where(e => e.Alignment?.Type == damageDealerAlignment)
            .Where(e => e.DamageDealer?.DealsDamage == true)
            .ToList();

        var damagedEntities = allEntities
            .Where(e => e.CollisionBox?.BodyTypeGetsDamaged() == true)
            .Where(e => e.Alignment?.IsHostileTo(damageDealerAlignment) == true)
            .Where(e => e.DamageTaker?.CanBeDamaged() == true)
            .ToList();

        foreach (var damagingEntity in damagingEntities)
        {
            foreach (var damagedEntity in damagedEntities)
            {
                if (damagingEntity.DamageDealer?.HitType == HitType.HitOnce
                    && damagingEntity.DamageDealer.IsInHitList(damagedEntity)) continue;
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
