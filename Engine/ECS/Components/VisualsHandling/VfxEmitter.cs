using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.ECS.Components.PositionHandling;
using Engine.ECS.Entities;
using Engine.ECS.Entities.EntityCreation;
using Engine.Helpers;
using Engine.Types;

namespace Engine.ECS.Components.VisualsHandling;

public class VfxEmitter : Component
{
    public int Frame { get; set; }

    // Emission properties
    public Type VfxType { get; set; }
    public int EmissionFrames { get; set; }
    public int EmissionsPerFrame { get; set; }
    public int Distance { get; set; }

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
        if (Frame < EmissionFrames)
        {
            for (var i = 0; i < EmissionsPerFrame; i++)
            {
                var relativePosition = new IntVector2(GetRandom.UnseededInt(-Distance, Distance), GetRandom.UnseededInt(-Distance, Distance));
                while (IntVector2.GetDistance(IntVector2.Zero, relativePosition) > Distance)
                    relativePosition = new IntVector2(GetRandom.UnseededInt(-Distance, Distance), GetRandom.UnseededInt(-Distance, Distance));
                EntityManager.CreateEntityAt(VfxType, Owner.Position.Pixel + relativePosition);
            }
            Frame++;
        }
        else
            EntityManager.DeleteEntity(Owner); // TODO: This should be optional (entity may be permanent)
    }
}
