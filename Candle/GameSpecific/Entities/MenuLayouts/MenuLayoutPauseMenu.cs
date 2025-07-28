using Candle.GameSpecific.Entities.Items;
using Engine.ECS.Components.MenuHandling;
using Engine.Helpers;
using Engine.Main;
using Engine.Managers.Graphics;
using Engine.Types;

namespace Candle.GameSpecific.Entities.MenuLayouts;

public class MenuLayoutPauseMenu : MenuLayout
{
    public MenuLayoutPauseMenu()
    {
        BackgroundImage = Drawer.TextureDictionary["BlackTile"];
        BackgroundImageSize = Settings.RoomSizeInPixels;

        var array = Extensions.NewTransposedArray(new[,]
        {
            { typeof(BigSlashItem), typeof(LanceItem), typeof(ArrowItem), },
            { typeof(ArmorItem), typeof(LeechItem), typeof(SlowburnItem), },
            { typeof(DashItem), typeof(DashItem), typeof(DoubleJumpItem), },
        });
        var position = IntVector2.New(48, 32);
        var spacing = IntVector2.New(64, 32);
        var menuArea = new MenuArea(array, position, spacing);
        MenuAreas.Add(menuArea);
    }
}