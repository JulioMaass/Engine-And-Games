using Engine.ECS.Components.CombatHandling;
using Engine.ECS.Entities;
using Engine.ECS.Entities.EntityCreation;
using Engine.Helpers;
using Engine.Main;
using Engine.Managers.Graphics;
using Engine.Types;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Managers.CollisionSystem;

public class SpatialGrid
{
    private IntVector2 CellSize { get; }
    private IntVector2 Size { get; }
    private List<Entity>[,] Grid { get; }
    private IntVector2 PositionInTiles { get; set; }

    // Entities properties
    public AlignmentType AlignmentType { get; }
    public HittingRole HittingRole { get; }

    public SpatialGrid(AlignmentType alignmentType, HittingRole hittingRole)
    {
        Size = Settings.RoomSizeInTiles + 3;
        CellSize = Settings.TileSize;
        Grid = new List<Entity>[Size.Width, Size.Height];
        AlignmentType = alignmentType;
        HittingRole = hittingRole;

        for (var x = 0; x < Size.Width; x++)
            for (var y = 0; y < Size.Height; y++)
                Grid[x, y] = new List<Entity>();
    }

    public void Update(IntVector2 position)
    {
        PositionInTiles = position;
        Clear();
        foreach (var entity in EntityManager.GetEntities(AlignmentType, HittingRole))
            AddEntity(entity);
    }

    public void Clear()
    {
        for (var x = 0; x < Size.Width; x++)
            for (var y = 0; y < Size.Height; y++)
                Grid[x, y].Clear();
    }

    public void AddEntity(Entity entity)
    {
        if (entity.CollisionBox == null)
            return;
        var (minX, maxX, minY, maxY) = GetEntityGridRectangle(entity);

        // Add entity to all occupied cells
        for (var x = minX; x <= maxX; x++)
            for (var y = minY; y <= maxY; y++)
                Grid[x, y].Add(entity);
    }

    public (int minX, int maxX, int minY, int maxY) GetEntityGridRectangle(Entity entity)
    {
        var position = entity.Position.Pixel;
        var collisionBox = entity.CollisionBox;

        // Calculate which grid cells the entity occupies
        var minX = (position.X - collisionBox.MaskLeft) / CellSize.X;
        var maxX = (position.X + collisionBox.MaskRight - 1) / CellSize.X;
        var minY = (position.Y - collisionBox.MaskTop) / CellSize.Y;
        var maxY = (position.Y + collisionBox.MaskBottom - 1) / CellSize.Y;

        // Position relative to grid
        minX -= PositionInTiles.X;
        maxX -= PositionInTiles.X;
        minY -= PositionInTiles.Y;
        maxY -= PositionInTiles.Y;

        // Clamp inside grid area
        minX = MathHelper.Clamp(minX, 0, Size.Width - 1);
        maxX = MathHelper.Clamp(maxX, 0, Size.Width - 1);
        minY = MathHelper.Clamp(minY, 0, Size.Height - 1);
        maxY = MathHelper.Clamp(maxY, 0, Size.Height - 1);

        return (minX, maxX, minY, maxY);
    }

    public void Draw()
    {
        for (var x = 0; x < Size.Width; x++)
            for (var y = 0; y < Size.Height; y++)
                if (Grid[x, y].Count > 0)
                {
                    var position = new IntVector2(x * Settings.TileSize.X, y * Settings.TileSize.Y);
                    position += PositionInTiles * Settings.TileSize;
                    Drawer.DrawRectangleOutline(position, Settings.TileSize, CustomColor.Yellow);
                }
    }

    public List<Entity> GetOverlappingEntities(Entity entity)
    {
        var overlappingEntitiesList = new HashSet<Entity>();
        var (minX, maxX, minY, maxY) = GetEntityGridRectangle(entity);

        for (var x = minX; x <= maxX; x++)
            for (var y = minY; y <= maxY; y++)
                foreach (var overlappingEntity in Grid[x, y])
                    overlappingEntitiesList.Add(overlappingEntity);

        return overlappingEntitiesList.ToList();
    }
}

public enum HittingRole
{
    Hitter,
    Hittable
}