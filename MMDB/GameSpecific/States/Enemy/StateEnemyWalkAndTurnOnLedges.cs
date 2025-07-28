using Engine.ECS.Components.ControlHandling.States;
using Engine.Types;

namespace MMDB.GameSpecific.States.Enemy;

public class StateEnemyWalkAndTurnOnLedges : State
{
    public override MovementType MovementType { get; set; } = MovementType.StopOnLedges;

    public override bool StartCondition()
    {
        return true;
    }

    public override bool KeepCondition()
    {
        return true;
    }

    public override bool PostProcessingKeepCondition()
    {
        return true;
    }

    public override void Behavior()
    {
        // Invert direction if there is a wall
        var collisionDirection = IntVector2.New(Owner.Facing.X, 0);
        if (Owner.Physics.SolidCollisionChecking.CollidesWithSolidWithPixelSpeed(collisionDirection))
            Owner.Facing.InvertX();

        // Invert direction if there is a ledge // TODO: Simple implementation - improve this. Only checks for tile collision (not entities) and turns after going beyond the edge.
        // TODO: Also keeps turning while in the air
        var collisionDirectionLedge = IntVector2.New(Owner.Facing.X, 1);
        var edgePosition = Owner.CollisionBox.GetEdgePosition(collisionDirectionLedge) + IntVector2.PixelDown + collisionDirection;
        if (Owner.Physics.SolidCollisionChecking.IsOnTopOfSolid())
            if (!Owner.Physics.SolidCollisionChecking.IsThereSolidAtPoint(edgePosition))
                Owner.Facing.InvertX();

        Owner.Speed.SetXSpeed(Owner.Facing.X * Owner.Speed.MoveSpeed);
    }
}
