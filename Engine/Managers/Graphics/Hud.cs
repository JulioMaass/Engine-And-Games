using Engine.ECS.Entities;
using Engine.ECS.Entities.EntityCreation;
using Engine.GameSpecific;
using Engine.Main;
using Engine.Managers.GameModes;
using Engine.Managers.GlobalManagement;
using Engine.Types;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Managers.Graphics;

public static class Hud
{
    public static List<(string String, object Object)> DebugInfoToPrint { set; get; } = new(); // keep it for the whole game
    public static List<(string String, object Object)> FrameDebugInfoToPrint { set; get; } = new(); // reset every frame

    public static void Draw() // TODO - ARCHITECTURE: Simplify this, and check the whole class
    {
        if (GameLoopManager.GameCurrentLoop != null)
        {
            if (GameManager.GameSpecificSettings.CurrentGame == GameId.Mmdb) // TODO: TEMPORARY: Game specific settings should on GameSpecificSettings
            {
                DrawHealthBar(EntityManager.PlayerEntity, 16, 16);
                var bossEntity = EntityManager.GetFilteredEntitiesFrom(EntityKind.Boss).FirstOrDefault();
                if (bossEntity != null)
                    DrawHealthBar(bossEntity, 384 - 32, 16);
            }
            else if (GameManager.GameSpecificSettings.CurrentGame == GameId.Candle)
            {
                var player = EntityManager.PlayerEntity;
                if (player == null)
                    return;

                // number
                var hp = player.DamageTaker.CurrentHp.Amount;
                Video.DrawStringWithOutline(Drawer.PicoFont, "hp: " + hp, IntVector2.Zero + (2, 9), Color.White);

                // candle wick
                var texture = Drawer.TextureDictionary.GetValueOrDefault("CandleWick");
                var textureRepetitions = hp / 16;
                var position = IntVector2.New(2, 2);
                for (var i = 0; i < textureRepetitions; i++)
                    Drawer.DrawTextureRectangleAt(texture, texture.Bounds, IntVector2.New(i * 16, 0) + position);
                var tipSize = hp % 16;
                Drawer.DrawTextureRectangleAt(texture, new IntRectangle(0, 0, tipSize, texture.Bounds.Height), IntVector2.New(textureRepetitions * 16, 0) + position);
            }
            else if (GameManager.GameSpecificSettings.CurrentGame == GameId.Shooter)
            {
                Video.DrawStringWithOutline(Drawer.PicoFont, "Score: " + GlobalManager.Values.Resources.GetAmount(ResourceType.Score),
                    new IntVector2(0, 0), Color.White);
            }
            else if (GameManager.GameSpecificSettings.CurrentGame == GameId.SpaceMiner)
            {
                Video.DrawStringWithOutline(Drawer.PicoFont, "Gray: " + GlobalManager.Values.Resources.GetAmount(ResourceType.OreGray),
                    new IntVector2(2, 2), Color.White);
                Video.DrawStringWithOutline(Drawer.PicoFont, "Blue: " + GlobalManager.Values.Resources.GetAmount(ResourceType.OreBlue),
                    new IntVector2(2, 10), Color.White);
                Video.DrawStringWithOutline(Drawer.PicoFont, "Green: " + GlobalManager.Values.Resources.GetAmount(ResourceType.OreGreen),
                    new IntVector2(2, 18), Color.White);
                Video.DrawStringWithOutline(Drawer.PicoFont, "Red: " + GlobalManager.Values.Resources.GetAmount(ResourceType.OreRed),
                    new IntVector2(2, 26), Color.White);
                Video.DrawStringWithOutline(Drawer.PicoFont, "Yellow: " + GlobalManager.Values.Resources.GetAmount(ResourceType.OreYellow),
                    new IntVector2(2, 34), Color.White);
                Video.DrawStringWithOutline(Drawer.PicoFont, "Atomic: " + GlobalManager.Values.Resources.GetAmount(ResourceType.MissileAtomic),
                    new IntVector2(2, 42), Color.White);
                Video.DrawStringWithOutline(Drawer.PicoFont, "Homing: " + GlobalManager.Values.Resources.GetAmount(ResourceType.MissileHoming),
                    new IntVector2(2, 50), Color.White);

                Video.DrawStringWithOutline(Drawer.PicoFont, "Hp: " + EntityManager.PlayerEntity?.DamageTaker.CurrentHp.Amount,
                    new IntVector2(440, 2), Color.White);
                //Video.DrawStringWithOutline(Drawer.PicoFont, "Time: " + ((SpaceMinerMainLoop)GameLoopManager.GameMainLoop).Timer / 60,
                //    new IntVector2(440, 10), Color.White);
            }
        }

        ScreenTextManager.DrawScreenText();
        DrawMenu();
        DrawDebugHud();
        //DrawStateChecker();
    }

    private static void DrawHealthBar(Entity entity, int x, int y)
    {
        var texture = Drawer.TextureDictionary.GetValueOrDefault("HudBar");
        var paletteTexture = Drawer.TextureDictionary["MMPalette"];

        // Draw the health bar black background
        PaletteManager.SetPalette(paletteTexture, 22);
        Drawer.DrawTextureRectangleAt(texture, texture.Bounds, IntVector2.New(x, y));

        // Draw the health bar foreground
        if (entity != null)
        {
            EntityManager.PlayerEntity.Palette?.SetPalette();
            var sourceRectangle = new IntRectangle(0, 0, 16, entity.DamageTaker.CurrentHp.Amount * 2 + 1);
            var position = IntVector2.New(x, y + (28 - entity.DamageTaker.CurrentHp.Amount) * 2);
            Drawer.DrawTextureRectangleAt(texture, sourceRectangle, position);
        }

        PaletteManager.ResetPalette();
    }

