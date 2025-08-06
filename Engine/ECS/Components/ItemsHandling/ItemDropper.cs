using Engine.ECS.Entities;
using Engine.ECS.Entities.EntityCreation;
using Engine.Helpers;
using Engine.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.ECS.Components.ItemsHandling;

public class ItemDropper : Component
{
    public List<(List<Type> Types, int Weight)> DropTable { get; } = new();
    public int DropDistance { get; set; }

    public ItemDropper(Entity owner)
    {
        Owner = owner;
    }

    public void DropItem()
    {
        var totalWeight = DropTable.Sum(item => item.Weight);
        var itemRollNumber = GetRandom.UnseededInt(totalWeight);
        List<Type> itemTypes = null;

        // Scroll through item table until roll number is reached, subtracting weight of each scrolled item
        foreach (var item in DropTable)
        {
            if (itemRollNumber < item.Weight)
            {
                itemTypes = item.Types;
                break;
            }
            itemRollNumber -= item.Weight;
        }

        if (itemTypes == null)
            return;
        foreach (var itemType in itemTypes)
        {
            var relativePosition = DropDistance * Angle.GetVectorLength(GetRandom.UnseededInt(0, 360000));
            EntityManager.CreateEntityAt(itemType, Owner.Position.Pixel + relativePosition);
        }
    }
}
