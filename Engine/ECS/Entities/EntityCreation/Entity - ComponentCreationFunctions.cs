using Engine.ECS.Components.CombatHandling;
using Engine.ECS.Components.ControlHandling;
using Engine.ECS.Components.ControlHandling.Behaviors;
using Engine.ECS.Components.ControlHandling.Conditions;
using Engine.ECS.Components.ControlHandling.States;
using Engine.ECS.Components.ItemsHandling;
using Engine.ECS.Components.PhysicsHandling;
using Engine.ECS.Components.VisualsHandling;
using Engine.Helpers;
using Engine.Managers;
using Engine.Managers.Graphics;
using Engine.Managers.StageHandling;
using Engine.Types;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable MemberCanBePrivate.Global

namespace Engine.ECS.Entities.EntityCreation;

public abstract partial class Entity
{
    // Basic
    public void AddBasicComponents()
    {
        AddPosition();
        AddSpeed();
        AddFrameCounter();
        SpawnManager = new(this);
        ComponentEnforcer = new(this);
        LayerId = LayerId.ForegroundTiles;
    }

    // Position
    public void AddPosition(int x = 0, int y = 0)
    {
        Position = new(this, x, y);
    }

    // Direction
    public void AddMoveDirection(int angle = 0)
    {
        MoveDirection = new(this, angle);
    }

    public void AddDirectionAndMove(int angle)
    {
        MoveDirection ??= new(this, angle);
        MoveDirection.Angle = angle;
        Speed.SetMoveSpeedToCurrentDirection();
    }

    public void AddShootDirection(int angle = 0)
    {
        ShootDirection = new(this, angle);
    }


    // Collision Box
    protected void AddCollisionBox(int w, int h, int ox = 0, int oy = 0)
    {
        CollisionBox = new(this, w, h, ox, oy);
    }

    protected void AddFullCollisionBox()
    {
        var w = Sprite.FinalSize.Width;
        var h = Sprite.FinalSize.Height;
        var ox = Sprite.FinalOrigin.X;
        var oy = Sprite.FinalOrigin.Y;
        AddCollisionBox(w, h, ox, oy);
    }

    public void AddFlippedFullCollisionBox()
    {
        var w = Sprite.FinalSize.Width;
        var h = Sprite.FinalSize.Height;
        var ox = Sprite.FinalSize.Width - Sprite.FinalOrigin.X;
        var oy = Sprite.FinalOrigin.Y;
        AddCollisionBox(w, h, ox, oy);
    }

    public void AddCenteredCollisionBox(int w, int h = 0)
    {
        if (h == 0) h = w;
        AddCollisionBox(w, h, w / 2, h / 2);
    }

    public void AddCenteredCollisionBox()
    {
        var w = Sprite.FinalSize.Width;
        var h = Sprite.FinalSize.Height;
        AddCenteredCollisionBox(w, h);
    }

    public void AddCenteredOutlinedCollisionBox()
    {
        var w = Sprite.FinalSize.Width - 2;
        var h = Sprite.FinalSize.Height - 2;
        AddCollisionBox(w, h, w / 2, h / 2);
    }

    // Facing
    public void AddFacing(int x = 1)
    {
        Facing = new(this, x);
    }

    // Speed
    public void AddSpeed(float x, float y = 0)
    {
        Speed = new(this);
        Speed.SetSpeed(x, y);
    }

    public void AddSpeed()
    {
        Speed ??= new(this);
    }

    public void AddPhysics()
    {
        Physics = new(this);
    }

    public void AddMoveSpeed(float speed)
    {
        Speed ??= new(this);
        Speed.MoveSpeed = speed;
    }

    public void AddRandomMoveSpeed(float minSpeed, float maxSpeed)
    {
        Speed ??= new(this);
        Speed.MoveSpeed = GetRandom.UnseededFloat(minSpeed, maxSpeed);
    }

    public void AddDashSpeed(float speed)
    {
        Speed ??= new(this);
        Speed.DashSpeed = speed;
    }

