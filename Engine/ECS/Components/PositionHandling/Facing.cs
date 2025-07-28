using Engine.ECS.Entities.EntityCreation;

namespace Engine.ECS.Components.PositionHandling;

public class Facing : Component
{
    public int X { get; private set; }
    public int Y { get; private set; }
    public bool HasYFacing { get; set; }
    public bool IsXMirrored => X == -1;

    public Facing(Entity owner, int x = 1)
    {
        Owner = owner;
        X = x;
    }

    public void CopyFacingFrom(Entity entity)
    {
        X = entity.Facing.X;
        Y = entity.Facing.Y;
    }

    public void CopyFacingAndMirrorDirection(Entity entity)
    {
        CopyFacingFrom(entity);
        if (Owner.MoveDirection == null)
            Owner.AddMoveDirection();
        if (IsXMirrored)
            Owner.MoveDirection.MirrorX();
    }

    public void SetXIfNotZero(int x)
    {
        if (x == 0)
            return;

        X = x;
    }

    public void InvertX()
    {
        X = -X;
    }

    public void UpdateFacingBasedOnControlDirection()
    {
        if (Owner.PlayerControl == null)
            return;

        if (!HasYFacing)
            UpdateWithoutYFacing();
        else
            UpdateWithYFacing();
    }

    private void UpdateWithoutYFacing()
    {
        if (Owner.PlayerControl.DirectionX != 0)
            SetXIfNotZero(Owner.PlayerControl.DirectionX);
    }

    private void UpdateWithYFacing()
    {
        // If player is moving only in one direction, face that direction
        if (Owner.PlayerControl.DirectionX != 0 ^ Owner.PlayerControl.DirectionY != 0)
        {
            X = Owner.PlayerControl.DirectionX;
            Y = Owner.PlayerControl.DirectionY;
        }
        // If player is moving diagonally, keep facing the non-zero direction
        if (Owner.PlayerControl.DirectionX == 0 || Owner.PlayerControl.DirectionY == 0) return;
        if (X == 0)
            Y = Owner.PlayerControl.DirectionY;
        else
            X = Owner.PlayerControl.DirectionX;
    }
}
