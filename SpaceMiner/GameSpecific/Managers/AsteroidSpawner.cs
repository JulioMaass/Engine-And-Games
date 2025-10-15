using Engine.ECS.Entities;
using Engine.Helpers;
using Engine.Managers.Graphics;
using Engine.Managers.StageHandling;
using Engine.Types;
using SpaceMiner.GameSpecific.Entities.Asteroids;
using SpaceMiner.GameSpecific.Entities.Enemies;
using System;
using System.Collections.Generic;

namespace SpaceMiner.GameSpecific.Managers;

public static class AsteroidSpawner
{
    public static int Timer { get; private set; }
    public static int CurrentLevel { get; private set; } = 1;
    public static List<LevelData> LevelDataList { get; } = new();
    public static List<AsteroidColor> Tier1Asteroids { get; } = new();
    public static List<AsteroidColor> Tier2Asteroids { get; } = new();
    public static List<AsteroidColor> ColorPool { get; } = new();
    public static List<Type> SpawnPool { get; } = new();
    public static int SpaceshipSpawnDelay { get; private set; } = 1;

    public static void Initialize()
    {
        Tier1Asteroids.Add(AsteroidColor.Blue);
        Tier1Asteroids.Add(AsteroidColor.Yellow);
        Tier1Asteroids.Add(AsteroidColor.Green);
        Tier2Asteroids.Add(AsteroidColor.Red);
        Tier2Asteroids.Add(AsteroidColor.Purple);
        Tier2Asteroids.Add(AsteroidColor.Orange);

        //// Level 0 (To avoid off by one error)
        //LevelDataList.Add(new LevelData(20));
        //// Levels 1 - 5
        //LevelDataList.Add(new LevelData(40));
        //LevelDataList.Add(new LevelData(50));
        //LevelDataList.Add(new LevelData(60));
        //LevelDataList.Add(new LevelData(80));
        //LevelDataList.Add(new LevelData(100));
        //// Levels 6 - 10
        //LevelDataList.Add(new LevelData(80, 20));
        //LevelDataList.Add(new LevelData(60, 40));
        //LevelDataList.Add(new LevelData(40, 60));
        //LevelDataList.Add(new LevelData(20, 80));
        //LevelDataList.Add(new LevelData(0, 100));
        //// Levels 11 - 15
        //LevelDataList.Add(new LevelData(20, 40, 60));
        //LevelDataList.Add(new LevelData(20, 40, 80));
        //LevelDataList.Add(new LevelData(20, 40, 100));
        //LevelDataList.Add(new LevelData(20, 40, 80, 20));
        //LevelDataList.Add(new LevelData(20, 40, 50, 50));
        //// Levels 16 - 20
        //LevelDataList.Add(new LevelData(0, 0, 40, 80));
        //LevelDataList.Add(new LevelData(0, 0, 80, 80));
        //LevelDataList.Add(new LevelData(0, 0, 80, 120));
        //LevelDataList.Add(new LevelData(0, 0, 80, 160));
        //LevelDataList.Add(new LevelData(0, 0, 80, 200));

        // Level 0 (To avoid off by one error)
        LevelDataList.Add(new LevelData(20));
        // Levels 1 - 5
        LevelDataList.Add(new LevelData(40));
        LevelDataList.Add(new LevelData(70));
        LevelDataList.Add(new LevelData(100));
        LevelDataList.Add(new LevelData(80, 20));
        LevelDataList.Add(new LevelData(40, 60));
        // Levels 6 - 10
        LevelDataList.Add(new LevelData(20, 40, 60));
        LevelDataList.Add(new LevelData(20, 40, 100));
        LevelDataList.Add(new LevelData(20, 40, 50, 50));
        LevelDataList.Add(new LevelData(0, 0, 80, 80));
        LevelDataList.Add(new LevelData(0, 0, 80, 160));
    }

    public static void LevelUp()
    {
        CurrentLevel++;
        Timer = 0;
    }

    public static void Update()
    {
        if (Timer == 0)
            UpdateSpawnPool();
        if (ItIsAsteroidSpawnTime())
            SpawnAsteroid();
        //if (ItIsShipSpawnTime())
        //    SpawnShip();
        Timer++;
        Hud.FrameDebugInfoToPrint.Add(("Level = " + CurrentLevel, null));
    }

    public static bool ItIsAsteroidSpawnTime()
    {
        var loopTime = 60 * 60; // 60 seconds in frames
        var spawnRate = loopTime / (float)(LevelDataList[CurrentLevel].TotalSpawns * 2); // * 2 to account for small and gray
        return (int)(Timer % spawnRate) == (int)Math.Floor(spawnRate / 2);
    }

