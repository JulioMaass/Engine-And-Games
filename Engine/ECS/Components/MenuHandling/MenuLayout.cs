using Engine.Types;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Engine.ECS.Components.MenuHandling;

public class MenuLayout
{
    public List<MenuArea> MenuAreas { get; } = new();
    public List<Type> SwappableAreaTypes { get; } = new();
    public Texture2D BackgroundImage { get; protected init; }
    public IntVector2 BackgroundImagePosition { get; protected init; }
    public IntVector2 BackgroundImageSize { get; protected init; }
    public int StartingCursorItem { get; protected init; }
}