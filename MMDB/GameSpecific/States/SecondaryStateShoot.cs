using Engine.ECS.Components.ControlHandling.SecondaryStates;

namespace MMDB.GameSpecific.States;

public class SecondaryStateShoot : SecondaryState
{
    public override bool StartCondition()
    {
        var shootPress = Owner.PlayerControl.Button1Press;
        var shotCountBelowLimit = Owner.WeaponManager.GetShotPriceCount() < Owner.WeaponManager.ShotLimit;
        var shootUncharged = shootPress && shotCountBelowLimit;

        var isCharged = Owner.ChargeManager?.GetChargeTier() > 0;
        var isReleasing = !Owner.PlayerControl.Button1Hold;
        var shootCharge = isCharged && isReleasing;

        return shootUncharged || shootCharge;
    }

    public override bool KeepCondition()
    {
        return Frame < Duration;
    }

    // Behaviors
    public override void StateSettingBehavior() // TODO - BUG: Shooting before moving makes the shot spawn in the wrong position (past frame)
    {
        Owner.WeaponManager.CurrentWeapon?.ShootPrimary();
    }

    public override void Behavior() { }
}