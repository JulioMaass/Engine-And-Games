using Engine.ECS.Components.MenuHandling;
using Engine.ECS.Entities;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Managers;

public static class MenuManager
{
    public static bool IsActive => AvailableMenuItems != null;
    public static EntityList AvailableMenuItems { get; private set; }
    public static List<Type> LayoutDirectoryList { get; } = new();
    public static MenuLayout CurrentMenuLayout { get; private set; }
    public static Entity SelectedItem { get; set; }
    private static int LastIntentionalX { get; set; } // Only changes when moving horizontally (resets when going into/out of sub-menus)
    private static int LastIntentionalY { get; set; } // Only changes when moving vertically

    public static void CreateMenu(Type menuType)
    {
        LayoutDirectoryList.Clear();
        LayoutDirectoryList.Add(menuType);
        UpdateAvailableMenuItems();
        SelectedItem = AvailableMenuItems[CurrentMenuLayout.StartingCursorItem];
    }

    public static void Clear()
    {
        ClearMenuItems();
        LayoutDirectoryList.Clear();
        CurrentMenuLayout = null;
        SelectedItem = null;
        LastIntentionalX = 0;
        LastIntentionalY = 0;
    }

    public static void UpdateAvailableMenuItems()
    {
        ClearMenuItems();
        AvailableMenuItems = new EntityList();
        CurrentMenuLayout = (MenuLayout)Activator.CreateInstance(LayoutDirectoryList.Last());
        if (CurrentMenuLayout == null) return;

        // Create each menu area
        foreach (var menuArea in CurrentMenuLayout.MenuAreas)
        {
            // Create each menu item in the area grid
            for (var x = 0; x < menuArea.MenuItemTypes.GetLength(0); x++)
            {
                for (var y = 0; y < menuArea.MenuItemTypes.GetLength(1); y++)
                {
                    var menuItemType = menuArea.MenuItemTypes[x, y];
                    if (menuItemType == null) continue;
                    var menuItemPosition = menuArea.Position + IntVector2.New(x * menuArea.Spacing.X, y * menuArea.Spacing.Y);
                    var menuItemEntity = EntityManager.CreateEntityAt(menuItemType, menuItemPosition);
                    menuItemEntity.MenuItem.OwningMenuArea = menuArea;
                    AvailableMenuItems.Add(menuItemEntity);
                    menuArea.MenuItemEntities[x, y] = menuItemEntity;
                }
            }
        }
    }

    private static void ClearMenuItems()
    {
        if (AvailableMenuItems == null) return;
        foreach (var item in AvailableMenuItems)
            EntityManager.DeleteEntity(item);
        AvailableMenuItems = null;
    }

    public static void Update()
    {
        // Movement
        if (Input.Up.Pressed)
            MoveSelection(-1, 0);
        else if (Input.Down.Pressed)
            MoveSelection(1, 0);
        else if (Input.Left.Pressed)
            MoveSelection(0, -1);
        else if (Input.Right.Pressed)
            MoveSelection(0, 1);

        // Select and Cancel
        if (Input.Button1.Pressed)
            SelectedItem.MenuItem.OnSelect?.Invoke();
        else if (Input.Button2.Pressed)
            GoBackOneMenu();
    }

    private static void GoBackOneMenu()
    {
        if (LayoutDirectoryList.Count <= 1) return;

        var previousMenu = LayoutDirectoryList[^1]; // Get the last item in the list
        LayoutDirectoryList.RemoveAt(LayoutDirectoryList.Count - 1);
        UpdateAvailableMenuItems();
        SelectedItem = AvailableMenuItems.FirstOrDefault(item => item.MenuItem.ContainedMenuLayoutType == previousMenu);
        SetSelectedItemAsIntentionalPosition();
    }

    public static void SetSelectedItemAsIntentionalPosition()
    {
        LastIntentionalX = SelectedItem.Position.Pixel.X;
        LastIntentionalY = SelectedItem.Position.Pixel.Y;
    }

    private static void MoveSelection(int yDir, int xDir)
    {
        var allowedDirectionAreas = new List<MenuLayout.MenuArea>();

        // Add the area that the selected item belongs to
        var currentArea = SelectedItem.MenuItem.OwningMenuArea;
        allowedDirectionAreas.Add(currentArea);

        // Get the areas that are allowed to be navigated to in the direction of the movement
        if (yDir < 0)
            allowedDirectionAreas.AddRange(SelectedItem.MenuItem.OwningMenuArea.AllowedAreasUp);
        else if (yDir > 0)
            allowedDirectionAreas.AddRange(SelectedItem.MenuItem.OwningMenuArea.AllowedAreasDown);
        else if (xDir < 0)
            allowedDirectionAreas.AddRange(SelectedItem.MenuItem.OwningMenuArea.AllowedAreasLeft);
        else if (xDir > 0)
            allowedDirectionAreas.AddRange(SelectedItem.MenuItem.OwningMenuArea.AllowedAreasRight);

        // Get the items that are in the allowed areas
        var allowedItems = AvailableMenuItems.Where(item => allowedDirectionAreas.Contains(item.MenuItem.OwningMenuArea)).ToList();

        // Remove items in the opposite direction of the movement
        var itemsInDirection = allowedItems
        .Where(item => (yDir < 0 && item.Position.Pixel.Y < SelectedItem.Position.Pixel.Y) ||
                          (yDir > 0 && item.Position.Pixel.Y > SelectedItem.Position.Pixel.Y) ||
                          (xDir < 0 && item.Position.Pixel.X < SelectedItem.Position.Pixel.X) ||
                          (xDir > 0 && item.Position.Pixel.X > SelectedItem.Position.Pixel.X))
                            .ToEntityList();

        // Group the filtered items by Y position if moving vertically, or X position if moving horizontally
        var groupedItems = itemsInDirection.GroupBy(item => yDir != 0 ? item.Position.Pixel.Y : item.Position.Pixel.X).ToList();

        // Get the group with the nearest x (if moving horizontally) or y (if moving vertically)
        var nearestGroup = xDir > 0 || yDir > 0
            ? groupedItems.MinBy(group => group.Key)
            : groupedItems.MaxBy(group => group.Key);

        // Get the item with the nearest Intentional coordinate in the perpendicular axis
        var selectedItem = xDir != 0
            ? nearestGroup?.MinBy(item => Math.Abs(item.Position.Pixel.Y - LastIntentionalY))
            : nearestGroup?.MinBy(item => Math.Abs(item.Position.Pixel.X - LastIntentionalX));

        // Set the selected item
        if (selectedItem == null)
            return;
        SelectedItem = selectedItem;

        // Update the last intentional coordinate
        if (yDir != 0)
            LastIntentionalY = SelectedItem.Position.Pixel.Y;
        if (xDir != 0)
            LastIntentionalX = SelectedItem.Position.Pixel.X;
    }
}