    private static void DrawMenu()
    {
        if (MenuManager.CurrentMenuLayout == null)
            return;

        // Draw background image
        if (MenuManager.CurrentMenuLayout.BackgroundImage != null)
        {
            IntVector2 textureSize = MenuManager.CurrentMenuLayout.BackgroundImage.Bounds.Size;
            var repetitions = MenuManager.CurrentMenuLayout.BackgroundImageSize / textureSize + 1;
            for (var i = 0; i < repetitions.X; i++)
                for (var j = 0; j < repetitions.Y; j++)
                    Drawer.DrawTextureRectangleAt(MenuManager.CurrentMenuLayout.BackgroundImage,
                        MenuManager.CurrentMenuLayout.BackgroundImage.Bounds,
                        new IntVector2(i, j) * textureSize + MenuManager.CurrentMenuLayout.BackgroundImagePosition);
        }

        // Draw menu items
        foreach (var menuEntity in MenuManager.AvailableMenuItems)
            menuEntity.MenuItem.Draw?.Invoke();

        // Draw selection
        MenuManager.SelectedItem.MenuItem.OnSelectDraw?.Invoke();
    }

    private static void DrawDebugHud() // TODO: TEMPORARY: Move debug hud stuff to a new class
    {
        var debugTextPosition = IntVector2.New(2, 2);
        var currentLine = 0;

        //// Draw enemy position
        //var position = EntityManager.GetFilteredEntitiesFrom(EntityKind.Enemy).FirstOrDefault()?.Position.Pixel;
        //var x = position?.X ?? 0;
        //var y = position?.Y ?? 0;
        //DrawDebugLine("enemy position", x + ", " + y);

        //// Draw enemy shot speed
        //var speed = EntityManager.GetFilteredEntitiesFrom(EntityKind.EnemyShot).FirstOrDefault()?.Speed.Value;
        //var xSpeed = speed?.X ?? 0;
        //var ySpeed = speed?.Y ?? 0;
        //DrawDebugLine("shot x", xSpeed);
        //DrawDebugLine("shot y", ySpeed);

        foreach (var value in DebugInfoToPrint)
            DrawDebugLine(value.String, value.Object);

        foreach (var value in FrameDebugInfoToPrint)
            DrawDebugLine(value.String, value.Object);
        FrameDebugInfoToPrint.Clear();

        //// Draw resource amounts
        //var repetitions = 0;
        //foreach (var resource in GlobalManager.Values.Resources.List)
        //{
        //    Video.DrawStringWithOutline(Drawer.PicoFont, resource.ResourceType + ": " + resource.Amount,
        //        IntVector2.Zero + (2, 18 + repetitions * 8), Color.White);
        //    repetitions++;
        //}

        //// Engine optimization checking
        //DrawDebugLine("total entities", EntityManager.GetAllEntities().Count);
        //DrawDebugLine("entity collision checks", DebugMode.EntityCollisionCounter);
        //DrawDebugLine("spawn collision checks", DebugMode.SpawnCollisionCounter);
        //DrawDebugLine("despawn collision checks", DebugMode.DespawnCollisionCounter);
        //DrawDebugLine("tile collisions", DebugMode.TileCollisionCounter);
        //DrawDebugLine("tile checks", DebugMode.TileCheckCounter);
        //DrawDebugLine("draw calls", Video.Graphics.GraphicsDevice.Metrics.DrawCount);
        //DrawDebugLine("solid scans", DebugMode.SolidScanCounter);

        //// MMDB specific
        //DrawDebugLine("charge frame", EntityManager.PlayerEntity?.ChargeManager?.ChargeFrame);

        return;

        void DrawDebugLine(string label, object value)
        {
            var yOffset = currentLine * 8;
            Video.DrawStringWithOutline(Drawer.PicoFont, $"{label}: {value}",
                debugTextPosition + new IntVector2(0, yOffset), Color.White);
            currentLine++;
        }
    }



    private static void DrawStateChecker()
    {
        var debugTextPosition = IntVector2.New(2, 2); //176);
        var entity = EntityManager.GetFilteredEntitiesFrom(EntityKind.Enemy).FirstOrDefault();

        var currentState = entity?.StateManager?.CurrentState?.Name;
        Video.DrawStringWithOutline(Drawer.PicoFont, "current state: " + currentState, debugTextPosition + (0, 0), Color.White);

        var currentCommandedState = entity?.StateManager?.CurrentCommandedState?.Name;
        Video.DrawStringWithOutline(Drawer.PicoFont, "current commanded state: " + currentCommandedState, debugTextPosition + (0, 8), Color.White);

        var stateQueue = entity?.StateManager?.CommandedStatesQueue;
        if (stateQueue != null)
        {
            var stateQueueString = stateQueue.Aggregate("state queue: ", (current, state) => current + state.Name + ", ");
            Video.DrawStringWithOutline(Drawer.PicoFont, stateQueueString, debugTextPosition + (0, 16), Color.White);
        }
    }
}
