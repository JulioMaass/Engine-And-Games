using Engine.ECS.Components.CombatHandling;
using Engine.ECS.Components.PositionHandling;
using Engine.ECS.Entities;
using Engine.ECS.Entities.EntityCreation;
using Engine.Main;
using Engine.Managers.Graphics;
using Engine.Managers.StageHandling;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Managers.CollisionSystem;

public static class CollisionHandler
{
    public static SpatialGrid FriendlyHitterSpatialGrid { get; } = new(AlignmentType.Friendly, HittingRole.Hitter);
    public static SpatialGrid HostileHitterSpatialGrid { get; } = new(AlignmentType.Hostile, HittingRole.Hitter);
    public static SpatialGrid NeutralHitterSpatialGrid { get; } = new(AlignmentType.Neutral, HittingRole.Hitter);
    public static SpatialGrid FriendlyHittableSpatialGrid { get; } = new(AlignmentType.Friendly, HittingRole.Hittable);
    public static SpatialGrid HostileHittableSpatialGrid { get; } = new(AlignmentType.Hostile, HittingRole.Hittable);
    public static SpatialGrid NeutralHittableSpatialGrid { get; } = new(AlignmentType.Neutral, HittingRole.Hittable);

    public static void UpdateSpatialGrids()
    {
        var position = Camera.GetSpawnScreenLimitsWithBorder(Settings.TileSize).Position
            .RoundDownToTileCoordinate();
        FriendlyHitterSpatialGrid.Update(position);
        HostileHitterSpatialGrid.Update(position);
        NeutralHitterSpatialGrid.Update(position);
        FriendlyHittableSpatialGrid.Update(position);
        HostileHittableSpatialGrid.Update(position);
        NeutralHittableSpatialGrid.Update(position);
    }

    private static List<SpatialGrid> GetGridsOpposingTo(AlignmentType alignmentType, HittingRole hittingRole)
    {
        var allGrids = new List<SpatialGrid>
        {
            FriendlyHitterSpatialGrid,
            HostileHitterSpatialGrid,
            NeutralHitterSpatialGrid,
            FriendlyHittableSpatialGrid,
            HostileHittableSpatialGrid,
            NeutralHittableSpatialGrid
        };
        var grids = allGrids.Where(g => g.HittingRole != hittingRole).ToList();
        if (alignmentType != AlignmentType.Neutral)
            grids = grids.Where(g => g.AlignmentType != alignmentType).ToList();
        return grids;
    }

    public static void AlignedEntitiesDealDamage(AlignmentType damageDealerAlignment)
    {
        var damagingEntities = EntityManager.GetEntities(damageDealerAlignment, HittingRole.Hitter);
        var opposingGrids = GetGridsOpposingTo(damageDealerAlignment, HittingRole.Hitter);

        foreach (var damagingEntity in damagingEntities)
        {
            List<Entity> overlappingEntities = new();
            foreach (var grid in opposingGrids)
                overlappingEntities.AddRange(grid.GetOverlappingEntities(damagingEntity));

            foreach (var damagedEntity in overlappingEntities)
            {
#if DEBUG
                if (damagedEntity == damagingEntity)
                    throw new Exception("Entity cannot damage itself.");
#endif

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

    public static void Draw()
    {
        FriendlyHitterSpatialGrid.Draw();
        HostileHitterSpatialGrid.Draw();
        NeutralHitterSpatialGrid.Draw();
        FriendlyHittableSpatialGrid.Draw();
        HostileHittableSpatialGrid.Draw();
        NeutralHittableSpatialGrid.Draw();
    }

    public static void EntitiesCollideWithTiles()
    {
        var damageableEntities = EntityManager.GetAllEntities()
            .Where(e => e.DamageTaker?.CanBeDamaged() == true)
            .ToList();

        foreach (var damagedEntity in damageableEntities)
        {
            var position = damagedEntity.Position.Pixel;
            if (damagedEntity.Physics.TileCollisionChecking.OverlapsWithTileWithPropertyAtPixel(damagedEntity.Position.Pixel, TileProperty.Spikes, StageManager.CurrentRoom))
            {
                damagedEntity.DamageTaker?.BufferDamage(10);
                damagedEntity.KnockbackReceiver?.TriggerKnockback();
            }
        }
    }

    public static void EntityTypeGetItems(EntityKind entityKind)
    {
        // TODO: Check item getter and item stats instead. Check if ItemGetter has the type of resource before deleting the entity.
        foreach (var entity in EntityManager.GetFilteredEntitiesFrom(entityKind))
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
