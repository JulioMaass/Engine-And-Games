using Engine.ECS.Entities;
using Engine.ECS.Entities.EntityCreation;
using Engine.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.ECS.Components.ItemsHandling;

public class ItemDropper : Component
{
    public List<(Type Type, int Weight)> DropTable { get; } = new();

    public ItemDropper(Entity owner)
    {
        Owner = owner;
    }

    public void DropItem()
    {
        var totalWeight = DropTable.Sum(item => item.Weight);
        var itemRollNumber = GetRandom.UnseededInt(totalWeight);
        Type itemType = null;

        // Scroll through item table until roll number is reached, subtracting weight of each scrolled item
        foreach (var item in DropTable)
        {
            if (itemRollNumber < item.Weight)
            {
                itemType = item.Type;
                break;
            }
            itemRollNumber -= item.Weight;
        }

        if (itemType == null)
            return;
        EntityManager.CreateEntityAt(itemType, Owner.Position.Pixel);
    }
}
