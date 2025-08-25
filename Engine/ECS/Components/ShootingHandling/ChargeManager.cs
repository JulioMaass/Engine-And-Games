using Engine.ECS.Entities.EntityCreation;
using System.Collections.Generic;

namespace Engine.ECS.Components.ShootingHandling;

public class ChargeManager : Component
{
    public int ChargeFrame { get; private set; }
    public List<int> ChargeTierFrames { get; set; } = new();

    public ChargeManager(Entity owner)
    {
        Owner = owner;
    }

    public void Update()
    {
        if (Owner.WeaponManager.CurrentWeapon.PrimaryShooterMidCharge == null && Owner.WeaponManager.CurrentWeapon.PrimaryShooterFullCharge == null)
        {
            ChargeFrame = 0;
            return;
        }

        if (Owner.PlayerControl.Button1Hold && !Owner.PlayerControl.Button1Press)
            ChargeFrame++;
        else
            ChargeFrame = 0;
    }

    public int GetChargeTier()
    {
        for (var i = ChargeTierFrames.Count - 1; i >= 0; i--)
        {
            if (ChargeFrame >= ChargeTierFrames[i])
                return i + 1;
        }
        return 0;
    }
}