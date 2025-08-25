using System.Collections.Generic;

namespace Engine.ECS.Components.ShootingHandling;

public class Weapon : Component
{
    // Shooters
    public Shooter PrimaryShooter { get; set; }
    public Shooter PrimaryShooterMidCharge { get; set; }
    public Shooter PrimaryShooterFullCharge { get; set; }
    public Shooter SecondaryShooter { get; set; }

    // Palette
    public int PaletteId { get; set; }
    public List<int> PaletteMidPattern { get; set; }
    public List<int> PaletteFullPattern { get; set; }

    public void ShootPrimary()
    {
        if (Owner.ChargeManager == null)
        {
            PrimaryShooter?.CheckToShoot();
            return;
        }
        if (Owner.ChargeManager?.GetChargeTier() == 0)
            PrimaryShooter?.CheckToShoot();
        else if (Owner.ChargeManager?.GetChargeTier() == 1)
            PrimaryShooterMidCharge?.CheckToShoot();
        else if (Owner.ChargeManager?.GetChargeTier() == 2)
            PrimaryShooterFullCharge?.CheckToShoot();
    }
}
