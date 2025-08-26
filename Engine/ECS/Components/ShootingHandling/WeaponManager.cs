using Engine.ECS.Entities;
using Engine.ECS.Entities.EntityCreation;
using System.Collections.Generic;
using System.Linq;

namespace Engine.ECS.Components.ShootingHandling;

public class WeaponManager : Component
{
    public int ShotLimit { get; set; }
    public List<Weapon> Weapons { get; } = new();
    public Weapon CurrentWeapon { get; set; }

    public WeaponManager(Entity owner)
    {
        Owner = owner;
    }

    public void Update()
    {
        CheckToSwitchWeapon();
    }

    public int GetShotCount() =>
        EntityManager.GetAllEntities().Count(e => e.Alignment?.OwningEntity == Owner);

    public int GetShotPriceCount()
    {
        var shots = EntityManager.GetAllEntities().Where(e => e.Alignment?.OwningEntity == Owner);
        return shots.Sum(s => s.ShotProperties?.ShotScreenPrice ?? 1);
    }

    private void CheckToSwitchWeapon()
    {
        if (Owner.PlayerControl?.SwitchLeft == true)
            SwitchWeapon(-1);
        else if (Owner.PlayerControl?.SwitchRight == true)
            SwitchWeapon(1);
        else if (Owner.PlayerControl?.SwitchReset == true)
            CurrentWeapon = Weapons.FirstOrDefault();
    }

    private void SwitchWeapon(int movePosition)
    {
        var currentPosition = Weapons.IndexOf(CurrentWeapon);
        currentPosition += movePosition + Weapons.Count;
        currentPosition %= Weapons.Count;
        CurrentWeapon = Weapons[currentPosition];
    }

    public int GetPaletteChargeOffset()
    {
        if (Owner.ChargeManager.GetChargeTier() == 1)
            return GetPaletteChargeId(CurrentWeapon.PaletteMidPattern, 4, 2, 20);
        if (Owner.ChargeManager.GetChargeTier() == 2 && CurrentWeapon.PaletteFullPattern != null)
            return GetPaletteChargeId(CurrentWeapon.PaletteFullPattern, 2, 2, 0);
        return 0;
    }

    private int GetPaletteChargeId(List<int> pattern, int slowSpeed, int fastSpeed, int speedChangeFrame)
    {
        var chargeTier = Owner.ChargeManager.GetChargeTier();
        var chargeTierFrame = Owner.ChargeManager.ChargeFrame - Owner.ChargeManager.ChargeTierFrames[chargeTier - 1];
        if (pattern == null)
            return 0;

        int id;
        if (chargeTierFrame < speedChangeFrame)
            id = chargeTierFrame / slowSpeed % pattern.Count;
        else
        {
            id = speedChangeFrame / pattern.Count;
            id += (chargeTierFrame - speedChangeFrame) / fastSpeed % pattern.Count;
        }
        return pattern[id % pattern.Count];
    }
}