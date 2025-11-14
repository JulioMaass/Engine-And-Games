using Engine.ECS.Entities.EntityCreation;
using Engine.Managers;
using Engine.Managers.GameModes;
using Engine.Managers.StageHandling;

namespace SpaceMiner.GameSpecific.Entities.Menus.StageSelectLayout;

public class MenuItemStageBlue : Entity
{
    public MenuItemStageBlue()
    {
        EntityKind = EntityKind.Menu;
        AddBasicComponents();
        MenuItem = new(this);

        // Basic, Sprite, EntityKind
        AddSpriteCenteredOrigin("StageBlue", 24);
        Sprite.HudSprite = true;
        AddFrameSprite("MenuSocketNineSlice", 2, 2);

        //MenuItem.Label = "Atomic";
        AddSpaceMinerStageSelectItemComponents();

        MenuItem.OnSelect = () =>
        {
            MenuManager.Clear();
            StageManager.CurrentStageName = "defaultStageSpaceMiner";
            GameLoopManager.ResetGameLoop();
        };
    }
}