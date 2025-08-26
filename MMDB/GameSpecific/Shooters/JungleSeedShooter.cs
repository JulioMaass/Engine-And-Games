using Engine.ECS.Components.ShootingHandling;
using MMDB.GameSpecific.Entities.Weapons;

namespace MMDB.GameSpecific.Shooters;

public class JungleSeedShooter : Shooter
{
    public JungleSeedShooter(Engine.ECS.Entities.EntityCreation.Entity owner) : base(owner)
    {
        Owner = owner;
        AddShootAction(ShootEightDirectionAim);
        RelativeSpawnPosition = Owner.RelativePosition.ShotCreation;
        ShotType = typeof(JungleSeed);
        SoundName = "sndShot";
    }
}