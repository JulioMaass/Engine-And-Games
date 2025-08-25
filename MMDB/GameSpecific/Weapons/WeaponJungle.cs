using Engine.ECS.Components.ShootingHandling;

namespace MMDB.GameSpecific.Weapons;

public class WeaponJungle : Weapon
{
    public WeaponJungle(Entity owner)
    {
        Owner = owner;
        PaletteId = 18;
    }
}
