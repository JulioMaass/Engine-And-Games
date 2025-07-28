using Engine.ECS.Entities.EntityCreation;
using System.Collections.Generic;

namespace Engine.ECS.Entities;

public class EntityCollection : List<Entity>
{
    // This class is just to mindfully avoid using List<Entity> directly, when it's a collection of entities.
}
