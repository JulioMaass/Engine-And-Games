using Engine.ECS.Components.ShootingHandling;

namespace MMDB.GameSpecific.Weapons;

public class WeaponRemote : Weapon
{
    public WeaponRemote(Entity owner)
    {
        Owner = owner;
        PaletteId = 21;
    }
}
