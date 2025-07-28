using System;

namespace Engine.ECS.Components.ControlHandling.Conditions;

public class ConditionXFacingAwayFromSpawn : Condition
{
    protected override bool IsTrue()
    {
        var spawnDirection = Math.Sign(Owner.Position.Pixel.X - Owner.Position.StartingPosition.X);
        return Owner.Facing.X == spawnDirection;
    }
}