    public void AddJumpSpeed(float speed)
    {
        Speed ??= new(this);
        Speed.JumpSpeed = speed;
    }

    public void AddTurnSpeed(int speed)
    {
        Speed ??= new(this);
        Speed.TurnSpeed = speed;
    }

    // Gravity
    public void AddGravity(float force)
    {
        Gravity = new(this, force);
    }

    public void AddGravity()
    {
        Gravity = new(this);
    }

    // Solid Behavior
    public void AddSolidBehavior(SolidType solidType = SolidType.NotSolid,
        SolidInteractionType pushable = SolidInteractionType.GoThroughSolids,
        MomentumType momentumType = MomentumType.StopHitSpeedOnly)
    {
        SolidBehavior = new(this, solidType, pushable, momentumType);
    }

    public void AddTileDestructor(Strength strength)
    {
        TileDestructor = new(this, strength);
    }

    // Sprite
    public void AddSprite(string textureName, int w, int h, int ox, int oy, int sheetX = 0, int sheetY = 0)
    {
        var size = IntVector2.New(w, h);
        var origin = IntVector2.New(ox, oy);
        var spriteSheetOrigin = IntVector2.New(sheetX, sheetY);
        Sprite = new(this, textureName, size, origin, spriteSheetOrigin);
    }

    public void AddHudSprite(string textureName, int w, int h, int ox, int oy, int sheetX = 0, int sheetY = 0)
    {
        AddSprite(textureName, w, h, ox, oy, sheetX, sheetY);
        Sprite.HudSprite = true;
    }

    public void AddSpriteCenteredOrigin(string textureName, int w, int h = 0, int sheetX = 0, int sheetY = 0)
    {
        if (h == 0) h = w;
        AddSprite(textureName, w, h, w / 2, h / 2, sheetX, sheetY);
    }

    public void AddSpriteTopLeftOrigin(string textureName, int w = 0, int h = 0, int sheetX = 0, int sheetY = 0)
    {
        if (w == 0) w = Drawer.TextureDictionary[textureName].Width;
        if (h == 0) h = Drawer.TextureDictionary[textureName].Height;
        AddSprite(textureName, w, h, 0, 0, sheetX, sheetY);
    }

    public void AddSpriteFullImageCenteredOrigin(string textureName)
    {
        var w = Drawer.TextureDictionary[textureName].Width;
        var h = Drawer.TextureDictionary[textureName].Height;
        AddSprite(textureName, w, h, w / 2, h / 2);
    }

    public void AddSpriteVariation(int totalVariations, int variationOffset)
    {
        if (Sprite == null)
            throw new InvalidOperationException("Sprite must be initialized before adding variations.");
        Sprite.VariationOffset = GetRandom.UnseededInt(totalVariations) * variationOffset;
    }

    public void AddFrameSprite(string textureName, int borderWidth, int borderHeight)
    {
        FrameSprite = new(this, textureName, IntVector2.New(borderWidth, borderHeight));
    }

    // Palette
    public void AddPalette(string paletteName) =>
        Palette = new(this, Drawer.TextureDictionary[paletteName]);

    // Paralax
    public void AddParalax(LayerId layerId, Vector2 distancePercentage, Vector2 speed)
    {
        Paralax = new(this);
        LayerId = layerId;
        Paralax.DistancePercentage = distancePercentage;
        Paralax.Speed = speed;
        SpawnManager.Permanent = true;
    }

    public void AddParalax(LayerId layerId, (float X, float Y) distancePercentage, (float X, float Y) speed)
    {
        AddParalax(layerId, new Vector2(distancePercentage.X, distancePercentage.Y), new Vector2(speed.X, speed.Y));
    }

    // Light Source
    public void AddLightSource(IntVector2 intVector2, int radius1, int radius2) =>
        LightSource = new(this, intVector2, radius1, radius2);

    // Alignment
    public void AddAlignment(AlignmentType type) =>
        Alignment = new(this, type);

    // Damage Dealer
    public void AddDamageDealer(int damage, PiercingType piercingType = PiercingType.PierceAll) =>
        DamageDealer = new(this, damage, piercingType);

