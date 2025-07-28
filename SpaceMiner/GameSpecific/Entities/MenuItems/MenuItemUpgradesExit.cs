using Engine.ECS.Entities.EntityCreation;
using Engine.Helpers;
using Engine.Managers;
using Engine.Managers.GameModes;
using Engine.Managers.Graphics;

namespace SpaceMiner.GameSpecific.Entities.MenuItems;

public class MenuItemUpgradesExit : Entity
{
    public MenuItemUpgradesExit()
    {
        EntityKind = EntityKind.Menu;
        AddBasicComponents();
        MenuItem = new(this);

        MenuItem.Label = "Exit";

        MenuItem.Draw = () =>
        {
            Video.SpriteBatch.DrawString(Drawer.MegaManFont, MenuItem.Label, Position.Pixel + (0, 10), CustomColor.White);
        };

        MenuItem.OnSelectDraw = () =>
        {
            var cursorPosition = MenuManager.SelectedItem.Position.Pixel;
            Video.SpriteBatch.DrawString(Drawer.MegaManFont, ">", cursorPosition + (-10, 10), CustomColor.White);
        };

        MenuItem.OnSelect = () =>
        {
            // Continue game
            MenuManager.Clear();
            GameLoopManager.SetGameLoop(GameLoopManager.GameMainLoop);
        };
    }
}