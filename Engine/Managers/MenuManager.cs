using Engine.ECS.Components.MenuHandling;
using Engine.ECS.Entities;
using Engine.ECS.Entities.EntityCreation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        foreach (var menuArea in CurrentMenuLayout.MenuAreas)
            menuArea.GenerateMenuItems();
    }

    public static void CheckToSwapMenuArea(Type newMenuAreaType)
    {
        var currentArea = CurrentMenuLayout.MenuAreas.FirstOrDefault(area => CurrentMenuLayout.SwappableAreaTypes.Contains(area.GetType()));
        if (currentArea == null)
            Debugger.Break(); // Current area should be one of the target types
        if (currentArea!.GetType() == newMenuAreaType)
            return;
        SwapMenuArea(currentArea.GetType(), newMenuAreaType);
    }

    private static void SwapMenuArea(Type swapOutType, Type swapInType)
    {
        var swapOutMenuArea = DeleteMenuArea(swapOutType);
        var swapInMenuArea = GenerateMenuArea(swapInType);

        foreach (var menuArea in CurrentMenuLayout.MenuAreas)
        {
            SwapAreaReferences(menuArea.AllowedAreasUp, swapOutMenuArea, swapInMenuArea);
            SwapAreaReferences(menuArea.AllowedAreasDown, swapOutMenuArea, swapInMenuArea);
            SwapAreaReferences(menuArea.AllowedAreasLeft, swapOutMenuArea, swapInMenuArea);
            SwapAreaReferences(menuArea.AllowedAreasRight, swapOutMenuArea, swapInMenuArea);
        }

        swapInMenuArea.AllowedAreasUp.AddRange(swapOutMenuArea.AllowedAreasUp);
        swapInMenuArea.AllowedAreasDown.AddRange(swapOutMenuArea.AllowedAreasDown);
        swapInMenuArea.AllowedAreasLeft.AddRange(swapOutMenuArea.AllowedAreasLeft);
        swapInMenuArea.AllowedAreasRight.AddRange(swapOutMenuArea.AllowedAreasRight);
    }

    private static MenuArea DeleteMenuArea(Type areaType)
    {
        var menuArea = CurrentMenuLayout.MenuAreas.FirstOrDefault(area => area.GetType() == areaType);
        menuArea!.DeleteMenuItems();
        CurrentMenuLayout.MenuAreas.Remove(menuArea);
        return menuArea;
    }

    private static MenuArea GenerateMenuArea(Type areaType)
    {
        var menuArea = (MenuArea)Activator.CreateInstance(areaType);
        menuArea!.GenerateMenuItems();
        CurrentMenuLayout.MenuAreas.Add(menuArea);
        return menuArea;
    }

    private static void SwapAreaReferences(List<MenuArea> areasInDirection, MenuArea swapOutMenuArea, MenuArea swapInMenuArea)
    {
        if (areasInDirection.Contains(swapOutMenuArea))
        {
            areasInDirection.Remove(swapOutMenuArea);
            areasInDirection.Add(swapInMenuArea);
        }
    }

    public static void ClearMenuItems()
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
        var allowedDirectionAreas = new List<MenuArea>();

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
