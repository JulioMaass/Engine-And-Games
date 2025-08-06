using Engine.GameSpecific;
using Engine.Main;
using Engine.Types;
using SpaceMiner.GameSpecific.Entities.Menus.MainLayout;
using SpaceMiner.GameSpecific.Entities.Player;
using SpaceMiner.GameSpecific.Managers;
using SpaceMiner.GameSpecific.Tilesets;

namespace SpaceMiner.GameSpecific;

public class GameSpecificSettings : Engine.GameSpecific.GameSpecificSettings
{
    public override void Initialize()
    {
        CurrentGame = GameId.SpaceMiner;
        InitialMenu = typeof(MenuLayoutMain);
        //PauseMenu = typeof(Entities.MenuLayouts.MenuLayoutPauseMenu);

        Settings.TileSize = IntVector2.New(16, 16);
        Settings.RoomSizeInTiles = IntVector2.New(30, 17);
        Settings.ScreenSize = Settings.TileSize * Settings.RoomSizeInTiles;
        PlayerTypes = new() { typeof(Ship) };
        MainLoop = typeof(SpaceMinerMainLoop);
        GlobalValues = typeof(SpaceMinerGlobalValues);

        DefaultTilesetType = typeof(SpaceMinerTestTileset);
        TilesetTypes.Add(typeof(SpaceMinerTestTileset));
        //TilesetTypes.Add(typeof(CemeteryTileset));

        GameFolder = "SPACEMINER";
        StageFiles.Add("defaultStageSpaceMiner");

        EditorEntityTypes.Add(typeof(Ship));

        AsteroidSpawner.Initialize();
    }
}
