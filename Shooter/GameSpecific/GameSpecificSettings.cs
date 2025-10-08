using Engine.GameSpecific;
using Engine.Main;
using Engine.Managers.Input;
using Engine.Types;
using Microsoft.Xna.Framework.Input;
using ShooterGame.GameSpecific.Entities;
using ShooterGame.GameSpecific.Tilesets;

namespace ShooterGame.GameSpecific;

public class GameSpecificSettings : Engine.GameSpecific.GameSpecificSettings
{
    public override void Initialize()
    {
        CurrentGame = GameId.Shooter;
        InitialMenu = typeof(Entities.MenuLayouts.MenuLayoutMain);
        //PauseMenu = typeof(Entities.MenuLayouts.MenuLayoutPauseMenu);

        Settings.TileSize = IntVector2.New(16, 16);
        Settings.RoomSizeInTiles = IntVector2.New(21, 12); // 21, 12
        Settings.ScreenSize = Settings.TileSize * Settings.RoomSizeInTiles;
        PlayerTypes = new() { typeof(ShooterPlayer) };
        MainLoop = typeof(ShooterMainLoop);
        GlobalValues = typeof(ShooterGlobalValues);

        DefaultTilesetType = typeof(ShooterTestTileset);
        TilesetTypes.Add(typeof(ShooterTestTileset));
        //TilesetTypes.Add(typeof(CemeteryTileset));

        GameFolder = "SHOOTER";
        StageFiles.Add("defaultStageShooter");

        EditorEntityTypes.Add(typeof(ShooterPlayer));
        EditorEntityTypes.Add(typeof(ShooterEnemy));
        EditorEntityTypes.Add(typeof(ShooterEnemySpawner));

        GameInput.Up.RebindKey(Keys.W);
        GameInput.Down.RebindKey(Keys.S);
        GameInput.Left.RebindKey(Keys.A);
        GameInput.Right.RebindKey(Keys.D);

        GameInput.Button2.RebindKey(Keys.W);
        GameInput.Button3.RebindKey(Keys.Space);
        GameInput.Button1.RebindMouseButton(MouseButton.Left);
        GameInput.Button3.RebindMouseButton(MouseButton.Right);
    }
}
