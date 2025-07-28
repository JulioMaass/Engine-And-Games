using Engine.ECS.Entities.EntityCreation;
using Engine.Types;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Engine.ECS.Components.MenuHandling;

public class MenuLayout
{
    public List<MenuArea> MenuAreas { get; } = new();
    public Texture2D BackgroundImage { get; protected init; }
    public IntVector2 BackgroundImagePosition { get; protected init; }
    public IntVector2 BackgroundImageSize { get; protected init; }
    public int StartingCursorItem { get; protected init; }

    public class MenuArea
    {
        // Navigation
        public IntVector2 Position { get; }
        public Type[,] MenuItemTypes { get; }
        public Entity[,] MenuItemEntities { get; }
        public IntVector2 Spacing { get; }
        public List<MenuArea> AllowedAreasUp { get; } = new();
        public List<MenuArea> AllowedAreasDown { get; } = new();
        public List<MenuArea> AllowedAreasLeft { get; } = new();
        public List<MenuArea> AllowedAreasRight { get; } = new();

        public MenuArea(Type[,] menuItemTypes, IntVector2 position, IntVector2 spacing)
        {
            MenuItemTypes = menuItemTypes;
            Position = position;
            Spacing = spacing;

            MenuItemEntities = new Entity[MenuItemTypes.GetLength(0), MenuItemTypes.GetLength(1)];
        }
    }
}