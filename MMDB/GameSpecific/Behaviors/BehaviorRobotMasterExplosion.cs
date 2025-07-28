using Engine.ECS.Components.ControlHandling.Behaviors;
using Engine.ECS.Entities;
using Microsoft.Xna.Framework;
using MMDB.GameSpecific.Entities;

namespace MMDB.GameSpecific.Behaviors;

public class BehaviorRobotMasterExplosion : Behavior
{
    public override void Action()
    {
        Vector2[] speeds = {
            new (-1, -1), new (1, -1), new (-1, 1), new (1, 1),
            new (-2, -2), new (2, -2), new (-2, 2), new (2, 2),
            new (-1.5f, 0), new (1.5f, 0), new (0, -1.5f), new (0, 1.5f),
            new (-3f, 0), new (3f, 0), new (0, -3f), new (0, 3f)
        };

        foreach (var speed in speeds)
        {
            var entity = EntityManager.CreateEntityAt<ExplosionRobotMaster>(Owner.Position.Pixel);
            entity.Speed.SetSpeed(speed);
        }
    }
}