    // Damage Taker
    public void AddDamageTaker(int maxHealth) =>
        DamageTaker = new(this, maxHealth);

    // Player components
    public void AddPlayerComponents()
    {
        PlayerControl = new PlayerControl(this);
        SpawnManager.PersistsOnTransitions = true;
        SpawnManager.AutomaticSpawn = false;
    }

    // State
    public void AddStateManager() =>
        StateManager = new(this);

    public State NewState(State state = default, int spriteId = 0, int spriteSpeed = 1, int spriteFrames = 1,
        List<int> spritePattern = null)
    {
        state ??= new StateDefault("Default");
        state.Owner = this;

        // Set animation properties
        state.SpriteId = spriteId;
        state.SpriteSpeed = spriteSpeed;
        state.SpriteFrames = spriteFrames;
        state.SpritePattern = spritePattern;

        return state;
    }

    public void MakeStateDurationLoop(bool defaultLoop, params (State state, int duration)[] stateAndDuration)
    {
        if (defaultLoop)
            StateManager.CommandState(stateAndDuration[0].state);
        // First state loops from the last
        var firstState = stateAndDuration[0].state;
        var lastState = stateAndDuration[^1];
        firstState.AddStartCondition(new ConditionStateFrame(lastState.state, lastState.duration));
        // Other states loop from the previous one
        for (var i = 1; i < stateAndDuration.Length; i++)
            stateAndDuration[i].state.AddStartCondition(new ConditionPreviousState(stateAndDuration[i - 1].state));
        // Add durations
        for (var i = 0; i < stateAndDuration.Length - 1; i++)
            stateAndDuration[i].state.AddKeepCondition(new ConditionFrame(stateAndDuration[i].duration, ComparisonType.Less));
    }

    public Condition NewCondition(Condition condition)
    {
        condition.Owner = this;
        return condition;
    }

    public Condition GroupedConditions(params Condition[] conditions)
    {
        var mainCondition = conditions.First();
        mainCondition.Owner = this;
        foreach (var secondaryCondition in conditions.Skip(1))
        {
            mainCondition.AddCondition(secondaryCondition);
            secondaryCondition.Owner = this;
        }
        return mainCondition;
    }

    public Behavior NewBehavior(Behavior behavior)
    {
        behavior.Owner = this;
        return behavior;
    }

    public Behavior GroupedBehaviors(params Behavior[] behaviors)
    {
        var mainBehavior = behaviors.First();
        mainBehavior.Owner = this;
        foreach (var secondaryBehavior in behaviors.Skip(1))
        {
            mainBehavior.AddBehavior(secondaryBehavior);
            secondaryBehavior.Owner = this;
        }
        return mainBehavior;
    }

    public State NewStateWithStartUp(State state, int spriteId, int spriteSpeed, int spriteFrames,
        int startUpDuration, int startUpSpriteId, int startUpSpriteSpeed = 1, int startUpSpriteFrames = 1)
    {
        state ??= new StateDefault("Default");
        NewState(state, spriteId, spriteSpeed, spriteFrames);
        state.StartUpDuration = startUpDuration;
        state.StartUpSpriteId = startUpSpriteId;
        state.StartUpSpriteSpeed = startUpSpriteSpeed;
        state.StartUpSpriteFrames = startUpSpriteFrames;
        return state;
    }

    public State NewStateWithTimedPattern(State state, params (int id, int f)[] spriteTimedPattern)
    {
        // Turns _spriteTimedPattern into a normal _spritePattern
        var spritePattern = new List<int>();
        foreach (var (spriteId, frames) in spriteTimedPattern)
            for (var i = 0; i < frames; i++)
                spritePattern.Add(spriteId);

        return NewState(state, 0, 1, 1, spritePattern);
    }

    public void AddFillerStates()
    {
        // Create state manager if necessary and add empty states to lists
        StateManager ??= new(this);
        if (StateManager.AutomaticStatesList.Count == 0)
            StateManager.AutomaticStatesList.Add(NewState());
    }

