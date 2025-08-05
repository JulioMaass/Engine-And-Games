using Engine.ECS.Entities.EntityCreation;
using Engine.Managers.StageHandling;

namespace Engine.ECS.Components.ControlHandling.Behaviors;

public class BehaviorWarp : Behavior
{
    public int MaxWarps { get; set; }
    private int CurrentWarps { get; set; }

    public BehaviorWarp(Entity owner, int maxWarps)
    {
        MaxWarps = maxWarps;
        owner.SpawnManager.DespawnOnScreenExit = false;
    }

    public override void Action()
    {
        if (CurrentWarps >= MaxWarps)
        {
            Owner.SpawnManager.DespawnOnScreenExit = true;
            return;
        }

        if (Owner.Position.Pixel.X < StageManager.CurrentRoom.PositionInPixels.X)
            Owner.Position.SetPixelX(StageManager.CurrentRoom.PositionInPixels.X + StageManager.CurrentRoom.SizeInPixels.X);
        if (Owner.Position.Pixel.X > StageManager.CurrentRoom.PositionInPixels.X + StageManager.CurrentRoom.SizeInPixels.X)
            Owner.Position.SetPixelX(StageManager.CurrentRoom.PositionInPixels.X);
        if (Owner.Position.Pixel.Y < StageManager.CurrentRoom.PositionInPixels.Y)
        {
            Owner.Position.SetPixelY(StageManager.CurrentRoom.PositionInPixels.Y + StageManager.CurrentRoom.SizeInPixels.Y);
            CurrentWarps++;
        }
        if (Owner.Position.Pixel.Y > StageManager.CurrentRoom.PositionInPixels.Y + StageManager.CurrentRoom.SizeInPixels.Y)
        {
            Owner.Position.SetPixelY(StageManager.CurrentRoom.PositionInPixels.Y);
            CurrentWarps++;
        }
    }
}
