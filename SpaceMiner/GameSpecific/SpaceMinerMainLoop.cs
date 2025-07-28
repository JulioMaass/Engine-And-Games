using Engine.ECS.Components.CombatHandling;
using Engine.ECS.Entities;
using Engine.ECS.Entities.EntityCreation;
using Engine.Helpers;
using Engine.Managers;
using Engine.Managers.GameModes;
using Engine.Managers.GlobalManagement;
using Engine.Managers.StageHandling;
using SpaceMiner.GameSpecific.Entities.Asteroids;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceMiner.GameSpecific;

public class SpaceMinerMainLoop : GameLoop
{
    public int Timer;
    public List<Type> CurrentAsteroidTypes { get; } = new();

    public override void GameSpecificSetup()
    {
        //LightingManager.IsOn = true;
        SetTimer();
    }

    private void SetTimer()
    {
        //// Advance time to increase difficulty
        //if (Timer == 0)
        //    Timer = 60 * 60 * 20; // 20 minutes
    }

    public override void Update()
    {
        // Game specific code
        CheckToSpawnAsteroids();

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

        // Update all entities
        UpdateAllEntities();

        CheckToGoToShop();
    }

    private void CheckToGoToShop()
    {
        if (Timer % (30 * 60) == 0) // 30 * 60 = Every 30 seconds
            GameLoopManager.SetGameLoop(new SpaceMinerShopLoop());
        Timer++;
    }

    private void CheckToSpawnAsteroids()
    {
        var level = Math.Min(Timer / (30 * 60) + 1, 10); // Goes from 1 to 10
        var spawnRate = 11 - level;

        // Re-roll list on level up
        if (Timer % (30 * 60) == 0)
        {
            var specialAsteroidTypes = new List<Type>
            {
                typeof(AsteroidBlueFast),
                typeof(AsteroidGreenBig),
                typeof(AsteroidYellow),
                typeof(AsteroidRedBlast),
            };
            CurrentAsteroidTypes.Clear();
            CurrentAsteroidTypes.Add(typeof(Asteroid));
            var specialTypesTotal = level / 4 + 2;
            for (var i = 0; i < specialTypesTotal; i++)
                CurrentAsteroidTypes.Add(specialAsteroidTypes.GetRandom());
        }

        if (GlobalManager.Values.Timer % spawnRate == 0)
        {
            (int x1, int x2) rangeX = (StageManager.CurrentRoom.PositionInPixels.X, StageManager.CurrentRoom.PositionInPixels.X + StageManager.CurrentRoom.SizeInPixels.X);

            // Randomly select an asteroid type from the list
            var asteroidType = CurrentAsteroidTypes.GetRandom();

            // Spawn asteroid in a random position in the top of the current room
            var yPosition = StageManager.CurrentRoom.PositionInPixels.Y;
            var asteroid = EntityManager.CreateEntityAt(asteroidType, (GetRandom.UnseededInt(rangeX.x1, rangeX.x2), yPosition));

            // Level up buffs
            var buffedHp = asteroid.DamageTaker.CurrentHp.MaxAmount * (1 + level / 2);
            asteroid.AddDamageTaker(buffedHp);
            var buffedSpeed = asteroid.Speed.MoveSpeed * (1 + (level - 1) * 0.125f);
            asteroid.Speed.MoveSpeed = buffedSpeed;
            // Exponential (after level 10)
            var uncappedLevel = Timer / (30 * 60) + 1;
            var levelsPastTen = Math.Max(1, uncappedLevel - 9);
            asteroid.AddDamageTaker(buffedHp * levelsPastTen);

            // Set asteroid to move
            asteroid.AddMoveDirection();
            asteroid.MoveDirection.Angle = 60000 + GetRandom.UnseededInt(0, 60000);
            asteroid.Speed.SetMoveSpeedToCurrentDirection();
        }
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