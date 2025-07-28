using Engine.ECS.Components.ControlHandling.States;
using Engine.Helpers;
using Engine.Managers.StageHandling;

namespace Mole.GameSpecific.States;

public class StateWalkToRandomNeighborTile : State
{
    private float Speed { get; }

    public StateWalkToRandomNeighborTile(float speed)
    {
        Speed = speed;
    }

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

    public override void StateSettingBehavior()
    {
    }

    public override void Behavior() // TODO: Organize code once we have a final state system
    {
        // Find a random free neighbor tile
        if (Owner.Position.Pixel.X % 16 == 8
            && Owner.Position.Pixel.Y % 16 == 8)
        {
            var tile = Owner.Position.Pixel.RoundDownToTileCoordinate();
            var neighbors = tile.GetNeighbors();
            neighbors.Shuffle();
            foreach (var neighbor in neighbors)
            {
                var tileType = StageManager.CurrentRoom.DebugTiles.GetTileTypeAt(neighbor);
                if (tileType == TileType.NoTile)
                    Owner.MoveDirection.SetAngleDirectionToTile(neighbor);
            }
        }

        Owner.Speed.SetSpeedToCurrentDirection(Speed);
    }
}
