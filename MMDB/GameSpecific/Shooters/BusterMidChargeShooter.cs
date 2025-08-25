using Engine.ECS.Components.ShootingHandling;
using MMDB.GameSpecific.Entities.Weapons;

namespace MMDB.GameSpecific.Shooters;

public class BusterMidChargeShooter : Shooter
{
    public BusterMidChargeShooter(Engine.ECS.Entities.EntityCreation.Entity owner) : base(owner)
    {
        Owner = owner;
        AddShootAction(ShootStraight);
        RelativeSpawnPosition = Owner.RelativePosition.ShotCreation;
        ShotType = typeof(MediumCharge);
        //SoundName = "sndShot";
    }
}