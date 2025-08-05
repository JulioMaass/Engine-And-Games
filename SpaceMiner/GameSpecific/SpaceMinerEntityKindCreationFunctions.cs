using Engine.ECS.Components.CombatHandling;
using Engine.ECS.Components.PhysicsHandling;
using Engine.ECS.Entities;
using Engine.ECS.Entities.EntityCreation;
using Engine.Helpers;
using Engine.Managers;
using Engine.Managers.Graphics;
using Engine.Types;
using System.Linq;

namespace SpaceMiner.GameSpecific;

public abstract class Entity : Engine.ECS.Entities.EntityCreation.Entity
{
    public void AddSpaceMinerEnemyComponents(int hp, int damage)
    {
        AiControl = new(this);
        AddAlignment(AlignmentType.Hostile);
        AddDamageTaker(hp);
        AddDamageDealer(damage);
    }

    public void AddItemComponents(ResourceType resource, int amount)
    {
        // Position components
        AddCenteredOutlinedCollisionBox();

        // Physics components
        AddSpeed();
        AddGravity();
        AddSolidBehavior(SolidType.NotSolid, SolidInteractionType.StopOnSolids);

        // Item components
        AddResourceItemStats(resource, amount);
    }

    public void AddSpaceMinerEquipmentItemComponents()
    {
        // ReSharper disable once ComplexConditionExpression
        MenuItem.Draw = () =>
        {
            // Get item price
            var player = EntityManager.GetFilteredEntitiesFrom(EntityKind.Player).FirstOrDefault();
            var ownedAmount = player!.EquipmentHolder.GetEquipmentItemCount(GetType());
            var itemPrice = ItemPrice.GetCurrentPrice(ownedAmount);
            string priceString = null;
            if (itemPrice == null)
                priceString = "-";
            else
            {
                foreach (var resourceCost in itemPrice.ResourceCosts)
                {
                    priceString = priceString + resourceCost.ResourceType.ToString().Substring(3) + " ";
                    priceString = priceString + resourceCost.Amount + " ";
                }
            }
            var color = CustomColor.White;
            if (!ItemPrice.CanBuy(ownedAmount))
                color = CustomColor.Gray;

            Video.SpriteBatch.DrawString(Drawer.MegaManFont, priceString, Position.Pixel + (0, 38), color);
            CollectionManager.DrawEntityPreview(GetType(), Position.Pixel + (8, 8), color);
            Video.SpriteBatch.DrawString(Drawer.MegaManFont, MenuItem?.Label, Position.Pixel + (0, 28), color);
        };

        MenuItem.OnSelectDraw = () =>
        {
            var cursorPosition = MenuManager.SelectedItem.Position.Pixel;
            Video.SpriteBatch.DrawString(Drawer.MegaManFont, ">", cursorPosition + (-16, 6), CustomColor.White);
        };

        MenuItem.OnSelect = () =>
        {
            // Get item price
            var player = EntityManager.GetFilteredEntitiesFrom(EntityKind.Player).FirstOrDefault();
            var ownedAmount = player!.EquipmentHolder.GetEquipmentItemCount(GetType());
            var itemPrice = ItemPrice.GetCurrentPrice(ownedAmount);
            if (itemPrice == null)
                return;

            // Check if player has enough resources to buy the item
            if (!ItemPrice.CanBuy(ownedAmount))
                return;
            ItemPrice.SubtractResources(ownedAmount);

            // Equip item
            player.EquipmentHolder.EquipItem(GetType());
        };
    }

    public void AddSpaceMinerMissileItemComponents()
    {
        // ReSharper disable once ComplexConditionExpression
        MenuItem.Draw = () =>
        {
            var price = ItemPrice.PriceList.FirstOrDefault();
            var resourceCost = price.ResourceCosts.FirstOrDefault();
            var priceString = resourceCost.ResourceType.ToString().Substring(3) + " " + resourceCost.Amount;

            var color = CustomColor.White;
            if (!ItemPrice.CanBuy(0))
                color = CustomColor.Gray;
            Video.SpriteBatch.DrawString(Drawer.MegaManFont, priceString, Position.Pixel + (0, 38), color);
            CollectionManager.DrawEntityPreview(GetType(), Position.Pixel + (8, 8), color);
            Video.SpriteBatch.DrawString(Drawer.MegaManFont, MenuItem?.Label, Position.Pixel + (0, 28), color);
        };

        MenuItem.OnSelectDraw = () =>
        {
            var cursorPosition = MenuManager.SelectedItem.Position.Pixel;
            Video.SpriteBatch.DrawString(Drawer.MegaManFont, ">", cursorPosition + (-16, 6), CustomColor.White);
        };

        MenuItem.OnSelect = () =>
        {
            // Get item price
            var itemPrice = ItemPrice.PriceList.FirstOrDefault();
            if (itemPrice == null)
                return;

            // Check if player has enough resources to buy the item
            if (!ItemPrice.CanBuy(0))
                return;
            ItemPrice.SubtractResources(0);

            // Equip item
            var player = EntityManager.GetFilteredEntitiesFrom(EntityKind.Player).FirstOrDefault();
            player!.ItemGetter.GetItem(this);
            player.EquipmentHolder.EquipItem(GetType());
        };
    }
}