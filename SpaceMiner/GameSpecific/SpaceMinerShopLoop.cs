using Engine.Managers;
using Engine.Managers.GameModes;
using SpaceMiner.GameSpecific.Entities.MenuLayouts;

namespace SpaceMiner.GameSpecific;

public class SpaceMinerShopLoop : GameLoop
{
    public override void GameSpecificSetup()
    {
        MenuManager.CreateMenu(typeof(MenuLayoutShop));
    }

    public override void Update()
    {
    }
}