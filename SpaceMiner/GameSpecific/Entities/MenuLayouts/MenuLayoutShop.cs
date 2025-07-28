using Engine.ECS.Components.MenuHandling;
using Engine.Helpers;
using Engine.Managers.Graphics;
using Engine.Types;
using SpaceMiner.GameSpecific.Entities.MenuItems;

namespace SpaceMiner.GameSpecific.Entities.MenuLayouts;

public class MenuLayoutShop : MenuLayout
{
    public MenuLayoutShop()
    {
        BackgroundImage = Drawer.TextureDictionary["BlackTile"];
        BackgroundImageSize = new IntVector2(256 + 64, 128 + 64);
        BackgroundImagePosition = new IntVector2(64, 32);

        var upgradesGrid = Extensions.NewTransposedArray(new[,]
        {
            { typeof(MenuItemMachineGun), typeof(MenuItemShotgun), typeof(MenuItemSlugger), typeof(MenuItemBlaster) },
            { typeof(MenuItemSocketBlue), typeof(MenuItemSocketYellow), typeof(MenuItemSocketGreen), typeof(MenuItemSocketRed) },
        });
        var upgradesPosition = IntVector2.New(64, 64);
        var upgradesSpacing = IntVector2.New(64 + 32, 32 + 32);
        var upgradesMenuArea = new MenuArea(upgradesGrid, upgradesPosition, upgradesSpacing);
        MenuAreas.Add(upgradesMenuArea);

        var optionsGrid = Extensions.NewTransposedArray(new[,]
        {
            { typeof(MenuItemUpgradesExit) },
        });
        var optionsPosition = IntVector2.New(256 - 64, 128 + 64);
        var optionsSpacing = IntVector2.New(64, 32);
        var optionsMenuArea = new MenuArea(optionsGrid, optionsPosition, optionsSpacing);
        MenuAreas.Add(optionsMenuArea);

        upgradesMenuArea.AllowedAreasDown.Add(optionsMenuArea);
        optionsMenuArea.AllowedAreasUp.Add(upgradesMenuArea);
    }
}