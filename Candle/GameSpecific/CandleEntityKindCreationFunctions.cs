using Candle.GameSpecific.Entities.Currency;
using Engine.ECS.Components.CombatHandling;
using Engine.ECS.Components.ControlHandling.Behaviors.Death;
using Engine.ECS.Entities;
using Engine.Helpers;
using Engine.Managers;
using Engine.Managers.GlobalManagement;
using Engine.Managers.Graphics;

namespace Candle.GameSpecific;

public abstract class Entity : Engine.ECS.Entities.EntityCreation.Entity
{
    public void AddCandleEnemyComponents(int hp, int damage)
    {
        AiControl = new(this);
        AddAlignment(AlignmentType.Hostile);
        AddDamageTaker(hp);
        AddDamageDealer(damage);
        AddDeathHandler(new BehaviorCandleHealPlayer(10));
        KnockbackReceiver = new();

        AddItemDropper(
            (typeof(WaxDrop), 9),
            (typeof(WaxBit), 3),
            (typeof(WaxBall), 1)
            );
    }

    public void AddCandleEnemyShotComponents(int damage, int hp = 0) // TODO: Copied from MMDB. Turn into a game generic function?
    {
        // Position components
        AddCenteredOutlinedCollisionBox();
        AddSpeed();

        // Combat components
        AddAlignment(AlignmentType.Hostile);
        AddDamageDealer(damage);
        if (hp > 0)
            AddDamageTaker(hp);
    }

    public void AddCandleGimmickComponents()
    {
        AddAlignment(AlignmentType.Hostile);
    }

    public void AddCandleEquipmentItemComponents()
    {
        // ReSharper disable once ComplexConditionExpression
        MenuItem.Draw = () =>
        {
            if (EquipmentItemStats.IsUnlocked())
            {
                CollectionManager.DrawEntityPreview(GetType(), Position.Pixel + (8, 8), CustomColor.White);
                Video.SpriteBatch.DrawString(Drawer.MegaManFont, EquipmentItemStats.GetLevel().ToString(), Position.Pixel + (18, 4), CustomColor.White);
            }
            var label = MenuItem?.Label;
            if (label != null)
                Video.SpriteBatch.DrawString(Drawer.MegaManFont, label, Position.Pixel + (0, 18), CustomColor.White);
            if (GlobalManager.Values.MainCharData.IsItemEquipped(GetType()))
                Drawer.DrawRectangleOutline(Position.Pixel - 4, (16 + 8, 16 + 8), CustomColor.White);
        };

        MenuItem.OnSelectDraw = () =>
        {
            var cursorPosition = MenuManager.SelectedItem.Position.Pixel;
            Video.SpriteBatch.DrawString(Drawer.MegaManFont, ">", cursorPosition + (-10, 6), CustomColor.White);
        };

        MenuItem.OnSelect = () =>
        {
            if (!EquipmentItemStats.IsUnlocked())
                return;
            GlobalManager.Values.MainCharData.TryToEquipItem(GetType());
        };
    }
}