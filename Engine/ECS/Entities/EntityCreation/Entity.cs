using Engine.ECS.Components;
using Engine.ECS.Components.CombatHandling;
using Engine.ECS.Components.ControlHandling;
using Engine.ECS.Components.ItemsHandling;
using Engine.ECS.Components.LinkedEntities;
using Engine.ECS.Components.MenuHandling;
using Engine.ECS.Components.PhysicsHandling;
using Engine.ECS.Components.PositionHandling;
using Engine.ECS.Components.ShootingHandling;
using Engine.ECS.Components.Spawning;
using Engine.ECS.Components.VisualsHandling;
using Engine.ECS.Systems.Physics;
using Engine.Managers;
using Engine.Managers.Graphics;
using Engine.Managers.StageHandling;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global

namespace Engine.ECS.Entities.EntityCreation;

public abstract partial class Entity
{
    public bool MarkedForDeletion { get; set; }

    // Entity typing
    public EntityInstance EntityInstance { get; set; }
    public EntityKind EntityKind { get; protected set; }
    public EntityKind DrawAs { get; protected set; } = EntityKind.None; // If not None, overrides Owner.EntityKind for drawing order
    public EntityKind DrawingOrder => DrawAs == EntityKind.None ? EntityKind : DrawAs;

    // Component enforcer
    public ComponentEnforcer ComponentEnforcer { get; protected set; }
    public bool Updated { get; set; } // Used to check if entity was updated before the end of its first frame. Entities that are created and not updated may cause issues.

    // Spawning
    public SpawnManager SpawnManager { get; protected set; }
    public CustomValueHandler CustomValueHandler { get; protected set; }

    // Position components
    public Position Position { get; protected set; }
    public CollisionBox CollisionBox { get; protected set; }
    public Facing Facing { get; protected set; }
    public Direction MoveDirection { get; protected set; }
    public Direction ShootDirection { get; protected set; }
    public RelativePosition RelativePosition { get; protected set; }

    // Physics components
    public Speed Speed { get; protected set; }
    public Gravity Gravity { get; protected set; }
    public SolidBehavior SolidBehavior { get; protected set; }
    public TileDestructor TileDestructor { get; protected set; }

    // Linked entities
    public LinkedEntitiesManager LinkedEntitiesManager { get; set; }

    // Sprite components
    public Sprite Sprite { get; protected set; }
    public SpriteVfxs SpriteVfxs { get; protected set; }
    public FrameSprite FrameSprite { get; protected set; }

    // Visual components
    public Palette Palette { get; protected set; }
    public ColorShader ColorShader { get; protected set; }
    public LightSource LightSource { get; protected set; }
    public BloomSource BloomSource { get; protected set; }
    public DeathHandler DeathHandler { get; protected set; }
    public Paralax Paralax { get; protected set; }
    public VfxEmitter VfxEmitter { get; protected set; }
    public VfxAnimation VfxAnimation { get; protected set; }
    public LayerId LayerId { get; protected set; }
    public int DrawOrder { get; protected set; } // Draw priority inside a layer

    // Combat components
    public Alignment Alignment { get; protected set; }
    public DamageDealer DamageDealer { get; protected set; }
    public DamageTaker DamageTaker { get; protected set; }
    public KnockbackReceiver KnockbackReceiver { get; protected set; }

    // Control components (player/AI)
    public PlayerControl PlayerControl { get; protected set; }
    public AiControl AiControl { get; protected set; }
    public TargetPool TargetPool { get; set; }
    public StateManager StateManager { get; protected set; }

    // Item components
    public ItemGetter ItemGetter { get; protected set; }
    public ItemPrice ItemPrice { get; protected set; }
    public List<ResourceItemStats> ResourceItemStatsList { get; protected set; }
    public EquipmentItemStats EquipmentItemStats { get; protected set; }
    public ItemDropper ItemDropper { get; protected set; }
    public EquipmentHolder EquipmentHolder { get; protected set; }

    // Time components
    public FrameHandler FrameHandler { get; protected set; }

    // Systems
    public Physics Physics { get; protected set; } // TODO - ARCHITECTURE - MMDB: Physics system should only handle solid objects, pushing, carrying, etc. Make a Movement system for simple movement. Enforce solidBehavior for physics then

    // Shooting components
    public Shooter Shooter { get; set; }
    public Shooter SecondaryShooter { get; set; }
    public Shooter SuperShooter { get; set; }
    public ChargeManager ChargeManager { get; protected set; }
    public WeaponManager WeaponManager { get; protected set; }
    public ShotProperties ShotProperties { get; protected set; }

    // Transition Controller
    public TransitionController TransitionController { get; protected set; }

    // Menus
    public MenuItem MenuItem { get; protected set; }

    protected Entity()
    {
        EntityManager.ComponentEnforcerCheckingList.Add(this);
    }

    public void AssignEntityKind(EntityKind entityNewKind)
    {
        if (entityNewKind == EntityKind.None)
            return;
#if DEBUG
        if (EntityKind != EntityKind.None)
            Debugger.Break(); // EntityKind already assigned, this should not happen
#endif
        EntityManager.RemoveEntityFromSubList(this);
        EntityKind = entityNewKind;
        EntityManager.AddEntityToSubList(this);
    }

    public void Update()
    {
#if DEBUG
        if (CodeBreaker.UpdateBreakEntityName == GetType().Name)
            Debugger.Break();
#endif
        CustomUpdate();

        // Special Components
        EquipmentHolder?.Update();
        WeaponManager?.Update();
        ChargeManager?.Update();
        ItemGetter?.Update();
        // Control and State
        FrameHandler.CheckDurationEnd();
        PlayerControl?.Update();
        AiControl?.Update();
        StateManager?.Update();
        // Physics
        // TODO: 1st: Calculate each object's intended origin and destiny.
        // TODO: 2nd: Check for collisions and resolve them (Including pushing and carrying).
        Physics?.Update();
        LinkedEntitiesManager?.UpdatePositions();
        // State Pos Processing
        StateManager?.PostProcessing();
        // Visual Components
        Paralax?.Update();
        VfxEmitter?.Update();
        VfxAnimation?.Update();

        Updated = true;
    }

    protected virtual void CustomUpdate() { }

    public void Draw() // TODO - ARCHITECTURE: Simplify this method // TODO - DEBUG: Show total calls per frame and list of entities drawn
    {
#if DEBUG
        if (CodeBreaker.DrawBreakEntityName == GetType().Name)
            Debugger.Break();
#endif
        if (Sprite == null)
            return;
        if (Sprite.IsVisible == false)
            return;
        if (DamageTaker?.IsFlickering() == true)
            return;

        FrameSprite?.Draw();

        if (!BloomManager.DrawingBloom)
        {
            if (Palette != null || ColorShader != null)
                Video.SwitchSpriteSortMode(SpriteSortMode.Immediate);
            else
                Video.SwitchSpriteSortMode(SpriteSortMode.Deferred);
        }

        Palette?.SetPalette();
        ColorShader?.Set();
        Sprite?.Draw();
        PaletteManager.ResetPalette();
        ColorShaderManager.Reset();
    }
}

public enum EntityKind // Also works as a draw order
{
    None,
    // Platformer entities
    Paralax,
    Background,
    Decoration,
    DecorationVfx,
    Gimmick,
    Item,
    Enemy,
    EnemyEffect,
    Boss,
    Player,
    PlayerShot,
    EnemyShot,
    Vfx,
    // Shared entities
    Menu,
    StageEditing
}