    public static bool ItIsShipSpawnTime()
    {
        var loopTime = 60 * 60; // 60 seconds in frames
        if (Timer == 0 || Timer == loopTime)
            return false;
        var spawnRate = loopTime / (CurrentLevel + 2);
        return Timer % spawnRate == 0;
    }


    public static void UpdateSpawnPool()
    {
        // Set type pool
        ColorPool.Clear();
        ColorPool.AddRange(Tier1Asteroids);
        if (CurrentLevel >= 4)
            ColorPool.AddRange(Tier2Asteroids);

        // Randomize colors for this level
        var totalColors = Math.Min(CurrentLevel / 4 + 2, 4);
        var currentColors = new List<AsteroidColor>();
        for (var i = 0; i < totalColors; i++)
            currentColors!.Add(ColorPool.GetRandomAndRemove());

        // Randomize colors and sizes
        var asteroidPropertyList = new List<(AsteroidColor, AsteroidSize, AsteroidStage)>();
        for (int i = 0; i < LevelDataList[CurrentLevel].SmallSpawns; i++)
            asteroidPropertyList.Add((currentColors.GetRandom(), AsteroidSize.Small, AsteroidStage.Stone));
        for (int i = 0; i < LevelDataList[CurrentLevel].BigSpawns; i++)
            asteroidPropertyList.Add((currentColors.GetRandom(), AsteroidSize.Big, AsteroidStage.Stone));
        for (int i = 0; i < LevelDataList[CurrentLevel].SmallSpawns2; i++)
            asteroidPropertyList.Add((currentColors.GetRandom(), AsteroidSize.Small, AsteroidStage.Scrap));
        for (int i = 0; i < LevelDataList[CurrentLevel].BigSpawns2; i++)
            asteroidPropertyList.Add((currentColors.GetRandom(), AsteroidSize.Big, AsteroidStage.Scrap));

        SpawnPool.Clear();
        //for (int i = 0; i < LevelDataList[CurrentLevel].TotalSpawns; i++)
        //    SpawnPool.Add(typeof(Asteroid));
        foreach (var (color, size, stage) in asteroidPropertyList)
        {
            AddAsteroidToSpawnPool(color, size, stage);
            AddAsteroidToSpawnPool(color, size, stage);
            SpawnPool.Add(typeof(Asteroid));
            SpawnPool.Add(typeof(Asteroid));
        }
        //SpawnPool.Add(currentColors.GetRandom());
    }

