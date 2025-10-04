using Engine.ECS.Entities.EntityCreation;
using Engine.Managers.Graphics;
using Engine.Types;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Engine.ECS.Entities;

/// <summary>
/// Creates and handles entities lists. These entities don't exist in the game world, they are used for searching, comparisons, etc.
/// These lists are stored in their respective managers.
/// </summary>
public static class CollectionManager // TODO: rename to EntityListGenerator? (also change summary)
{
    private static Dictionary<Type, Entity> CollectionDictionary { get; } = new();

    static CollectionManager()
    {
        CollectionDictionary = FullEntityList().ToDictionary(entity => entity.GetType());
    }

    private static List<Entity> FullEntityList()
    {
        var engineAssembly = Assembly.GetExecutingAssembly();
        var sharedEntitiesNamespace = "Engine.ECS.Entities.Shared";
        var engineEntities = GetEntityList(engineAssembly, sharedEntitiesNamespace);

        var gameAssembly = Assembly.GetEntryAssembly();
        var @namespace = gameAssembly!.GetName().Name + ".GameSpecific.Entities";
        var gameEntities = GetEntityList(gameAssembly, @namespace);

        return engineEntities.Concat(gameEntities).ToList();
    }

    private static List<Entity> GetEntityList(Assembly assembly, string @namespace)
    {
        return assembly.GetTypes()
            .Where(type => type.IsSubclassOf(typeof(Entity)) &&
                           !type.IsAbstract &&
                           type.Namespace != null &&
                           (type.Namespace.StartsWith(@namespace)))
            .Select(EntityManager.CreateEntityForCollection)
            .ToList();
    }

    public static Entity GetEntityFromType(Type type) =>
        CollectionDictionary.GetValueOrDefault(type);

    public static bool Contains(Entity entity) =>
        CollectionDictionary.ContainsValue(entity);

    public static void DrawEntityPreview(Type entityType, IntVector2 previewPosition, Color color, int stretchSize = 0)
    {
        var sprite = GetEntityFromType(entityType).Sprite;
        var needsToStretch = stretchSize != 0 && (sprite.Size.X > stretchSize || sprite.Size.Y > stretchSize);

        if (!needsToStretch)
        {
            var position = previewPosition - sprite.Origin;
            Video.SpriteBatch.Draw(sprite.Texture, position, new IntRectangle(sprite.SpriteSheetOrigin, sprite.Size), color);
        }
        else
        {
            var biggerSize = Math.Max(sprite.Size.X, sprite.Size.Y);
            var smallerSize = Math.Min(sprite.Size.X, sprite.Size.Y);
            var scale = (float)stretchSize / biggerSize;
            var offsetValue = (int)((biggerSize - smallerSize) * scale / 2);
            var offsetCoordinate = sprite.Size.X > sprite.Size.Y ? IntVector2.New(0, offsetValue) : IntVector2.New(offsetValue, 0);
            var position = previewPosition - stretchSize / 2 + offsetCoordinate;
            Video.SpriteBatch.Draw(sprite.Texture, position, new IntRectangle(sprite.SpriteSheetOrigin, sprite.Size), color, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
        }
    }
}
