using Engine.ECS.Components.ControlHandling.Behaviors;
using Engine.ECS.Components.ControlHandling.Behaviors.Shoot;
using Engine.ECS.Components.ControlHandling.Conditions;
using Engine.ECS.Components.ShootingHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Managers.Graphics;
using Engine.Managers.StageHandling;
using Engine.Types;
using MMDB.GameSpecific.Entities.EnemyShots;

namespace MMDB.GameSpecific.Entities.Enemies;

public class PoliceHeli : Entity
{
    public PoliceHeli()
    {
        EntityKind = EntityKind.Enemy;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSprite("PoliceHeli", 64, 64, 32, 32);
        AddMmdbEnemyComponents(16, 4);
        AddSolidBehavior();
        AddCollisionBox(48, 48, 24, 24);

        // Enemy specific components
        AddSpeed();

        // Shooter Manager
        Shooter = new Shooter(this);
        Shooter.AddShootAction(() => Shooter.ShootAtPlayer());
        Shooter.RelativeSpawnPosition = IntVector2.New(13, 3);
        Shooter.ShotType = typeof(EnemyBullet);
        Shooter.ShotModifiers.Add(e => e.Speed.MoveSpeed = 2.75f);

        // States
        AddStateManager();
        // Auto States
        var state = NewState()
            .AddBehaviorWithConditions(new BehaviorShoot(), new ConditionFrameLoop(120, 4, 30))
            .AddBehavior(new BehaviorCustom(() =>
            {
                if (!StageManager.IsTransitioning)
                {
                    Position.SetPixelX(Camera.Panning.X + 64);
                    Position.SetPixelY(Camera.Panning.Y + 64);
                }
            }))
            .AddToAutomaticStatesList();
    }
}