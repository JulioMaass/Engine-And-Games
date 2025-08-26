using Engine.ECS.Components.ShootingHandling;
using MMDB.GameSpecific.Shooters;

namespace MMDB.GameSpecific.Weapons;

public class WeaponJungle : Weapon
{
    public WeaponJungle(Entity owner)
    {
        Owner = owner;
        PrimaryShooter = new JungleSeedShooter(Owner);
        PaletteId = 18;
    }
}
