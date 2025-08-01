using Engine.ECS.Entities;
using Engine.ECS.Entities.EntityCreation;
using Engine.Managers;
using Engine.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.ECS.Components.MenuHandling;

public class MenuArea
{
    // Navigation
    public Type[,] MenuItemTypes { get; protected init; }
    private Entity[,] MenuItemEntities { get; set; }
    public IntVector2 Position { get; protected init; }
    public IntVector2 Spacing { get; protected init; }
    public List<MenuArea> AllowedAreasUp { get; } = new();
    public List<MenuArea> AllowedAreasDown { get; } = new();
    public List<MenuArea> AllowedAreasLeft { get; } = new();
    public List<MenuArea> AllowedAreasRight { get; } = new();

    protected MenuArea() { }

    public MenuArea(Type[,] menuItemTypes, IntVector2 position, IntVector2 spacing)
    {
        MenuItemTypes = menuItemTypes;
        Position = position;
        Spacing = spacing;

        MenuItemEntities = new Entity[MenuItemTypes.GetLength(0), MenuItemTypes.GetLength(1)];
    }

    public Entity[,] GetMenuItemEntities()
    {
        MenuItemEntities ??= new Entity[MenuItemTypes.GetLength(0), MenuItemTypes.GetLength(1)];
        return MenuItemEntities;
    }

    public void GenerateMenuItems()
    {
        for (var x = 0; x < MenuItemTypes.GetLength(0); x++)
        {
            for (var y = 0; y < MenuItemTypes.GetLength(1); y++)
            {
                var menuItemType = MenuItemTypes[x, y];
                if (menuItemType == null) continue;
                var menuItemPosition = Position + IntVector2.New(x * Spacing.X, y * Spacing.Y);
                var menuItemEntity = EntityManager.CreateEntityAt(menuItemType, menuItemPosition);
                menuItemEntity.MenuItem.OwningMenuArea = this;
                MenuManager.AvailableMenuItems.Add(menuItemEntity);
                GetMenuItemEntities()[x, y] = menuItemEntity;
            }
        }
    }

    public void DeleteMenuItems()
    {
        foreach (var menuItemEntity in MenuManager.AvailableMenuItems.ToList()
            .Where(menuItemEntity => menuItemEntity.MenuItem.OwningMenuArea == this))
        {
            MenuManager.AvailableMenuItems.Remove(menuItemEntity);
            EntityManager.DeleteEntity(menuItemEntity);
        }
    }
}