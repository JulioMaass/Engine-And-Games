namespace Engine.ECS.Components.ControlHandling.Conditions;

public class ConditionCollidesWithSolidAtDirection : Condition
{
    private int Distance { get; }

    public ConditionCollidesWithSolidAtDirection(int distance = 1)
    {
        Distance = distance;
    }

    protected override bool IsTrue()
    {
        var speed = Owner.MoveDirection.GetVectorLength();
        return Owner.Physics.SolidCollisionChecking.CollidesWithSolidWithPixelSpeed(speed);
    }
}