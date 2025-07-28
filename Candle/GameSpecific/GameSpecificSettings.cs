using Candle.GameSpecific.Entities.BgObjects;
using Candle.GameSpecific.Entities.Currency;
using Candle.GameSpecific.Entities.Enemies;
using Candle.GameSpecific.Entities.Gimmicks;
using Candle.GameSpecific.Entities.Hazards;
using Candle.GameSpecific.Entities.Items;
using Candle.GameSpecific.Tilesets;
using Engine.ECS.Entities.Shared;
using Engine.GameSpecific;
using Engine.Main;
using Engine.Types;

namespace Candle.GameSpecific;

public class GameSpecificSettings : Engine.GameSpecific.GameSpecificSettings
{
    public override void Initialize()
    {
        CurrentGame = GameId.Candle;
        InitialMenu = typeof(Entities.MenuLayouts.MenuLayoutMain);
        PauseMenu = typeof(Entities.MenuLayouts.MenuLayoutPauseMenu);

        Settings.TileSize = IntVector2.New(16, 16);
        Settings.RoomSizeInTiles = IntVector2.New(21, 12); // 21, 12
        Settings.ScreenSize = Settings.TileSize * Settings.RoomSizeInTiles;
        PlayerTypes = new() { typeof(Entities.Candle) };
        MainLoop = typeof(CandleMainLoop);
        GlobalValues = typeof(CandleGlobalValues);

        DefaultTilesetType = typeof(CandleTestTileset);
        TilesetTypes.Add(typeof(CandleTestTileset));
        TilesetTypes.Add(typeof(CemeteryTileset));
        TilesetTypes.Add(typeof(IceTileset));
        TilesetTypes.Add(typeof(ForestTileset));
        TilesetTypes.Add(typeof(ClockworkTileset));
        TilesetTypes.Add(typeof(MinesTileset));
        TilesetTypes.Add(typeof(LabTileset));
        TilesetTypes.Add(typeof(WaterTileset));
        TilesetTypes.Add(typeof(LibraryTileset));

        GameFolder = "CANDLE";
        StageFiles.Add("defaultStageCandle");

        EditorEntityTypes.Add(typeof(Entities.Candle));
        EditorEntityTypes.Add(typeof(TriggerArrowShooter));
        EditorEntityTypes.Add(typeof(Arrow));
        EditorEntityTypes.Add(typeof(TriggerLance));

        EditorEntityTypes.Add(typeof(Rat));
        EditorEntityTypes.Add(typeof(RatGray)); // Armor
        EditorEntityTypes.Add(typeof(Bat));
        EditorEntityTypes.Add(typeof(Cannon));

        EditorEntityTypes.Add(typeof(Shade));
        EditorEntityTypes.Add(typeof(Death));
        EditorEntityTypes.Add(typeof(Eye));
        EditorEntityTypes.Add(typeof(IceCrystal));

        EditorEntityTypes.Add(typeof(LabVial));
        EditorEntityTypes.Add(typeof(Owl));
        EditorEntityTypes.Add(typeof(DrippingCeiling));
        EditorEntityTypes.Add(typeof(RocketShooter));

        EditorEntityTypes.Add(typeof(WheelDoll));
        EditorEntityTypes.Add(typeof(SparkBall));
        EditorEntityTypes.Add(typeof(HeliBomb));
        EditorEntityTypes.Add(typeof(Clock));

        EditorEntityTypes.Add(typeof(Crab));
        EditorEntityTypes.Add(typeof(Frog));
        EditorEntityTypes.Add(typeof(Fish));
        EditorEntityTypes.Add(typeof(WaterTower));

        EditorEntityTypes.Add(typeof(Scissors));
        EditorEntityTypes.Add(typeof(InkPot));
        EditorEntityTypes.Add(typeof(Spider));
        EditorEntityTypes.Add(typeof(RuneBook));

        EditorEntityTypes.Add(typeof(Crystal));
        EditorEntityTypes.Add(typeof(FireflyWanderer));
        EditorEntityTypes.Add(typeof(FireflyChaser));
        EditorEntityTypes.Add(typeof(MoleDigger));
        EditorEntityTypes.Add(typeof(Golem));

        EditorEntityTypes.Add(typeof(Zombie));
        EditorEntityTypes.Add(typeof(Crow));
        EditorEntityTypes.Add(typeof(Ghost));
        EditorEntityTypes.Add(typeof(DarkMage));

        EditorEntityTypes.Add(typeof(Vine));
        EditorEntityTypes.Add(typeof(Moth));
        EditorEntityTypes.Add(typeof(SpikeFlower));
        EditorEntityTypes.Add(typeof(Trunk));

        EditorEntityTypes.Add(typeof(DoubleJumpItem));
        EditorEntityTypes.Add(typeof(DashItem));
        EditorEntityTypes.Add(typeof(ArmorItem));
        EditorEntityTypes.Add(typeof(LeechItem));

        EditorEntityTypes.Add(typeof(SlowburnItem));
        EditorEntityTypes.Add(typeof(ArrowItem));
        EditorEntityTypes.Add(typeof(LanceItem));
        EditorEntityTypes.Add(typeof(BigSlashItem));

        EditorEntityTypes.Add(typeof(CannonTest));
        EditorEntityTypes.Add(typeof(Crate));
        EditorEntityTypes.Add(typeof(SidePlat));
        EditorEntityTypes.Add(typeof(TimedLance));

        EditorEntityTypes.Add(typeof(FallBridge));
        EditorEntityTypes.Add(typeof(IceCeilingSpikes));

        EditorEntityTypes.Add(typeof(TimedArrowShooter));

        EditorEntityTypes.Add(typeof(RespawnPoint));

        EditorEntityTypes.Add(typeof(WaxBall));
        EditorEntityTypes.Add(typeof(WaxBit));
        EditorEntityTypes.Add(typeof(WaxDrop));

        EditorEntityTypes.Add(typeof(BgCandle));
    }
}
