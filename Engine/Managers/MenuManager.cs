using Engine.ECS.Components.MenuHandling;
using Engine.ECS.Entities;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;
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
    public static Entity SelectedItem { get; private set; }
    // Navigation untying
    private static IntVector2 LastIntentionalPosition { get; set; }
    private static IntVector2 LastMoveDirection { get; set; }

    public static void CreateMenu(Type menuType)
    {
        if (menuType == null)
            return;

        LayoutDirectoryList.Clear();
        LayoutDirectoryList.Add(menuType);
        UpdateAvailableMenuItems();
        SetSelectedItem(CurrentMenuLayout.StartingCursorItem);
    }

    public static void Clear()
    {
        ClearMenuItems();
        LayoutDirectoryList.Clear();
        CurrentMenuLayout = null;
        SetSelectedItem(null);
    }

    public static void UpdateAvailableMenuItems()
    {
        ClearMenuItems();
        AvailableMenuItems = new EntityList();
        CurrentMenuLayout = (MenuLayout)Activator.CreateInstance(LayoutDirectoryList.Last());
        if (CurrentMenuLayout == null)
            return;

        foreach (var menuArea in CurrentMenuLayout.MenuAreas)
            menuArea.GenerateMenuItems();
    }

    public static void ClearMenuItems()
    {
        if (AvailableMenuItems == null) return;
        foreach (var item in AvailableMenuItems)
            EntityManager.DeleteEntity(item);
        AvailableMenuItems = null;
    }

    private static void SetSelectedItem(Entity item, IntVector2 dir) // Updates both LastIntentionalPosition and LastMoveDirection
    {
        if (dir.X != 0)
            LastIntentionalPosition = new IntVector2(item.Position.Pixel.X, LastIntentionalPosition.Y);
        else if (dir.Y != 0)
            LastIntentionalPosition = new IntVector2(LastIntentionalPosition.X, item.Position.Pixel.Y);
        SetSelectedItem(item, true);
    }

    public static void SetSelectedItem(Entity item, bool trackMoveDirection) // Updates LastMoveDirection
    {
        SelectedItem = item;
        if (trackMoveDirection)
        {
            LastMoveDirection = new IntVector2(
                Math.Sign(item.Position.Pixel.X - SelectedItem.Position.Pixel.X),
                Math.Sign(item.Position.Pixel.Y - SelectedItem.Position.Pixel.Y));
            return;
        }
        LastIntentionalPosition = item.Position.Pixel;
        LastMoveDirection = IntVector2.Zero;
    }

    public static void SetSelectedItem(Type itemType)
    {
        if (AvailableMenuItems == null)
        {
            ResetSelectedItem();
            return;
        }
        var item = AvailableMenuItems.FirstOrDefault(i => i.GetType() == itemType)
            ?? AvailableMenuItems.FirstOrDefault();
        SetSelectedItem(item, false);
    }

    public static void ResetSelectedItem()
    {
        SelectedItem = null;
        LastIntentionalPosition = IntVector2.Zero;
        LastMoveDirection = IntVector2.Zero;
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
        DeleteMenuArea(swapOutType);
        GenerateMenuArea(swapInType);
    }

    private static void DeleteMenuArea(Type areaType)
    {
        var menuArea = CurrentMenuLayout.MenuAreas.FirstOrDefault(area => area.GetType() == areaType);
        menuArea!.DeleteMenuItems();
        CurrentMenuLayout.MenuAreas.Remove(menuArea);
    }

    private static void GenerateMenuArea(Type areaType)
    {
        var menuArea = (MenuArea)Activator.CreateInstance(areaType);
        menuArea!.GenerateMenuItems();
        CurrentMenuLayout.MenuAreas.Add(menuArea);
    }

    public static void Update()
    {
        // Movement
        if (Input.Up.Pressed)
            MoveSelection((0, -1));
        else if (Input.Down.Pressed)
            MoveSelection((0, 1));
        else if (Input.Left.Pressed)
            MoveSelection((-1, 0));
        else if (Input.Right.Pressed)
            MoveSelection((1, 0));

        // Select and Cancel
        if (Input.Button1.Pressed)
            SelectedItem.MenuItem.OnSelect?.Invoke();
        else if (Input.Button2.Pressed)
            GoBackOneMenu();
    }

    private static void GoBackOneMenu()
    {
        if (LayoutDirectoryList.Count <= 1) return;

        var previousMenuItem = LayoutDirectoryList[^1]; // Get the last item in the list
        LayoutDirectoryList.RemoveAt(LayoutDirectoryList.Count - 1);
        UpdateAvailableMenuItems();
        SetSelectedItem(previousMenuItem);
    }

    private static void MoveSelection(IntVector2 dir)
    {
        var itemsInDirection = GetItemsInDirection(dir);

        // Find items in the same area
        var itemsInSameArea = AvailableMenuItems
            .Where(item => item.MenuItem.OwningMenuArea == SelectedItem.MenuItem.OwningMenuArea)
            .ToEntityList();
        var itemsToGoTo = itemsInSameArea.Intersect(itemsInDirection).ToEntityList();

        // If there are no items in the same area, find other areas
        if (itemsToGoTo.Count == 0)
        {
            var areasInDirection = GetNearestAreasInDirection(dir);
            itemsToGoTo = AvailableMenuItems.Where(item => areasInDirection.Contains(item.MenuItem.OwningMenuArea)).ToEntityList();
        }

        // Navigate to item
        var nearestItems = GetNearestItemsInDirection(itemsToGoTo, dir);
        var newSelectedItem = GetItemNearestToPosition(nearestItems, LastIntentionalPosition);
        if (newSelectedItem == null)
            return;
        SetSelectedItem(newSelectedItem, dir);
    }

    private static EntityList GetItemsInDirection(IntVector2 dir)
    {
        var itemsXDirection = AvailableMenuItems.Where(item => GetDirectionToItem(item).X == dir.X).ToEntityList();
        var itemsYDirection = AvailableMenuItems.Where(item => GetDirectionToItem(item).Y == dir.Y).ToEntityList();
        return dir.X != 0 ? itemsXDirection : itemsYDirection;
    }

    private static IntVector2 GetDirectionToItem(Entity item)
    {
        var xDir = Math.Sign(item.Position.Pixel.X - SelectedItem.Position.Pixel.X);
        var yDir = Math.Sign(item.Position.Pixel.Y - SelectedItem.Position.Pixel.Y);
        return (xDir, yDir);
    }

    private static List<MenuArea> GetNearestAreasInDirection(IntVector2 dir)
    {
        var areasInDirection = GetAreasInDirection(dir);
        if (areasInDirection.Count == 0)
            return areasInDirection;

        var currentArea = SelectedItem.MenuItem.OwningMenuArea;
        Func<MenuArea, int> distanceCalculationFunction = dir.X != 0
            ? area => Math.Abs(area.Position.X - currentArea.Position.X)
            : area => Math.Abs(area.Position.Y - currentArea.Position.Y);

        return GetNearestList(areasInDirection, distanceCalculationFunction);
    }

    private static List<MenuArea> GetAreasInDirection(IntVector2 dir)
    {
        var areasInDirection = new List<MenuArea>();
        var currentArea = SelectedItem.MenuItem.OwningMenuArea;
        foreach (var area in CurrentMenuLayout.MenuAreas.Except([currentArea]))
        {
            var canMoveRight = dir.X == 1 && currentArea.AreaRectangle.IsLeftOfAndAligned(area.AreaRectangle);
            var canMoveLeft = dir.X == -1 && currentArea.AreaRectangle.IsRightOfAndAligned(area.AreaRectangle);
            var canMoveDown = dir.Y == 1 && currentArea.AreaRectangle.IsAboveAndAligned(area.AreaRectangle);
            var canMoveUp = dir.Y == -1 && currentArea.AreaRectangle.IsBelowAndAligned(area.AreaRectangle);
            if (canMoveRight || canMoveLeft || canMoveDown || canMoveUp)
                areasInDirection.Add(area);
        }
        return areasInDirection;
    }

    private static EntityList GetNearestItemsInDirection(EntityList itemList, IntVector2 dir)
    {
        Func<Entity, int> distanceCalculationFunction = dir.X != 0
            ? item => Math.Abs(item.Position.Pixel.X - SelectedItem.Position.Pixel.X)
            : item => Math.Abs(item.Position.Pixel.Y - SelectedItem.Position.Pixel.Y);

        return GetNearestList(itemList, distanceCalculationFunction).ToEntityList();
    }

    private static Entity GetItemNearestToPosition(EntityList itemList, IntVector2 position)
    {
        var nearestItems = GetNearestList(itemList, item =>
            Math.Abs(item.Position.Pixel.X - position.X) +
            Math.Abs(item.Position.Pixel.Y - position.Y)).ToEntityList();
        if (nearestItems.Count == 0)
            return null;
        if (nearestItems.Count == 1)
            return nearestItems[0];
        // Reverse NextMoveDir to untie
        return GetNearestItemsInDirection(nearestItems, -LastMoveDirection)[0];
    }

    private static List<T> GetNearestList<T>(IEnumerable<T> list, Func<T, int> distanceCalculationFunction)
    {
        var listOfNearest = new List<T>();
        var nearestDistance = int.MaxValue;

        foreach (var item in list)
        {
            var distance = distanceCalculationFunction(item);
            if (distance < nearestDistance) // Find nearest item and distance
            {
                nearestDistance = distance;
                listOfNearest.Clear();
                listOfNearest.Add(item);
            }
            else if (distance == nearestDistance) // Accumulate tied items
                listOfNearest.Add(item);
        }
        return listOfNearest;
    }
}
