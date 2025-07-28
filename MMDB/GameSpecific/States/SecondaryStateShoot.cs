using Engine.ECS.Components.ControlHandling.SecondaryStates;
using Engine.ECS.Entities;
using Engine.Managers.Audio;
using MMDB.GameSpecific.Entities.Weapons;

namespace MMDB.GameSpecific.States;

public class SecondaryStateShoot : SecondaryState
{
    public override bool StartCondition()
    {
        var isCharged = Owner.ChargeManager?.GetChargeTier() > 0;
        var isReleasing = !Owner.PlayerControl.Button1Hold;
        var shootCharge = isCharged && isReleasing;
        var shootPress = Owner.PlayerControl.Button1Press;
        var commandShoot = shootPress || shootCharge;

        var shotCountBelowLimit = Owner.WeaponManager.GetShotCount() < Owner.WeaponManager.ShotLimit;

        return commandShoot && shotCountBelowLimit;
    }

    public override bool KeepCondition()
    {
        return Frame < Duration;
    }

    // Behaviors
    public override void StateSettingBehavior() // TODO - BUG: Shooting before moving makes the shot spawn in the wrong position (past frame)
    {
        if (Owner.PlayerControl.Button1Hold)
        {
            var position = Owner.Position.Pixel + Owner.RelativePosition.ShotCreation * (Owner.Facing.X, 1);
            var shot = EntityManager.CreateEntityAt<Lemon>(position);
            shot.Speed.SetXSpeed(Owner.Facing.X * shot.Speed.X);
            shot.Facing.CopyFacingFrom(Owner);
            AudioManager.PlaySound("sndShot");
            Owner.WeaponManager.CountedShots.Add(shot);
        }
        else if (Owner.ChargeManager?.GetChargeTier() < 3)
        {
            var position = Owner.Position.Pixel + Owner.RelativePosition.ShotCreation * (Owner.Facing.X, 1);
            var shot = EntityManager.CreateEntityAt<MediumCharge>(position);
            shot.Speed.SetXSpeed(Owner.Facing.X * shot.Speed.X);
            shot.Facing.CopyFacingFrom(Owner);
            Owner.WeaponManager.CountedShots.Add(shot);
        }
        else if (Owner.ChargeManager?.GetChargeTier() >= 3)
        {
            var position = Owner.Position.Pixel + Owner.RelativePosition.ShotCreation * (Owner.Facing.X, 1);
            var shot = EntityManager.CreateEntityAt<BigCharge>(position);
            shot.Speed.SetXSpeed(Owner.Facing.X * shot.Speed.X);
            shot.Facing.CopyFacingFrom(Owner);
            Owner.WeaponManager.CountedShots.Add(shot);
        }
    }

    public override void Behavior() { }
}