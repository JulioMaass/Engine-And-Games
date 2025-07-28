using Candle.GameSpecific.Entities.Currency;
using Engine.ECS.Components.PhysicsHandling;
using Engine.ECS.Entities.EntityCreation;

namespace Candle.GameSpecific.Entities.Gimmicks;

public class Crate : Entity
{
    public Crate()
    {
        EntityKind = EntityKind.Enemy;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteFullImageCenteredOrigin("Crate");
        AddCenteredCollisionBox();
        AddCandleGimmickComponents();

        AddDamageTaker(1);
        AddAlignment(Engine.ECS.Components.CombatHandling.AlignmentType.Neutral);
        AddSolidBehavior(SolidType.Solid, SolidInteractionType.StopOnSolids);
        AddGravity();

        AddItemDropper(
            (null, 17),
            (typeof(WaxDrop), 9),
            (typeof(WaxBit), 3),
            (typeof(WaxBall), 1)
        );
    }
}