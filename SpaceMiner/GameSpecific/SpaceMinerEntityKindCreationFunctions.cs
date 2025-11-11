using Engine.ECS.Components.CombatHandling;
using Engine.ECS.Components.ItemsHandling;
using Engine.ECS.Components.MenuHandling;
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

        // Item components
        AddResourceItemStats(resource, amount, increaseKind);
    }

    public void AddSpaceMinerWeaponUpgradeItemComponents()
    {
        MenuItem.OwnerAmountGetter = () =>
            GlobalManager.Values.MainCharData.GetAmountOfUpgradesOnWeapon(GetType());

        MenuItem.OnSuccessfulBuy = () =>
            GlobalManager.Values.MainCharData.AddUpgradeToWeapon(GetType());

        MenuItem.Draw = () =>
            DrawSlot(MenuItem.OwnerAmountGetter(), ItemPrice.GetCurrentPrice(MenuItem.OwnerAmountGetter()));

        MenuItem.OnSelectDraw =
            OnSelectDraw;
    }

    public void AddSpaceMinerWeaponItemComponents()
    {
        MenuItem.OwnerAmountGetter = () =>
            GlobalManager.Values.MainCharData.Equipment.Count(e => e.Type == GetType());

        MenuItem.OnSuccessfulBuy = () =>
            GlobalManager.Values.MainCharData.AddSwitchEquipment(GetType(), 1, 0);

        MenuItem.OnSelect = () =>
            GlobalManager.Values.MainCharData.TryToEquipItem(GetType());

        MenuItem.Draw = () =>
        {
            DrawSlot(MenuItem.OwnerAmountGetter(), ItemPrice?.GetCurrentPrice(MenuItem.OwnerAmountGetter()));

            // Draw rectangle selection on current weapon
            if (GlobalManager.Values.MainCharData.IsItemEquipped(GetType()))
                Drawer.DrawRectangleOutline(Position.Pixel + (-16, -16), (32, 32), CustomColor.White);
        };

        MenuItem.OnSelectDraw =
            OnSelectDraw;
    }

    public void AddSpaceMinerShipUpgradeItemComponents()
    {
        MenuItem.OwnerAmountGetter = () =>
            GlobalManager.Values.MainCharData.GetAmount(GetType());

        MenuItem.OnSuccessfulBuy = () =>
            EntityManager.PlayerEntity!.ItemGetter.GetItem(this);

        MenuItem.OnSelect = () =>
            GlobalManager.Values.MainCharData.TryToEquipItem(GetType());

        MenuItem.Draw = () =>
        {
            var ownedAmount = MenuItem.OwnerAmountGetter();
            var isResourceFull = GlobalManager.Values.MainCharData.IsResourceFull(GetType());
            ownedAmount = Math.Min(ownedAmount, ItemPrice.PriceList.Count - 1); // Cap to max price
            DrawSlot(ownedAmount, ItemPrice.GetCurrentPrice(MenuItem.OwnerAmountGetter()), isResourceFull);
        };

        MenuItem.OnSelectDraw =
            OnSelectDraw;
    }

    public void AddSpaceMinerMissileItemComponents()
    {
        MenuItem.OwnerAmountGetter = () =>
            GlobalManager.Values.MainCharData.GetAmount(GetType());

        MenuItem.OnSuccessfulBuy = () =>
            EntityManager.PlayerEntity!.ItemGetter.GetItem(this);

        MenuItem.OnSelect = () =>
            GlobalManager.Values.MainCharData.TryToEquipItem(GetType());

        MenuItem.Draw = () =>
        {
            var ownedAmount = MenuItem.OwnerAmountGetter();
            var isResourceFull = GlobalManager.Values.MainCharData.IsResourceFull(GetType());
            ownedAmount = Math.Min(ownedAmount, ItemPrice.PriceList.Count - 1); // Cap to max price
            DrawSlot(ownedAmount, ItemPrice.GetCurrentPrice(MenuItem.OwnerAmountGetter()), isResourceFull);
        };

        MenuItem.OnSelectDraw =
            OnSelectDraw;
    }

    private void DrawSlot(int ownedAmount, ItemPrice.Price itemPrice, bool isResourceFull = false)
    {
        // Set color
        var color = CustomColor.White;
        var unlocked = ownedAmount > 0 && ItemPrice?.PriceType == PriceType.Unlock;
        if (!unlocked && (ItemPrice?.CanBuy(ownedAmount) != true || itemPrice == null || isResourceFull))
            color = CustomColor.Gray;

        ColorShader ??= new ColorShader(this);
        ColorShader.GrayscaleOn = color == CustomColor.Gray;
        Sprite.Color = color;
        Draw();

        if (itemPrice == null || unlocked)
            return;
        var i = 0;
        foreach (var resourceCost in itemPrice.ResourceCosts)
        {
            var priceColor = CustomColor.White;
            if (!GlobalManager.Values.MainCharData.Resources.HasResource(resourceCost.ResourceType, resourceCost.Amount))
                priceColor = CustomColor.Gray;
            StringDrawer.DrawStringOutlined(StringDrawer.TinyUnicodeSoftFont, resourceCost.Amount.ToString(), Position.Pixel + (-4, 21 + i * 10), priceColor);
            if (itemPrice != null)
                DrawOreIcon(resourceCost.ResourceType, Position.Pixel + (-14, 20 + i * 10), priceColor);
            i++;
        }
    }

    private void OnSelectDraw()
    {
        var cursorPosition = MenuManager.SelectedItem.Position.Pixel;
        StringDrawer.DrawStringOutlined(StringDrawer.PressStart2PShadowFont, ">", cursorPosition + (-24, -2), CustomColor.White);
        StringDrawer.DrawStringOutlined(StringDrawer.PressStart2PShadowFont, MenuItem?.Label, new IntVector2(64, 128 + 64 + 32 + 16), CustomColor.White);
    }

    private void DrawOreIcon(ResourceType resourceType, IntVector2 position, Color color)
    {
        var texture = Drawer.TextureDictionary.GetValueOrDefault("OreGray"); // Default texture
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
}