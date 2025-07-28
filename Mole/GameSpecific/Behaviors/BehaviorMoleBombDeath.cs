using Engine.ECS.Components.ControlHandling.Behaviors;
using Engine.ECS.Entities;
using Engine.Types;
using Mole.GameSpecific.Entities;

namespace Mole.GameSpecific.Behaviors;

public class BehaviorMoleBombDeath : Behavior
{
    public override void Action()
    {
        IntVector2[] positions =
        {
            new(0, 0),
            new(-12, -12), new(12, -12), new(-12, 12), new(12, 12),
            new(0, -16), new(0, 16), new(-16, 0), new(16, 0),
        };
        foreach (var position in positions)
            EntityManager.CreateEntityAt<MoleBombExplosion>(Owner.Position.Pixel + position);
    }
}
