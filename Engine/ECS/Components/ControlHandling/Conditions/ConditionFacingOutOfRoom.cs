using Engine.Managers.Graphics;
using Engine.Managers.StageHandling;
using Engine.Types;
using System;

namespace Engine.ECS.Components.ControlHandling.Conditions;

public class ConditionFacingOutOfRoom : Condition
{
    protected override bool IsTrue()
    {
        var collisionDirectionLedge = IntVector2.New(Owner.Facing.X, 1);
        var edgePosition = Owner.CollisionBox.GetEdgePosition(collisionDirectionLedge);
        return StageManager.CurrentStage.GetRoomAtPixel(edgePosition) != StageManager.CurrentRoom;
    }
}