    // Item
    public void AddItemComponents(ResourceType resource, int amount)
    {
        // Position components
        AddCenteredOutlinedCollisionBox();

        // Physics components
        AddSpeed();
        AddGravity();
        AddSolidBehavior(SolidType.NotSolid, SolidInteractionType.StopOnSolids);

        // Item components
        AddResourceItemStats(resource, amount);
    }


    // Item Getter
    public void AddItemGetter(int attractionRadius = 0)
    {
        ItemGetter = new(this, attractionRadius);
    }

    // Item Stats
    public void AddResourceItemStats(ResourceType resourceType, int amount, IncreaseKind increaseKind = IncreaseKind.Current)
    {
        ResourceItemStatsList ??= new();
        var resourceItemStats = new ResourceItemStats(this);
        resourceItemStats.Amount = amount;
        resourceItemStats.ResourceType = resourceType;
        resourceItemStats.IncreaseKind = increaseKind;
        ResourceItemStatsList.Add(resourceItemStats);
    }

    public void AddEquipmentItemStats(EquipKind kind)
    {
        EquipmentItemStats ??= new(this);
        EquipmentItemStats.EquipKind = kind;
        EquipmentItemStats.Stats = new Stats();
    }

    // Item Dropper
    public void AddItemDropper(Type itemType) // Always same item, 1 item
    {
        ItemDropper = new(this);
        ItemDropper.DropTable.Add(([itemType], 1));
    }

    public void AddItemDropper(params (Type itemType, int chance)[] dropTable) // Drop table chances, 1 item
    {
        ItemDropper = new(this);
        foreach (var (itemType, chance) in dropTable)
            ItemDropper.DropTable.Add(([itemType], chance));
    }

    public void AddItemDropper(Type type, int amount, int dropDistance) // Always same item, more quantity
    {
        ItemDropper = new(this);
        ItemDropper.DropDistance = dropDistance;
        var itemList = new List<Type>();
        for (var i = 0; i < amount; i++)
            itemList.Add(type);
        ItemDropper.DropTable.Add((itemList, 1));
    }

    public void AddItemDropper(int dropDistance, params (Type type, int amount)[] typeAndAmount) // Always same items, define quantity of each one
    {
        ItemDropper = new(this);
        ItemDropper.DropDistance = dropDistance;
        var itemList = new List<Type>();
        foreach (var (type, amount) in typeAndAmount)
            for (var i = 0; i < amount; i++)
                itemList.Add(type);
        ItemDropper.DropTable.Add((itemList, 1));
    }

    // Frame Counter
    public void AddFrameCounter(int entityDuration = 0, bool triggerDeathAtDurationEnd = false)
    {
        FrameHandler = new(this, entityDuration, triggerDeathAtDurationEnd);
    }

    // Death Animation
    public void AddDeathHandler(params Behavior[] deathBehaviors)
    {
        DeathHandler = new(this);
        foreach (var behavior in deathBehaviors)
            DeathHandler.AddBehavior(behavior);
    }

    // Custom value handler
    public void AddCustomValueHandler()
    {
        CustomValueHandler = new(this);
    }

    // VFX components
    public void AddVfxComponents(int duration = 0)
    {
        if (duration > 0)
        {
            AddFrameCounter(duration);
            SpawnManager.DespawnOnScreenExit = false;
        }
    }

    // Menu
    public void AddBasicMenuComponents()
    {
        EntityKind = EntityKind.Menu;
        AddBasicComponents();
        MenuItem = new(this);
    }

    // Pointed Label Menu
    public void AddPointedLabelMenuComponents()
    {
        AddBasicMenuComponents();

        MenuItem.Draw = () =>
        {
            Video.SpriteBatch.DrawString(Drawer.MegaManFont, MenuItem.Label, Position.Pixel, CustomColor.MegaManWhite);
        };

        MenuItem.OnSelectDraw = () =>
        {
            var cursorPosition = MenuManager.SelectedItem.Position.Pixel;
            Video.SpriteBatch.DrawString(Drawer.MegaManFont, ">", cursorPosition - (10, 0), Color.White);
        };
    }
}
