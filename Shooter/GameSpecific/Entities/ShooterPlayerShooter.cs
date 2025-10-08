using Engine.ECS.Components.ShootingHandling;
using Engine.Managers.Input;
using Engine.Types;

namespace ShooterGame.GameSpecific.Entities;

public class ShooterPlayerShooter : Shooter
{
    public ShooterPlayerShooter(Entity owner) : base(owner)
    {
        Owner = owner;
        AddShootAction(() => ShootAtPosition(MouseHandler.MousePositionOnGame));
        RelativeSpawnPosition = IntVector2.New(0, 0);
        ShotType = typeof(ShooterPlayerShot);
    }
}