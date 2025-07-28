using Engine.ECS.Components;
using Engine.ECS.Entities.EntityCreation;
using Engine.Managers;
using Engine.Types;

namespace Engine.ECS.Systems.Physics.GeneralPhysics;

public class EntityCollisionChecking : Component
{
    public EntityCollisionChecking(Entity owner)
    {
        Owner = owner;
    }

    public bool CollidesWithEntityAtPixel(Entity entity, IntVector2 position)
    {
        var collisionBox1 = Owner.CollisionBox.GetCollisionRectangleAt(position);
        var collisionBox2 = entity.CollisionBox.GetCollisionRectangle();

        DebugMode.EntityCollisionCounter++;
        return collisionBox1.Overlaps(collisionBox2);
    }
}
