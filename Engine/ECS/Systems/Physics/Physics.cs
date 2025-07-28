using Engine.ECS.Components;
using Engine.ECS.Components.ControlHandling.States;
using Engine.ECS.Components.PhysicsHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.ECS.Systems.Physics.GeneralPhysics;
using Engine.ECS.Systems.Physics.SolidPhysics;
using Engine.ECS.Systems.Physics.SpecialMovement;

namespace Engine.ECS.Systems.Physics;

public class Physics : Component
{
    // General Physics
    public EntityCollisionChecking EntityCollisionChecking { get; private set; }
    public TileCollisionChecking TileCollisionChecking { get; private set; }
    public FreeMovement FreeMovement { get; private set; }
    public PhysicsHelperFunctions PhysicsHelperFunctions { get; private set; }

    // Solid Physics
    public SolidCollisionChecking SolidCollisionChecking { get; private set; }
    public SolidCollidingMovement SolidCollidingMovement { get; private set; }
    public PushAndCarryMovement PushAndCarryMovement { get; private set; }

    // Special Movement
    public StairHandling StairHandling { get; private set; }
    public StopOnLedgeMovement StopOnLedgeMovement { get; private set; }
    public CrawlingMovement CrawlingMovement { get; private set; }
    public ParabolicMovement ParabolicMovement { get; private set; }

    public Physics(Entity owner)
    {
        Owner = owner;

        EntityCollisionChecking = new EntityCollisionChecking(Owner);
        TileCollisionChecking = new TileCollisionChecking(Owner);
        FreeMovement = new FreeMovement(Owner);
        PhysicsHelperFunctions = new PhysicsHelperFunctions(Owner);

        SolidCollisionChecking = new SolidCollisionChecking(Owner);
        SolidCollidingMovement = new SolidCollidingMovement(Owner);
        PushAndCarryMovement = new PushAndCarryMovement(Owner);

        StairHandling = new StairHandling(Owner);
        StopOnLedgeMovement = new StopOnLedgeMovement(Owner);
        CrawlingMovement = new CrawlingMovement(Owner);
        ParabolicMovement = new ParabolicMovement(Owner);
    }

    public void Update()
    {
        // TODO: Carry/push only works for SolidTops influencing not solid entities for now, for simplicity

        // Carry and push physics preparation
        if (Owner.SolidBehavior?.SolidType is SolidType.SolidTop)
        {
            PushAndCarryMovement.SavePreMovePosition();
            PushAndCarryMovement.UpdateCarriablesOnTop();
        }

        // Movement
        if (Owner.SolidBehavior is null
            || Owner.SolidBehavior.SolidInteractionType == SolidInteractionType.GoThroughSolids)
        {
            FreeMovement.MoveInPixelsAndFraction();
        }
        else if (Owner.SolidBehavior.SolidInteractionType == SolidInteractionType.StopOnSolids)
        {
            if (Owner.StateManager.CurrentState.MovementType == MovementType.Normal)
                SolidCollidingMovement.MoveToSolid();
            else if (Owner.StateManager.CurrentState.MovementType == MovementType.StopOnLedges)
                StopOnLedgeMovement.MoveToSolidAndStopOnLedges();
            else if (Owner.StateManager.CurrentState.MovementType == MovementType.Crawling)
                CrawlingMovement.Crawl();
        }

        // Carry physics
        if (Owner.SolidBehavior?.SolidType is SolidType.SolidTop)
            PushAndCarryMovement.CarryCarriablesOnTop();

        // Push physics
        if (Owner.SolidBehavior?.SolidType is SolidType.SolidTop
            && Owner.Physics.PushAndCarryMovement.FrameSpeed.Y < 0)
            PushAndCarryMovement.SnapPushablesUp();
    }
}