    public static void AddAsteroidToSpawnPool(AsteroidColor color, AsteroidSize size, AsteroidStage stage)
    {
        Type asteroidType = null;
        if (size == AsteroidSize.Small && stage == AsteroidStage.Stone && color == AsteroidColor.Blue) asteroidType = typeof(AsteroidBlue);
        else if (size == AsteroidSize.Small && stage == AsteroidStage.Stone && color == AsteroidColor.Yellow) asteroidType = typeof(AsteroidYellow);
        else if (size == AsteroidSize.Small && stage == AsteroidStage.Stone && color == AsteroidColor.Green) asteroidType = typeof(AsteroidGreen);
        else if (size == AsteroidSize.Small && stage == AsteroidStage.Stone && color == AsteroidColor.Red) asteroidType = typeof(AsteroidRed);
        else if (size == AsteroidSize.Small && stage == AsteroidStage.Stone && color == AsteroidColor.Purple) asteroidType = typeof(AsteroidPurple);
        else if (size == AsteroidSize.Small && stage == AsteroidStage.Stone && color == AsteroidColor.Orange) asteroidType = typeof(AsteroidOrange);
        else if (size == AsteroidSize.Big && stage == AsteroidStage.Stone && color == AsteroidColor.Blue) asteroidType = typeof(AsteroidBlueBig);
        else if (size == AsteroidSize.Big && stage == AsteroidStage.Stone && color == AsteroidColor.Yellow) asteroidType = typeof(AsteroidYellowBig);
        else if (size == AsteroidSize.Big && stage == AsteroidStage.Stone && color == AsteroidColor.Green) asteroidType = typeof(AsteroidGreenBig);
        else if (size == AsteroidSize.Big && stage == AsteroidStage.Stone && color == AsteroidColor.Red) asteroidType = typeof(AsteroidRedBig);
        else if (size == AsteroidSize.Big && stage == AsteroidStage.Stone && color == AsteroidColor.Purple) asteroidType = typeof(AsteroidPurpleBig);
        else if (size == AsteroidSize.Big && stage == AsteroidStage.Stone && color == AsteroidColor.Orange) asteroidType = typeof(AsteroidOrangeBig);
        else if (size == AsteroidSize.Small && stage == AsteroidStage.Scrap && color == AsteroidColor.Blue) asteroidType = typeof(AsteroidBlue2);
        else if (size == AsteroidSize.Small && stage == AsteroidStage.Scrap && color == AsteroidColor.Yellow) asteroidType = typeof(AsteroidYellow2);
        else if (size == AsteroidSize.Small && stage == AsteroidStage.Scrap && color == AsteroidColor.Green) asteroidType = typeof(AsteroidGreen2);
        else if (size == AsteroidSize.Small && stage == AsteroidStage.Scrap && color == AsteroidColor.Red) asteroidType = typeof(AsteroidRed2);
        else if (size == AsteroidSize.Small && stage == AsteroidStage.Scrap && color == AsteroidColor.Purple) asteroidType = typeof(AsteroidPurple2);
        else if (size == AsteroidSize.Small && stage == AsteroidStage.Scrap && color == AsteroidColor.Orange) asteroidType = typeof(AsteroidOrange2);
        else if (size == AsteroidSize.Big && stage == AsteroidStage.Scrap && color == AsteroidColor.Blue) asteroidType = typeof(AsteroidBlueBig2);
        else if (size == AsteroidSize.Big && stage == AsteroidStage.Scrap && color == AsteroidColor.Yellow) asteroidType = typeof(AsteroidYellowBig2);
        else if (size == AsteroidSize.Big && stage == AsteroidStage.Scrap && color == AsteroidColor.Green) asteroidType = typeof(AsteroidGreenBig2);
        else if (size == AsteroidSize.Big && stage == AsteroidStage.Scrap && color == AsteroidColor.Red) asteroidType = typeof(AsteroidRedBig2);
        else if (size == AsteroidSize.Big && stage == AsteroidStage.Scrap && color == AsteroidColor.Purple) asteroidType = typeof(AsteroidPurpleBig2);
        else if (size == AsteroidSize.Big && stage == AsteroidStage.Scrap && color == AsteroidColor.Orange) asteroidType = typeof(AsteroidOrangeBig2);
        if (asteroidType != null)
            SpawnPool.Add(asteroidType);
    }

    private static void SpawnAsteroid()
    {
        // Screen limits
        var screenLeft = StageManager.CurrentRoom.PositionInPixels.X;
        var screenRight = StageManager.CurrentRoom.PositionInPixels.X + StageManager.CurrentRoom.SizeInPixels.X;
        var screenTop = StageManager.CurrentRoom.PositionInPixels.Y;
        var screenBottom = StageManager.CurrentRoom.PositionInPixels.Y + StageManager.CurrentRoom.SizeInPixels.Y;
        // Create asteroid
        var asteroidType = SpawnPool.GetRandomAndRemove();
        var position = new IntVector2(GetRandom.UnseededInt(screenLeft, screenRight), screenTop);
        var asteroid = EntityManager.CreateEntityAt(asteroidType, position);

        // Set asteroid to move
        asteroid.AddMoveDirection();
        var margin = 128; // To avoid screen sides being too safe. Asteroids may aim for outside the screen if they are spawned in the inner area.
        if (position.X < screenLeft + margin || position.X > screenRight - margin)
            margin = 0;
        var minAngle = Angle.GetDirection(asteroid, (screenRight + margin, screenBottom)).Value;
        var maxAngle = Angle.GetDirection(asteroid, (screenLeft - margin, screenBottom)).Value;
        var minAngleCapped = Math.Max(60000, minAngle);
        var maxAngleCapped = Math.Min(120000, maxAngle);
        asteroid.MoveDirection.Angle = GetRandom.UnseededInt(minAngleCapped, maxAngleCapped);
        asteroid.Speed.SetMoveSpeedToCurrentDirection();
    }

    private static void SpawnShip()
    {
        var screenLeft = StageManager.CurrentRoom.PositionInPixels.X;
        var screenRight = StageManager.CurrentRoom.PositionInPixels.X + StageManager.CurrentRoom.SizeInPixels.X;
        var screenTop = StageManager.CurrentRoom.PositionInPixels.Y;
        var position = new IntVector2(GetRandom.UnseededInt(screenLeft, screenRight), screenTop);
        EntityManager.CreateEntityAt(typeof(EnemyShip), position);
    }
}

public enum AsteroidColor
{
    Blue,
    Yellow,
    Green,
    Red,
    Purple,
    Orange,
}

public enum AsteroidSize
{
    Small,
    Big,
}

public enum AsteroidStage
{
    Stone,
    Scrap,
    Block,
    Bright,
}