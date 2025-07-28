using Engine.ECS.Entities.Shared;
using Engine.GameSpecific;
using Engine.Main;
using Engine.Managers.StageHandling;
using Engine.Types;
using Mole.GameSpecific.Entities;
using Mole.GameSpecific.Tilesets;

namespace Mole.GameSpecific;

public class GameSpecificSettings : Engine.GameSpecific.GameSpecificSettings
{
    public override void Initialize()
    {
        CurrentGame = GameId.Mole;
        InitialMenu = typeof(Entities.MenuLayouts.MenuLayoutMain);

        Settings.TileSize = IntVector2.New(16, 16);
        Settings.RoomSizeInTiles = IntVector2.New(21, 12); // 21, 12
        Settings.ScreenSize = Settings.TileSize * Settings.RoomSizeInTiles;
        PlayerTypes = new() { typeof(Entities.Mole) };
        MainLoop = typeof(MoleMainLoop);
        GlobalValues = typeof(MoleGlobalValues);

        DefaultTilesetType = typeof(MoleTileset);
        TilesetTypes.Add(typeof(MoleTileset));
        GameFolder = "MOLE";
        StageFiles.Add("defaultStageTopdown");

        EditorEntityTypes.Add(typeof(Entities.Mole));
    }
}
