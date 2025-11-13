using Engine.ECS.Entities;
using Engine.ECS.Entities.EntityCreation;
using Engine.GameSpecific;
using Engine.Helpers;
using Engine.Main;
using Engine.Managers.GameModes;
using Engine.Managers.GlobalManagement;
using Engine.Types;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Managers.Graphics;

public static class Hud
{
    public static List<(string String, object Object)> DebugInfoToPrint { set; get; } = new(); // keep it for the whole game
    public static List<(string String, object Object)> FrameDebugInfoToPrint { set; get; } = new(); // reset every frame

    public static void Draw() // TODO: - ARCHITECTURE: Simplify this, and check the whole class
    {
        if (GameLoopManager.GameCurrentLoop != null)
        {
            if (GameManager.GameSpecificSettings.CurrentGame == GameId.Mmdb) // TODO: TEMPORARY: Game specific settings should on GameSpecificSettings
            {
                Video.SwitchSpriteSortMode(SpriteSortMode.Immediate);
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
                StringDrawer.DrawStringOutlined(StringDrawer.TinyUnicodeSoftFont, "HP: " + hp, IntVector2.Zero + (2, 9), Color.White);

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
                StringDrawer.DrawStringOutlined(StringDrawer.TinyUnicodeSoftFont, "Score: " + GlobalManager.Values.MainCharData.Resources.GetAmount(ResourceType.Score),
                    new IntVector2(0, 0), Color.White);
            }
            else if (GameManager.GameSpecificSettings.CurrentGame == GameId.SpaceMiner)
            {
                Video.SpriteBatch.Draw(Drawer.TextureDictionary.GetValueOrDefault("OreGray"), new IntRectangle(1, 1 + 8 * 0, 8, 8), new IntRectangle(8, 0, 8, 8), CustomColor.White);
                StringDrawer.DrawStringOutlined(StringDrawer.TinyUnicodeSoftFont, "  " + GlobalManager.Values.MainCharData.Resources.GetAmount(ResourceType.OreGray),
                    new IntVector2(2, 2 + 8 * 0), Color.White);

                Video.SpriteBatch.Draw(Drawer.TextureDictionary.GetValueOrDefault("OreBlue"), new IntRectangle(1, 1 + 8 * 1, 8, 8), new IntRectangle(8, 0, 8, 8), CustomColor.White);
                StringDrawer.DrawStringOutlined(StringDrawer.TinyUnicodeSoftFont, "  " + GlobalManager.Values.MainCharData.Resources.GetAmount(ResourceType.OreBlue),
                    new IntVector2(2, 2 + 8 * 1), Color.White);

                Video.SpriteBatch.Draw(Drawer.TextureDictionary.GetValueOrDefault("OreGreen"), new IntRectangle(1, 1 + 8 * 2, 8, 8), new IntRectangle(8, 0, 8, 8), CustomColor.White);
                StringDrawer.DrawStringOutlined(StringDrawer.TinyUnicodeSoftFont, "  " + GlobalManager.Values.MainCharData.Resources.GetAmount(ResourceType.OreGreen),
                    new IntVector2(2, 2 + 8 * 2), Color.White);

                Video.SpriteBatch.Draw(Drawer.TextureDictionary.GetValueOrDefault("OreRed"), new IntRectangle(1, 1 + 8 * 3, 8, 8), new IntRectangle(8, 0, 8, 8), CustomColor.White);
                StringDrawer.DrawStringOutlined(StringDrawer.TinyUnicodeSoftFont, "  " + GlobalManager.Values.MainCharData.Resources.GetAmount(ResourceType.OreRed),
                    new IntVector2(2, 2 + 8 * 3), Color.White);

                Video.SpriteBatch.Draw(Drawer.TextureDictionary.GetValueOrDefault("OreYellow"), new IntRectangle(1, 1 + 8 * 4, 8, 8), new IntRectangle(8, 0, 8, 8), CustomColor.White);
                StringDrawer.DrawStringOutlined(StringDrawer.TinyUnicodeSoftFont, "  " + GlobalManager.Values.MainCharData.Resources.GetAmount(ResourceType.OreYellow),
                    new IntVector2(2, 2 + 8 * 4), Color.White);

                Video.SpriteBatch.Draw(Drawer.TextureDictionary.GetValueOrDefault("OreOrange"), new IntRectangle(1, 1 + 8 * 5, 8, 8), new IntRectangle(8, 0, 8, 8), CustomColor.White);
                StringDrawer.DrawStringOutlined(StringDrawer.TinyUnicodeSoftFont, "  " + GlobalManager.Values.MainCharData.Resources.GetAmount(ResourceType.OreOrange),
                    new IntVector2(2, 2 + 8 * 5), Color.White);

                Video.SpriteBatch.Draw(Drawer.TextureDictionary.GetValueOrDefault("OrePurple"), new IntRectangle(1, 1 + 8 * 6, 8, 8), new IntRectangle(8, 0, 8, 8), CustomColor.White);
                StringDrawer.DrawStringOutlined(StringDrawer.TinyUnicodeSoftFont, "  " + GlobalManager.Values.MainCharData.Resources.GetAmount(ResourceType.OrePurple),
                    new IntVector2(2, 2 + 8 * 6), Color.White);

                Video.SpriteBatch.Draw(Drawer.TextureDictionary.GetValueOrDefault("MissileIcons"), new IntRectangle(1, 1 + 8 * 7, 8, 8), new IntRectangle(0, 0, 8, 8), CustomColor.White);
                StringDrawer.DrawStringOutlined(StringDrawer.TinyUnicodeSoftFont, "  " + GlobalManager.Values.MainCharData.Resources.GetAmount(ResourceType.MissileAtomic),
                    new IntVector2(2, 2 + 8 * 7), Color.White);

                Video.SpriteBatch.Draw(Drawer.TextureDictionary.GetValueOrDefault("MissileIcons"), new IntRectangle(1, 1 + 8 * 8, 8, 8), new IntRectangle(8, 0, 8, 8), CustomColor.White);
                StringDrawer.DrawStringOutlined(StringDrawer.TinyUnicodeSoftFont, "  " + GlobalManager.Values.MainCharData.Resources.GetAmount(ResourceType.MissileHoming),
                    new IntVector2(2, 2 + 8 * 8), Color.White);

                Video.SpriteBatch.Draw(Drawer.TextureDictionary.GetValueOrDefault("MissileIcons"), new IntRectangle(1, 1 + 8 * 9, 8, 8), new IntRectangle(16, 0, 8, 8), CustomColor.White);
                StringDrawer.DrawStringOutlined(StringDrawer.TinyUnicodeSoftFont, "  " + GlobalManager.Values.MainCharData.Resources.GetAmount(ResourceType.MissileSpray),
                    new IntVector2(2, 2 + 8 * 9), Color.White);

                Video.SpriteBatch.Draw(Drawer.TextureDictionary.GetValueOrDefault("MissileIcons"), new IntRectangle(1, 1 + 8 * 10, 8, 8), new IntRectangle(24, 0, 8, 8), CustomColor.White);
                StringDrawer.DrawStringOutlined(StringDrawer.TinyUnicodeSoftFont, "  " + GlobalManager.Values.MainCharData.Resources.GetAmount(ResourceType.MissileDrill),
                    new IntVector2(2, 2 + 8 * 10), Color.White);

                Video.SpriteBatch.Draw(Drawer.TextureDictionary.GetValueOrDefault("MissileIcons"), new IntRectangle(1, 1 + 8 * 11, 8, 8), new IntRectangle(32, 0, 8, 8), CustomColor.White);
                StringDrawer.DrawStringOutlined(StringDrawer.TinyUnicodeSoftFont, "  " + GlobalManager.Values.MainCharData.Resources.GetAmount(ResourceType.MissileMine),
                    new IntVector2(2, 2 + 8 * 11), Color.White);

                StringDrawer.DrawStringOutlined(StringDrawer.TinyUnicodeSoftFont, "HP: " + EntityManager.PlayerEntity?.DamageTaker.CurrentHp.Amount,
                    new IntVector2(440, 2 + 8 * 0), Color.White);
                //Video.DrawStringWithOutline(Drawer.TinyUnicodeSoftFont, "Time: " + ((SpaceMinerMainLoop)GameLoopManager.GameMainLoop).Timer / 60,
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
            Drawer.DrawNineSliceTextureAt(MenuManager.CurrentMenuLayout.BackgroundImage,
                new IntRectangle((0, 0), MenuManager.CurrentMenuLayout.BackgroundImage.Bounds.Size),
                MenuManager.CurrentMenuLayout.BackgroundImagePosition,
                MenuManager.CurrentMenuLayout.BackgroundImageSize,
                MenuManager.CurrentMenuLayout.NineSliceBorder);
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

        if (DebugMode.IsOn)
        {
            foreach (var value in DebugInfoToPrint)
                DrawDebugLine(value.String, value.Object);

            foreach (var value in FrameDebugInfoToPrint)
                DrawDebugLine(value.String, value.Object);
        }
        FrameDebugInfoToPrint.Clear();

        //// Draw resource amounts
        //var repetitions = 0;
        //foreach (var resource in GlobalManager.Values.Resources.List)
        //{
        //    Video.DrawStringWithOutline(Drawer.TinyUnicodeFont, resource.ResourceType + ": " + resource.Amount,
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
            StringDrawer.DrawStringOutlined(StringDrawer.TinyUnicodeFont, $"{label}: {value}",
                debugTextPosition + new IntVector2(0, yOffset), Color.White);
            currentLine++;
        }
    }



    //private static void DrawStateChecker()
    //{
    //    var debugTextPosition = IntVector2.New(2, 2); //176);
    //    var entity = EntityManager.GetFilteredEntitiesFrom(EntityKind.Enemy).FirstOrDefault();

    //    var currentState = entity?.StateManager?.CurrentState?.Name;
    //    StringDrawer.DrawStringOutlined(StringDrawer.TinyUnicodeFont, "current state: " + currentState, debugTextPosition + (0, 0), Color.White);

    //    var currentCommandedState = entity?.StateManager?.CurrentCommandedState?.Name;
    //    StringDrawer.DrawStringOutlined(StringDrawer.TinyUnicodeFont, "current commanded state: " + currentCommandedState, debugTextPosition + (0, 8), Color.White);

    //    var stateQueue = entity?.StateManager?.CommandedStatesQueue;
    //    if (stateQueue != null)
    //    {
    //        var stateQueueString = stateQueue.Aggregate("state queue: ", (current, state) => current + state.Name + ", ");
    //        StringDrawer.DrawStringOutlined(StringDrawer.TinyUnicodeFont, stateQueueString, debugTextPosition + (0, 16), Color.White);
    //    }
    //}
}
