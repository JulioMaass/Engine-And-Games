using Engine.ECS.Components.CombatHandling;
using Engine.ECS.Components.ItemsHandling;
using Engine.ECS.Components.PhysicsHandling;
using Engine.ECS.Components.VisualsHandling;
using Engine.ECS.Entities;
using Engine.Helpers;
using Engine.Managers;
using Engine.Managers.GlobalManagement;
using Engine.Managers.Graphics;
using Engine.Types;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
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

    public void AddItemComponents(ResourceType resource, int amount, IncreaseKind increaseKind = IncreaseKind.Current)
    {
        // Position components
        AddCenteredOutlinedCollisionBox();

        // Physics components
        AddSpeed();
        AddGravity();
        AddSolidBehavior(SolidType.NotSolid, SolidInteractionType.StopOnSolids);

        // Item components
        AddResourceItemStats(resource, amount, increaseKind);
    }

    public void AddSpaceMinerUpgradeItemComponents()
    {
        // ReSharper disable once ComplexConditionExpression
        MenuItem.Draw = () =>
        {
            // Get item price
            var ownedAmount = GlobalManager.Values.MainCharData.GetAmountOfUpgradesOnWeapon(GetType());
            var itemPrice = ItemPrice.GetCurrentPrice(ownedAmount);
            string priceString = null;
            if (itemPrice == null)
                priceString = "-";
            else
            {
                foreach (var resourceCost in itemPrice.ResourceCosts)
                {
                    //priceString = priceString + resourceCost.ResourceType.ToString().Substring(3) + " ";
                    priceString = "  " + priceString + resourceCost.Amount + " ";
                }
            }

            //// Check for color limit
            //var hasBlue = GlobalManager.Values.MainCharData.GetAmountOfUpgradesOnWeapon(typeof(MenuItemSocketBlueAttackSpeed)) > 0;
            //var hasGreen = GlobalManager.Values.MainCharData.GetAmountOfUpgradesOnWeapon(typeof(MenuItemSocketGreenPierce)) > 0;
            //var hasRed = GlobalManager.Values.MainCharData.GetAmountOfUpgradesOnWeapon(typeof(MenuItemSocketRed)) > 0;
            //var hasYellow = GlobalManager.Values.MainCharData.GetAmountOfUpgradesOnWeapon(typeof(MenuItemSocketYellowStraightMulti)) > 0;
            //var colorCount = (hasBlue ? 1 : 0) + (hasGreen ? 1 : 0) + (hasRed ? 1 : 0) + (hasYellow ? 1 : 0);
            //if (colorCount >= 2 && ownedAmount == 0)
            //    priceString = "-";

            // Set color
            var color = CustomColor.White;
            if (!ItemPrice.CanBuy(ownedAmount) || priceString == "-")
                color = CustomColor.Gray;

            StringDrawer.DrawStringOutlined(StringDrawer.TinyUnicodeSoftFont, priceString, Position.Pixel + (-12, -12), color);
            ColorShader ??= new ColorShader(this);
            ColorShader.GrayscaleOn = color == CustomColor.Gray;
            Sprite.Color = color;
            Draw();
            if (itemPrice != null)
                DrawOreIcon(itemPrice.ResourceCosts[0].ResourceType, Position.Pixel + (-14, 20), color);
            //StringDrawer.DrawString(StringDrawer.TinyUnicodeFont, MenuItem?.Label, Position.Pixel + (0, 28), color);
        };

        MenuItem.OnSelectDraw = () =>
        {
            var cursorPosition = MenuManager.SelectedItem.Position.Pixel;
            StringDrawer.DrawStringOutlined(StringDrawer.PressStart2PShadowFont, ">", cursorPosition + (-24, -2), CustomColor.White);
            StringDrawer.DrawStringOutlined(StringDrawer.PressStart2PShadowFont, MenuItem?.Label, new IntVector2(64, 128 + 64 + 32 + 16), CustomColor.White);
        };

        // ReSharper disable once ComplexConditionExpression
        MenuItem.OnSelect = () =>
        {
            // Get item price
            var ownedAmount = GlobalManager.Values.MainCharData.GetAmountOfUpgradesOnWeapon(GetType());
            var itemPrice = ItemPrice.GetCurrentPrice(ownedAmount);
            if (itemPrice == null)
                return;

            //// Check for color limit
            //var hasAvailableColorSlot = true;
            //var hasBlue = GlobalManager.Values.MainCharData.GetAmountOfUpgradesOnWeapon(typeof(MenuItemSocketBlueAttackSpeed)) > 0;
            //var hasGreen = GlobalManager.Values.MainCharData.GetAmountOfUpgradesOnWeapon(typeof(MenuItemSocketGreenPierce)) > 0;
            //var hasRed = GlobalManager.Values.MainCharData.GetAmountOfUpgradesOnWeapon(typeof(MenuItemSocketRed)) > 0;
            //var hasYellow = GlobalManager.Values.MainCharData.GetAmountOfUpgradesOnWeapon(typeof(MenuItemSocketYellowStraightMulti)) > 0;
            //var colorCount = (hasBlue ? 1 : 0) + (hasGreen ? 1 : 0) + (hasRed ? 1 : 0) + (hasYellow ? 1 : 0);
            //if (colorCount >= 2 && ownedAmount == 0)
            //    hasAvailableColorSlot = false;

            // Check if player has enough resources to buy the item
            if (!ItemPrice.CanBuy(ownedAmount))
                return;
            ItemPrice.SubtractResources(ownedAmount);

            //// Equip item
            GlobalManager.Values.MainCharData.AddUpgradeToWeapon(GetType());
        };
    }

    private void DrawOreIcon(ResourceType resourceType, IntVector2 position, Color color)
    {
        var texture = Drawer.TextureDictionary.GetValueOrDefault("OreBlue"); // Default texture
        if (resourceType == ResourceType.OreBlue)
            texture = Drawer.TextureDictionary.GetValueOrDefault("OreBlue");
        if (resourceType == ResourceType.OreYellow)
            texture = Drawer.TextureDictionary.GetValueOrDefault("OreYellow");
        if (resourceType == ResourceType.OreGreen)
            texture = Drawer.TextureDictionary.GetValueOrDefault("OreGreen");
        if (resourceType == ResourceType.OreRed)
            texture = Drawer.TextureDictionary.GetValueOrDefault("OreRed");
        if (resourceType == ResourceType.OreOrange)
            texture = Drawer.TextureDictionary.GetValueOrDefault("OreOrange");

        var destinationRectangle = new IntRectangle(position, (8, 8));
        Video.SpriteBatch.Draw(texture, destinationRectangle, new IntRectangle(8, 0, 8, 8), color);
    }

    public void AddSpaceMinerWeaponItemComponents()
    {
        // ReSharper disable once ComplexConditionExpression
        MenuItem.Draw = () =>
        {
            // Get item price
            var ownedAmount = GlobalManager.Values.MainCharData.Equipment.Count(e => e.Type == GetType());
            var itemPrice = ItemPrice?.GetCurrentPrice(ownedAmount);
            string priceString = null;
            if (itemPrice == null)
                priceString = "";
            else
            {
                foreach (var resourceCost in itemPrice.ResourceCosts)
                {
                    //priceString = priceString + resourceCost.ResourceType.ToString().Substring(3) + " ";
                    priceString = "  " + priceString + resourceCost.Amount + " ";
                }
            }

            // Set color
            var color = CustomColor.White;
            if (ItemPrice?.CanBuy(ownedAmount) != true && ownedAmount == 0)
                color = CustomColor.Gray;

            // Draw upgrade levels
            var ownedUpgrades = GlobalManager.Values.MainCharData.GetAllUpgradesOnWeapon(GetType());
            (Type Type, int Amount) upgrade1 = (ownedUpgrades.FirstOrDefault(), 0);
            (Type Type, int Amount) upgrade2 = (ownedUpgrades.FirstOrDefault(u => u != upgrade1.Type), 0);
            foreach (var upgrade in ownedUpgrades)
                if (upgrade == upgrade1.Type)
                    upgrade1.Amount++;
                else if (upgrade == upgrade2.Type)
                    upgrade2.Amount++;
            if (upgrade1.Type != null)
            {
                CollectionManager.DrawEntityPreview(upgrade1.Type, Position.Pixel + (4, 42), color, 8);
                StringDrawer.DrawStringOutlined(StringDrawer.PressStart2PShadowFont, upgrade1.Amount.ToString(), Position.Pixel + (8, 38), color);
            }
            if (upgrade2.Type != null)
            {
                CollectionManager.DrawEntityPreview(upgrade2.Type, Position.Pixel + (4, 42 + 8), color, 8);
                StringDrawer.DrawStringOutlined(StringDrawer.PressStart2PShadowFont, upgrade2.Amount.ToString(), Position.Pixel + (8, 38 + 8), color);
            }

            StringDrawer.DrawStringOutlined(StringDrawer.TinyUnicodeSoftFont, priceString, Position.Pixel + (-12, 22), color);
            ColorShader ??= new ColorShader(this);
            ColorShader.GrayscaleOn = color == CustomColor.Gray;
            Sprite.Color = color;
            Draw();
            if (itemPrice != null)
                DrawOreIcon(itemPrice.ResourceCosts[0].ResourceType, Position.Pixel + (-14, 20), color);
            //StringDrawer.DrawString(StringDrawer.TinyUnicodeFont, MenuItem?.Label, Position.Pixel + (0, 28), color);

            // Draw rectangle selection on current weapon
            if (GlobalManager.Values.MainCharData.IsItemEquipped(GetType()))
                Drawer.DrawRectangleOutline(Position.Pixel + (-16, -16), (32, 32), CustomColor.White);
        };

        MenuItem.OnSelectDraw = () =>
        {
            var cursorPosition = MenuManager.SelectedItem.Position.Pixel;
            StringDrawer.DrawStringOutlined(StringDrawer.PressStart2PShadowFont, ">", cursorPosition + (-24, -2), CustomColor.White);
            StringDrawer.DrawStringOutlined(StringDrawer.PressStart2PShadowFont, MenuItem?.Label, new IntVector2(64, 128 + 64 + 32 + 16), CustomColor.White);
        };

        MenuItem.OnSelect = () =>
        {
            var ownedAmount = GlobalManager.Values.MainCharData.Equipment.Count(e => e.Type == GetType());
            if (ownedAmount > 0)
            {
                // If player already owns the item, just equip it
                GlobalManager.Values.MainCharData.TryToEquipItem(GetType());
                return;
            }

            // Get item price
            var itemPrice = ItemPrice.GetCurrentPrice(ownedAmount);
            if (itemPrice == null)
                return;

            // Check if player has enough resources to buy the item
            if (!ItemPrice.CanBuy(ownedAmount))
                return;
            ItemPrice.SubtractResources(ownedAmount);

            // Unlock and equip item
            GlobalManager.Values.MainCharData.AddSwitchEquipment(GetType(), 1, 0);
            GlobalManager.Values.MainCharData.TryToEquipItem(GetType());
        };
    }

    public void AddSpaceMinerMissileItemComponents()
    {
        // ReSharper disable once ComplexConditionExpression
        MenuItem.Draw = () =>
        {
            var ownedAmount = GlobalManager.Values.MainCharData.GetAmount(GetType());
            ownedAmount = Math.Min(ownedAmount, ItemPrice.PriceList.Count - 1); // Cap to max price
            var price = ItemPrice!.GetCurrentPrice(ownedAmount);

            var resourceCost = price.ResourceCosts.FirstOrDefault();
            var priceString = "  " + resourceCost.Amount;

            var color = CustomColor.White;
            if (!ItemPrice.CanBuy(ownedAmount))
                color = CustomColor.Gray;

            StringDrawer.DrawStringOutlined(StringDrawer.TinyUnicodeSoftFont, priceString, Position.Pixel + (-12, 22), color);
            ColorShader ??= new ColorShader(this);
            ColorShader.GrayscaleOn = color == CustomColor.Gray;
            Sprite.Color = color;
            Draw();
            var itemPrice = ItemPrice.GetCurrentPrice(ownedAmount);
            if (itemPrice != null)
                DrawOreIcon(itemPrice.ResourceCosts[0].ResourceType, Position.Pixel + (-14, 20), color);
        };

        MenuItem.OnSelectDraw = () =>
        {
            var cursorPosition = MenuManager.SelectedItem.Position.Pixel;
            StringDrawer.DrawStringOutlined(StringDrawer.PressStart2PShadowFont, ">", cursorPosition + (-24, -2), CustomColor.White);
            StringDrawer.DrawStringOutlined(StringDrawer.PressStart2PShadowFont, MenuItem?.Label, new IntVector2(64, 128 + 64 + 32 + 16), CustomColor.White);
        };

        // ReSharper disable once ComplexConditionExpression
        MenuItem.OnSelect = () =>
        {
            // Get item price
            var price = ItemPrice.PriceList.FirstOrDefault();
            var ownedAmount = GlobalManager.Values.MainCharData.GetAmount(GetType());
            ownedAmount = Math.Min(ownedAmount, ItemPrice.PriceList.Count - 1); // Cap to max price
            price = ItemPrice?.GetCurrentPrice(ownedAmount);
            if (price == null)
                return;

            // Check if player has enough resources to buy the item
            if (!ItemPrice.CanBuy(ownedAmount))
                return;
            ItemPrice.SubtractResources(ownedAmount);

            // Equip item
            EntityManager.PlayerEntity!.ItemGetter.GetItem(this);
            GlobalManager.Values.MainCharData.TryToEquipItem(GetType());
        };
    }
}