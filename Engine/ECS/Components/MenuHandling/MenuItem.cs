using Engine.ECS.Entities.EntityCreation;
using Engine.Managers;
using Engine.Managers.GlobalManagement;
using System;

namespace Engine.ECS.Components.MenuHandling;

public class MenuItem : Component
{
    // Visuals
    public string Label { get; set; }
    public Action Draw { get; set; }
    public Action OnSelectDraw { get; set; }
    // Functionality
    public Action OnSelect { get; set; }
    public Type ContainedMenuLayoutType { get; set; }
    // Navigation
    public MenuArea OwningMenuArea { get; set; }
    // Buyable
    public Func<int> OwnerAmountGetter { get; set; }
    public Action OnSuccessfulBuy { get; set; }

    public MenuItem(Entity owner)
    {
        Owner = owner;
    }

    public void OpenContainedMenuLayout()
    {
        MenuManager.LayoutDirectoryList.Add(ContainedMenuLayoutType);
        MenuManager.UpdateAvailableMenuItems();
        MenuManager.SetSelectedItem(MenuManager.CurrentMenuLayout.StartingCursorItem);
    }

    public void Select()
    {
        if (OnSuccessfulBuy != null)
            TryToBuy();
        OnSelect?.Invoke();
        if (ContainedMenuLayoutType != null)
            OpenContainedMenuLayout();
    }

    public void TryToBuy()
    {
        var ownedAmount = OwnerAmountGetter();
        if (GlobalManager.Values.MainCharData.IsResourceFull(Owner.GetType()))
            return;
        var itemPrice = Owner.ItemPrice?.GetCurrentPrice(ownedAmount);
        if (itemPrice == null)
            return;
        if (!Owner.ItemPrice.CanBuy(ownedAmount))
            return;
        Owner.ItemPrice.SubtractResources(ownedAmount);
        OnSuccessfulBuy.Invoke();
    }
}