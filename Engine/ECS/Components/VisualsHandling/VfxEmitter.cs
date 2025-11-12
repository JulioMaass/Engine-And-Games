using Engine.ECS.Entities;
using Engine.ECS.Entities.EntityCreation;
using Engine.Helpers;
using Engine.Types;
using System;

namespace Engine.ECS.Components.VisualsHandling;

public class VfxEmitter : Component
{
    public int Frame { get; set; }

    // Emission properties
    public Type VfxType { get; set; }
    public int EmissionFrames { get; set; } // 0 = infinite (entity doesn't delete itself)
    public int EmissionsPerFrame { get; set; }
    public int Distance { get; set; }
    // Movement
    public float DistanceToSpeedMultiplier { get; set; } // How much speed to apply based on distance

    public VfxEmitter(Entity owner, Type vfxType, int emissionFrames, int emissionsPerFrame, int distance)
    {
        Owner = owner;
        VfxType = vfxType;
        EmissionFrames = emissionFrames;
        EmissionsPerFrame = emissionsPerFrame;
        Distance = distance;
    }

    public void Update()
    {
        if (Frame < EmissionFrames || EmissionFrames == 0)
        {
            for (var i = 0; i < EmissionsPerFrame; i++)
            {
                var relativePosition = new IntVector2(GetRandom.UnseededInt(-Distance, Distance), GetRandom.UnseededInt(-Distance, Distance));
                while (IntVector2.GetDistance(IntVector2.Zero, relativePosition) > Distance)
                    relativePosition = new IntVector2(GetRandom.UnseededInt(-Distance, Distance), GetRandom.UnseededInt(-Distance, Distance));
                var entity = EntityManager.CreateEntityAt(VfxType, Owner.Position.Pixel + relativePosition);
                SetSpeedFromDistance(entity, relativePosition);
            }
            Frame++;
        }
        else
            EntityManager.MarkEntityForDeletion(Owner);
    }

    private void SetSpeedFromDistance(Entity entity, IntVector2 distance)
    {
        var moveDirection = Angle.GetAngleFromDistanceCoordinates(distance);
        var moveSpeed = IntVector2.GetDistance(IntVector2.Zero, distance) * DistanceToSpeedMultiplier;
        entity.AddMoveDirection(moveDirection.Value);
        entity.AddMoveSpeed(moveSpeed);
    }
}
