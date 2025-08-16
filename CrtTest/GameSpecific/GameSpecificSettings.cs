using Engine.GameSpecific;
using Engine.GameSpecific.Placeholders;
using Engine.Main;
using Engine.Types;

namespace CrtTest.GameSpecific;

public class GameSpecificSettings : Engine.GameSpecific.GameSpecificSettings
{
    public override void Initialize()
    {
        CurrentGame = GameId.CrtTest;

        Settings.TileSize = IntVector2.New(16, 16);
        Settings.RoomSizeInTiles = IntVector2.New(16, 14);
        Settings.ScreenSize = Settings.TileSize * Settings.RoomSizeInTiles;
        MainLoop = typeof(CrtTestMainLoop);
        GlobalValues = typeof(CrtTestGlobalValues);

        DefaultTilesetType = typeof(PlaceholderTileset);
        TilesetTypes.Add(typeof(PlaceholderTileset));

        GameFolder = "CrtTest";
        StageFiles.Add("defaultStage");
    }
}
