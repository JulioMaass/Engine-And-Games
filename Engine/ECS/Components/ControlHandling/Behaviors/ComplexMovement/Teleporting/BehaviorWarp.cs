using Engine.ECS.Entities;
using Engine.ECS.Entities.EntityCreation;
using Engine.Managers.StageHandling;
using System;

namespace Engine.ECS.Components.ControlHandling.Behaviors.ComplexMovement.Teleporting;

public class BehaviorWarp : Behavior
{
    public int MaxWarps { get; set; }
    private int CurrentWarps { get; set; }
    private Type VfxType { get; set; }

    public BehaviorWarp(Entity owner, int maxWarps, Type vfxType = null)
    {
        MaxWarps = maxWarps;
        owner.SpawnManager.DespawnOnScreenExit = false;
        VfxType = vfxType;
    }

    public override void Action()
    {
        if (CurrentWarps >= MaxWarps)
        {
            Owner.SpawnManager.DespawnOnScreenExit = true;
            return;
        }

        if (Owner.Position.Pixel.X < StageManager.CurrentRoom.PositionInPixels.X)
        {
            CreateTeleportVfx();
            Owner.Position.SetPixelX(StageManager.CurrentRoom.PositionInPixels.X + StageManager.CurrentRoom.SizeInPixels.X);
            CreateTeleportVfx();
        }
        if (Owner.Position.Pixel.X > StageManager.CurrentRoom.PositionInPixels.X + StageManager.CurrentRoom.SizeInPixels.X)
        {
            CreateTeleportVfx();
            Owner.Position.SetPixelX(StageManager.CurrentRoom.PositionInPixels.X);
            CreateTeleportVfx();
        }
        if (Owner.Position.Pixel.Y < StageManager.CurrentRoom.PositionInPixels.Y)
        {
            CreateTeleportVfx();
            Owner.Position.SetPixelY(StageManager.CurrentRoom.PositionInPixels.Y + StageManager.CurrentRoom.SizeInPixels.Y);
            CreateTeleportVfx();
            CurrentWarps++;
        }
        if (Owner.Position.Pixel.Y > StageManager.CurrentRoom.PositionInPixels.Y + StageManager.CurrentRoom.SizeInPixels.Y)
        {
            CreateTeleportVfx();
            Owner.Position.SetPixelY(StageManager.CurrentRoom.PositionInPixels.Y);
            CreateTeleportVfx();
            CurrentWarps++;
        }
    }

    public void CreateTeleportVfx()
    {
        if (VfxType != null)
            EntityManager.CreateEntityAt(VfxType, Owner.Position.Pixel);
    }
}
