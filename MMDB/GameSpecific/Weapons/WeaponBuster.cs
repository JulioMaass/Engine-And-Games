using Engine.ECS.Components.ShootingHandling;
using MMDB.GameSpecific.Shooters;

namespace MMDB.GameSpecific.Weapons;

public class WeaponBuster : Weapon
{
    public WeaponBuster(Entity owner)
    {
        Owner = owner;
        PrimaryShooter = new LemonShooter(Owner);
        PrimaryShooterMidCharge = new BusterMidChargeShooter(Owner);
        PrimaryShooterFullCharge = new BusterFullChargeShooter(Owner);
        SecondaryShooter = null;

        PaletteId = 0;
        PaletteMidPattern = [1, 0, 1, 0];
        PaletteFullPattern = [2, 3, 1, 0]; //[2, 0, 1, 0]; //Original
    }
}
