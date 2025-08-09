using Engine.ECS.Entities;
using Engine.ECS.Entities.EntityCreation;
using Engine.Managers;
using Engine.Types;
using System;
using System.Linq;

namespace Engine.ECS.Components.MenuHandling;

public class MenuArea
{
    // Navigation
    public Type[,] MenuItemTypes { get; protected init; }
    private Entity[,] MenuItemEntities { get; set; }
    public IntVector2 Position { get; protected init; }
    public IntVector2 Spacing { get; protected init; }
    public IntRectangle AreaRectangle => GetMenuAreaRectangle();

    protected MenuArea() { }

    public MenuArea(Type[,] menuItemTypes, IntVector2 position, IntVector2 spacing)
    {
        MenuItemTypes = menuItemTypes;
        Position = position;
        Spacing = spacing;

        MenuItemEntities = new Entity[MenuItemTypes.GetLength(0), MenuItemTypes.GetLength(1)];
    }

    public void GenerateMenuItems()
    {
        MenuItemEntities ??= new Entity[MenuItemTypes.GetLength(0), MenuItemTypes.GetLength(1)];
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
                MenuItemEntities[x, y] = menuItemEntity;
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

    private IntRectangle GetMenuAreaRectangle() =>
        new(Position.X, Position.Y, MenuItemTypes.GetLength(0) * Spacing.X, MenuItemTypes.GetLength(1) * Spacing.Y);
}