using Engine.ECS.Entities.Shared;
using Engine.GameSpecific;
using Engine.Main;
using Engine.Managers.StageHandling;
using Engine.Types;
using MMDB.GameSpecific.Entities.Background;
using MMDB.GameSpecific.Entities.Bosses.Disco;
using MMDB.GameSpecific.Entities.Chars;
using MMDB.GameSpecific.Entities.Enemies;
using MMDB.GameSpecific.Entities.Enemies.GasEnemies;
using MMDB.GameSpecific.Entities.Gimmicks;
using MMDB.GameSpecific.Entities.Paralax;
using MMDB.GameSpecific.Tilesets;

namespace MMDB.GameSpecific;

public class GameSpecificSettings : Engine.GameSpecific.GameSpecificSettings
{
    public override void Initialize()
    {
        CurrentGame = GameId.Mmdb;
        InitialMenu = typeof(Entities.MenuLayouts.MenuLayoutMain);
        PauseMenu = typeof(Entities.MenuLayouts.MenuLayoutPauseMenu);

        Settings.ScreenSize = IntVector2.New(384, 224); // 384, 224
        Settings.TileSize = IntVector2.New(16, 16);
        Settings.RoomSizeInTiles = IntVector2.New(24, 14);
        PlayerTypes = new() { typeof(MegaMan) };
        MainLoop = typeof(MmdbMainLoop);
        GlobalValues = typeof(MmdbGlobalValues);

        DefaultTilesetType = typeof(IntroTileset);
        TilesetTypes.Add(typeof(IntroTileset));
        TilesetTypes.Add(typeof(ChainTileset));
        TilesetTypes.Add(typeof(GasTileset));
        TilesetTypes.Add(typeof(GhostTileset));
        TilesetTypes.Add(typeof(JungleTileset));
        TilesetTypes.Add(typeof(OceanTileset));
        TilesetTypes.Add(typeof(PoliceTileset));
        TilesetTypes.Add(typeof(RemoteTileset));
        TilesetTypes.Add(typeof(VacuumTileset));
        TilesetTypes.Add(typeof(MmdbTestTileset));
        GameFolder = "MMDB";
        StageFiles.Add("defaultStagePlatformer");
        StageFiles.Add("policeStage");
        StageFiles.Add("chainStage");
        StageFiles.Add("vacuumStage");
        StageFiles.Add("jungleStage");
        StageFiles.Add("oceanStage");
        StageFiles.Add("gasStage");
        StageFiles.Add("ghostStage");
        StageFiles.Add("remoteStage");

        EditorEntityTypes.Add(typeof(MegaMan));
        EditorEntityTypes.Add(typeof(RespawnPoint));

        EditorEntityTypes.Add(typeof(BunbyTankBottom));
        EditorEntityTypes.Add(typeof(BunbyTankTop));

        EditorEntityTypes.Add(typeof(Spine));
        EditorEntityTypes.Add(typeof(BunbyTank));
        EditorEntityTypes.Add(typeof(SniperJoe));
        EditorEntityTypes.Add(typeof(Cannopeller));

        EditorEntityTypes.Add(typeof(ShieldAttacker));
        EditorEntityTypes.Add(typeof(MetShooter));
        EditorEntityTypes.Add(typeof(MetJumper));

        EditorEntityTypes.Add(typeof(FireBouncer));
        EditorEntityTypes.Add(typeof(Lamp));
        EditorEntityTypes.Add(typeof(Firefly));
        EditorEntityTypes.Add(typeof(FireBouncerSpawner));
        EditorEntityTypes.Add(typeof(LameGate));
        EditorEntityTypes.Add(typeof(Disco));
        EditorEntityTypes.Add(typeof(GravityShooter));
        EditorEntityTypes.Add(typeof(ShotJumper));
        EditorEntityTypes.Add(typeof(BlobBob));
        EditorEntityTypes.Add(typeof(BulbJumper));

        EditorEntityTypes.Add(typeof(SuzyX));
        EditorEntityTypes.Add(typeof(SuzyY));
        EditorEntityTypes.Add(typeof(Telly));

        EditorEntityTypes.Add(typeof(BunbyTank));
        EditorEntityTypes.Add(typeof(BunbyTankBottom));
        EditorEntityTypes.Add(typeof(BunbyTankTop));
        EditorEntityTypes.Add(typeof(Cannopeller));
        EditorEntityTypes.Add(typeof(MetJumper));
        EditorEntityTypes.Add(typeof(MetShooter));
        EditorEntityTypes.Add(typeof(ShieldAttacker));
        EditorEntityTypes.Add(typeof(SniperJoe));
        EditorEntityTypes.Add(typeof(Spine));

        EditorEntityTypes.Add(typeof(PoliceHeli));
        EditorEntityTypes.Add(typeof(FireShooter));

        EditorEntityTypes.Add(typeof(SpinStar));

        // Gimmicks
        EditorEntityTypes.Add(typeof(SwingPlat));
        EditorEntityTypes.Add(typeof(ConveyorBeltRight));
        EditorEntityTypes.Add(typeof(DestructibleBlock)); // TODO: Carriable carriers / pushable pushers are not implemented (chain reactions). Only push NotSolid for now.

        // Paralax and Background
        EditorEntityTypes.Add(typeof(IntroCloud));
        EditorEntityTypes.Add(typeof(IntroMountain));

        EditorEntityTypes.Add(typeof(PoliceBuilding));
        EditorEntityTypes.Add(typeof(PoliceBadge));

        EditorEntityTypes.Add(typeof(ChainCloud1));
        EditorEntityTypes.Add(typeof(ChainCloud2));
        EditorEntityTypes.Add(typeof(ChainCloud3));
        EditorEntityTypes.Add(typeof(ChainMountain));
        EditorEntityTypes.Add(typeof(ChainPowerLines));
        EditorEntityTypes.Add(typeof(ChainLinksBigUp));
        EditorEntityTypes.Add(typeof(ChainLinksBigDown));
        EditorEntityTypes.Add(typeof(ChainLinksSmallUp));
        EditorEntityTypes.Add(typeof(ChainLinksSmallDown));
        EditorEntityTypes.Add(typeof(ChainPowerCore));

        EditorEntityTypes.Add(typeof(GhostMoonAndClouds));
        EditorEntityTypes.Add(typeof(GhostTombstones));
        EditorEntityTypes.Add(typeof(GhostTombstonesAndTrees1));
        EditorEntityTypes.Add(typeof(GhostTombstonesAndTrees2));

        EditorEntityTypes.Add(typeof(RemoteCloud));

        EditorEntityTypes.Add(typeof(OceanPalmTree));

        var topMargin = Settings.TileSize.Height * 4;
        var bottomMargin = Settings.TileSize.Height * 2;
        StageManager.GameSpecificSetup(topMargin, bottomMargin);
    }
}
