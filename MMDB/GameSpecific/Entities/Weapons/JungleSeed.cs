using Engine.ECS.Components.CombatHandling;
using Engine.ECS.Components.PhysicsHandling;
using Engine.ECS.Entities;
using Engine.ECS.Entities.EntityCreation;
using Engine.Helpers;
using MMDB.GameSpecific.Behaviors;
using MMDB.GameSpecific.Entities.Vfx;
using System.Linq;

namespace MMDB.GameSpecific.Entities.Weapons;

public class JungleSeed : Entity
{
    public int SplitLevel { get; private set; } = 3;

    public JungleSeed()
    {
        EntityKind = EntityKind.PlayerShot;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSprite("JungleSeed", 15, 15, 8, 8);
        AddPlayerShotComponents(999, PiercingType.PierceNone, 3);
        AddSolidBehavior(SolidType.NotSolid, SolidInteractionType.StopOnSolids, MomentumType.StopBothAxesOnHit);

        // Custom components
        AddMoveSpeed(5f);
        AddCenteredCollisionBox(7);
        AddDeathHandler(new BehaviorJungleSeedDeathSplitting());

        // States
        AddStateManager();
        var state = NewState()
            .AddBehavior(new BehaviorJungleSeedWallSplitting())
            .AddBehavior(new BehaviorJungleSeedCommandSplitting());
        StateManager.AutomaticStatesList.Add(state);
    }

    public void SetLevel(int level)
    {
        SplitLevel = level;
        Sprite.SpriteSheetOrigin = ((3 - level) * 15, 0);
        if (level == 0)
            ShotProperties.ShotScreenPrice = 0;
    }

    protected override void CustomUpdate()
    {
        // Make leaf vfx
        if (FrameHandler.CurrentFrame % 4 == 0)
        {
            var leafPosition = Position.Pixel + (GetRandom.UnseededInt(-2, 2), GetRandom.UnseededInt(-2, 2));
            var leaf = EntityManager.CreateEntityAt(typeof(VfxJungleLeaf), leafPosition);

            leaf.StateManager.CommandState(leaf.StateManager.AutomaticStatesList.FirstOrDefault());
            var frameOffset = (3 - SplitLevel) * 6 + 8;
            leaf.StateManager.CurrentState.Frame = frameOffset;
            leaf.FrameHandler.SetFrame(frameOffset);
            leaf.FrameHandler.CheckDurationEnd();
            leaf.StateManager.CurrentState.UpdateAnimationFrame();
            leaf.Facing.SetXIfNotZero(GetRandom.UnseededBool() ? 1 : -1);

            if (SplitLevel == 3)
                leaf.Sprite.SetColor(16, 128 + 64, 32, 255);
            if (SplitLevel == 2)
                leaf.Sprite.SetColor(16, 128 + 48, 32, 255);
            if (SplitLevel == 1)
                leaf.Sprite.SetColor(16, 128 + 32, 32, 255);
            if (SplitLevel == 0)
                leaf.Sprite.SetColor(16, 128 + 16, 32, 255);
        }
    }
}