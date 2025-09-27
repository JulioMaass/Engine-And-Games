using Engine.ECS.Components.CombatHandling;
using Engine.ECS.Entities;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;
using Microsoft.Xna.Framework;
using System;

namespace Engine.ECS.Components.ControlHandling.Behaviors.EntityCreation;

public class BehaviorCreateBlast : Behavior
{
    public Type EntityType { get; set; }
    public IntVector2 RelativePosition { get; set; }
    public EntityKind EntityKind { get; set; }
    public AlignmentType Alignment { get; set; }
    public int Duration { get; set; }
    public int Damage { get; set; }
    public int Size { get; set; }
    public Color Color { get; set; }

    public BehaviorCreateBlast(Type type, EntityKind entityKind, AlignmentType alignment, int duration, int damage, int size, Color color)
    {
        EntityType = type;
        EntityKind = entityKind;
        Alignment = alignment;
        Duration = duration;
        Damage = damage;
        Size = size;
        Color = color;
    }

    public override void Action()
    {
        var position = Owner.Position.Pixel + RelativePosition;
        var entity = EntityManager.CreateEntityAt(EntityType, position);

        // Set properties
        entity.AssignEntityKind(EntityKind);
        entity.AddAlignment(Alignment);
        entity.AddFrameCounter(Duration);
        entity.AddDamageDealer(Damage);
        entity.Sprite.StretchedSize = new IntVector2(Size, Size);
        entity.AddCenteredOutlinedCollisionBox();
        entity.Sprite.SetColor(Color);
    }
}
