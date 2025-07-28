using Engine.ECS.Components.ShootingHandling;
using Engine.GameSpecific;
using Engine.Main;
using Engine.Managers;
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

        Input.Up.RebindKey(Keys.W);
        Input.Down.RebindKey(Keys.S);
        Input.Left.RebindKey(Keys.A);
        Input.Right.RebindKey(Keys.D);

        Input.Button2.RebindKey(Keys.W);
        Input.Button3.RebindKey(Keys.Space);
        Input.Button1.MouseButton = MouseButton.Left;
        Input.Button3.MouseButton = MouseButton.Right;
    }
}
