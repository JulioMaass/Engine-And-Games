using Engine.ECS.Components;
using Engine.ECS.Components.CombatHandling;
using Engine.ECS.Components.ControlHandling;
using Engine.ECS.Components.ItemsHandling;
using Engine.ECS.Components.MenuHandling;
using Engine.ECS.Components.PhysicsHandling;
using Engine.ECS.Components.PositionHandling;
using Engine.ECS.Components.ShootingHandling;
using Engine.ECS.Components.Spawning;
using Engine.ECS.Components.VisualsHandling;
using Engine.ECS.Systems.Physics;
using Engine.Managers.Graphics;
using Engine.Managers.StageHandling;
using System.Diagnostics;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global

namespace Engine.ECS.Entities.EntityCreation;

public abstract partial class Entity
{
    // Entity typing
    public EntityInstance EntityInstance { get; set; }
    public EntityKind EntityKind { get; protected set; }

    // Component enforcer
    public ComponentEnforcer ComponentEnforcer { get; protected set; }

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

    // Visual components
    public Sprite Sprite { get; protected set; }
    public Palette Palette { get; protected set; }
    public LightSource LightSource { get; protected set; }
    public BloomSource BloomSource { get; protected set; }
    public DeathHandler DeathHandler { get; protected set; }
    public Paralax Paralax { get; protected set; }
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
    public StateManager StateManager { get; protected set; }

    // Item components
    public ItemGetter ItemGetter { get; protected set; }
    public ItemPrice ItemPrice { get; protected set; }
    public ResourceItemStats ResourceItemStats { get; protected set; }
    public EquipmentItemStats EquipmentItemStats { get; protected set; }
    public ItemDropper ItemDropper { get; protected set; }
    public EquipmentHolder EquipmentHolder { get; protected set; }
    public StatsManager StatsManager { get; protected set; }

    // Time components
    public FrameHandler FrameHandler { get; protected set; }

    // Systems
    public Physics Physics { get; protected set; } // TODO - ARCHITECTURE - MMDB: Physics system should only handle solid objects, pushing, carrying, etc. Make a Movement system for simple movement. Enforce solidBehavior for physics then

    // Shooting components
    public Shooter Shooter { get; set; }
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
        if (EntityKind != EntityKind.None)
            Debugger.Break(); // EntityKind already assigned, this should not happen
        EntityManager.RemoveEntityFromSubList(this);
        EntityKind = entityNewKind;
        EntityManager.AddEntityToSubList(this);
    }

    public Entity GetCopy()
    {
        var type = GetType();
        return EntityManager.CreateEntity(type);
    }

    public void Update()
    {
        CustomUpdate();

        // Special Components
        ChargeManager?.Update();
        Paralax?.Update();
        // Control and State
        FrameHandler.CheckDurationEnd();
        PlayerControl?.Update();
        AiControl?.Update();
        StateManager.Update();
        // Physics
        // TODO: 1st: Calculate each object's intended origin and destiny.
        // TODO: 2nd: Check for collisions and resolve them (Including pushing and carrying).
        Physics?.Update();
        // State Pos Processing
        StateManager.PostProcessing();
    }

    protected virtual void CustomUpdate() { }

    public void Draw() // TODO - ARCHITECTURE: Simplify this method // TODO - DEBUG: Show total calls per frame and list of entities drawn
    {
        if (Sprite == null)
            return;
        if (Sprite.IsVisible == false)
            return;
        if (DamageTaker?.IsFlickering() == true)
            return;

        Palette?.SetPalette();
        Sprite?.Draw();
        PaletteManager.ResetPalette();
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
    Boss,
    Enemy,
    Player,
    EnemyShot,
    Vfx,
    PlayerShot,
    // Shared entities
    Menu,
    StageEditing
}