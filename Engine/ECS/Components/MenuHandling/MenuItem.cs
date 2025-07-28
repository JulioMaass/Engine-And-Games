using Engine.ECS.Entities.EntityCreation;
using Engine.Managers;
using System;
using static Engine.ECS.Components.MenuHandling.MenuLayout;

namespace Engine.ECS.Components.MenuHandling;

public class MenuItem : Component
{
    // Visuals
    public string Label { get; set; }
    public Action Draw { get; set; }
    public Action OnSelectDraw { get; set; }
    // Functionality
    public Action OnSelect { get; set; }
    public Type ContainedMenuLayoutType { get; set; }
    // Navigation
    public MenuArea OwningMenuArea { get; set; }

    public MenuItem(Entity owner)
    {
        Owner = owner;
    }

    public void OpenContainedMenuLayout()
    {
        MenuManager.LayoutDirectoryList.Add(ContainedMenuLayoutType);
        MenuManager.UpdateAvailableMenuItems();
        MenuManager.SelectedItem = MenuManager.AvailableMenuItems[MenuManager.CurrentMenuLayout.StartingCursorItem];
        MenuManager.SetSelectedItemAsIntentionalPosition();
    }
}