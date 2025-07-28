using Engine.ECS.Components.CombatHandling;
using Engine.ECS.Entities;
using Engine.ECS.Entities.EntityCreation;
using Engine.Managers;
using Engine.Managers.GameModes;
using Engine.Managers.Graphics;
using Engine.Managers.StageHandling;
using System.Linq;

namespace ShooterGame.GameSpecific;

public class ShooterMainLoop : GameLoop
{
    public override void GameSpecificSetup()
    {
        //LightingManager.IsOn = true;
    }

    public override void Update()
    {
        // Player respawning and entity despawning
        StageManager.CheckToRespawnPlayer();
        foreach (var entity in EntityManager.GetAllEntities())
            entity.SpawnManager?.CheckToDespawn();

        // Update duration counters
        AdvanceDurationCounter();
        foreach (var entity in EntityManager.GetAllEntities())
            entity.StateManager?.AddFrame();

        // Collision handling (it happens 1 frame later, so the player can see the hit before the outcome)
        CollisionHandler.EntityTypeGetItems(EntityKind.Player);
        CollisionHandler.AlignedEntitiesDealDamage(AlignmentType.Friendly);
        CollisionHandler.AlignedEntitiesDealDamage(AlignmentType.Hostile);
        CollisionHandler.EntitiesCollideWithTiles();
        ApplyBufferedDamage();

        // Update all entities
        UpdateAllEntities();

        foreach (var entity in EntityManager.GetAllEntities())
            entity.Gravity?.Apply(); // TODO - BUG - MMDB: Move this elsewhere? Check if jump height is correct after moving this line
    }

    private void UpdateAllEntities()
    {
        UpdateEntitiesOfType(EntityKind.Decoration);
        UpdateEntitiesOfType(EntityKind.DecorationVfx);
        UpdateEntitiesOfType(EntityKind.Gimmick);
        UpdateEntitiesOfType(EntityKind.Player);
        UpdateEntitiesOfType(EntityKind.PlayerShot);
        UpdateEntitiesOfType(EntityKind.Boss);
        UpdateEntitiesOfType(EntityKind.Enemy);
        UpdateEntitiesOfType(EntityKind.EnemyShot);
        UpdateEntitiesOfType(EntityKind.Item);
        UpdateEntitiesOfType(EntityKind.Vfx);
        UpdateEntitiesOfType(EntityKind.Paralax);
    }

    private void ApplyBufferedDamage()
    {
        foreach (var entity in EntityManager.GetAllEntities())
        {
            entity.DamageTaker?.ApplyBufferedDamage();
        }
    }

    private void AdvanceDurationCounter()
    {
        foreach (var entity in EntityManager.GetAllEntities())
        {
            entity.FrameHandler?.AdvanceFrameCounter();
        }
    }

    private void UpdateEntitiesOfType(EntityKind entityKind)
    {
        EntityManager.GetFilteredEntitiesFrom(entityKind).ToList()
            .ForEach(entity => entity.Update());
    }
}