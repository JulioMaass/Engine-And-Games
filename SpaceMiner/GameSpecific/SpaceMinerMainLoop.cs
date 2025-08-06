using Engine.ECS.Components.CombatHandling;
using Engine.ECS.Entities;
using Engine.ECS.Entities.EntityCreation;
using Engine.Helpers;
using Engine.Managers;
using Engine.Managers.GameModes;
using Engine.Managers.Graphics;
using Engine.Managers.StageHandling;
using SpaceMiner.GameSpecific.Entities.Background;
using SpaceMiner.GameSpecific.Managers;
using System.Linq;

namespace SpaceMiner.GameSpecific;

public class SpaceMinerMainLoop : GameLoop
{
    public int Timer;

    public override void GameSpecificSetup()
    {
        BloomManager.IsOn = true;
        Drawer.BackgroundColor = new(7, 14, 27, 255);
        SetTimer();
        GenerateInitialStars();
    }

    private void SetTimer()
    {
        //// Advance time to increase difficulty
        //if (Timer == 0)
        //    Timer = 60 * 60 * 20; // 20 minutes
    }

    private void GenerateInitialStars()
    {
        if (Timer != 0)
            return;
        var amount = StageManager.CurrentRoom.SizeInPixels.Y / 10;
        for (var i = 0; i < amount; i++)
        {
            (int x1, int x2) rangeX = (StageManager.CurrentRoom.PositionInPixels.X, StageManager.CurrentRoom.PositionInPixels.X + StageManager.CurrentRoom.SizeInPixels.X);
            var yPosition = StageManager.CurrentRoom.PositionInPixels.Y + i * 10;
            EntityManager.CreateEntityAt(typeof(Star), (GetRandom.UnseededInt(rangeX.x1, rangeX.x2), yPosition));
        }
    }

    public override void Update()
    {
        // Game specific code
        AsteroidSpawner.Update();
        //CheckToSpawnAsteroids();
        CheckToGenerateStars();

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
        CollisionHandler.AlignedEntitiesDealDamage(AlignmentType.Neutral);
        CollisionHandler.AlignedEntitiesDealDamage(AlignmentType.Hostile);
        CollisionHandler.EntitiesCollideWithTiles();
        ApplyBufferedDamage();

        CheckToGoToShop();

        UpdateAllEntities();
        Timer++;
    }

    private void CheckToGoToShop()
    {
        if (Timer % (60 * 60) == 0) // 60 * 60 = Every 60 seconds
        {
            GameLoopManager.SetGameLoop(new SpaceMinerShopLoop());
            if (Timer != 0)
                AsteroidSpawner.LevelUp();
        }
    }

    private void CheckToGenerateStars()
    {
        if (Timer % (30) != 0)
            return;
        (int x1, int x2) rangeX = (StageManager.CurrentRoom.PositionInPixels.X, StageManager.CurrentRoom.PositionInPixels.X + StageManager.CurrentRoom.SizeInPixels.X);
        var yPosition = StageManager.CurrentRoom.PositionInPixels.Y;
        EntityManager.CreateEntityAt(typeof(Star), (GetRandom.UnseededInt(rangeX.x1, rangeX.x2), yPosition));
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