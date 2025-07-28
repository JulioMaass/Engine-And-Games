using Engine.ECS.Components.ShootingHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Managers;
using Engine.Types;

namespace ShooterGame.GameSpecific.Entities;

public class ShooterPlayerShooter : Shooter
{
    public ShooterPlayerShooter(Entity owner) : base(owner)
    {
        Owner = owner;
        AddShootAction(() => ShootAtPosition(Input.MousePositionOnGame));
        RelativeSpawnPosition = IntVector2.New(0, 0);
        ShotType = typeof(ShooterPlayerShot);
    }
}