using Engine.ECS.Components.ControlHandling.States;
using Engine.Helpers;
using Engine.Managers.Ai;
using Engine.Managers.StageHandling;

namespace Mole.GameSpecific.States;

public class StateWalkToPlayer : State
{
    private float Speed { get; }

    public StateWalkToPlayer(float speed)
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
            var tile = Owner.Position.Pixel.RoundDownToTileCoordinate() - StageManager.CurrentRoom.PositionInTiles;
            var neighbors = tile.GetNeighbors();
            neighbors.Shuffle();

            var distanceToPlayer = 999;
            foreach (var neighbor in neighbors)
            {
                // check if in bounds
                var insideXBounds = neighbor.X >= 0 && neighbor.X < AiManager.DistanceMap.GetLength(0);
                var insideYBounds = neighbor.Y >= 0 && neighbor.Y < AiManager.DistanceMap.GetLength(1);
                if (!insideXBounds || !insideYBounds)
                    continue;

                var tileDistance = AiManager.DistanceMap[neighbor.X, neighbor.Y];
                if (tileDistance <= distanceToPlayer)
                {
                    Owner.MoveDirection.SetAngleDirectionToTile(neighbor + StageManager.CurrentRoom.PositionInTiles);
                    distanceToPlayer = tileDistance;
                }
            }
        }

        Owner.Speed.SetSpeedToCurrentDirection(Speed);
    }
}
