using Engine.ECS.Components.ControlHandling.Behaviors;
using Engine.ECS.Entities;
using Engine.ECS.Entities.EntityCreation;
using Engine.Managers;
using Engine.Managers.Graphics;
using Engine.Managers.StageHandling;
using Engine.Types;
using System.Collections.Generic;

namespace Engine.ECS.Components.Spawning;

public class SpawnManager : Component
{
    public Room Room { get; } // Which room the entity belongs to
    public bool PersistsOnTransitions { get; set; }
    public bool AutomaticSpawn { get; set; } = true;
    public bool EarlySpawn { get; set; } // Spawns on room enter instead of scrolling // TODO: Early spawners should deactivate physics if out of screen
    public List<Behavior> SpawnBehaviors { get; } = new();
    public bool DespawnOnScreenExit { get; set; } = true;
    public int DespawnDelay { get; set; }
    public int DespawnCounter { get; set; }
    public bool Permanent { get; set; } // Ignores spawn/despawn rules (Paralax, etc?)

    public SpawnManager(Entity owner)
    {
        Owner = owner;
        Room = StageManager.CurrentRoom;
    }

    public void CheckToDespawn()
    {
        if (Permanent)
            return;
        DespawnCounter++;
        var screenLimits = Camera.GetSpawnScreenLimits();
        var collisionRectangle = GetDespawnRectangle();
        if (!DespawnOnScreenExit || EarlySpawn)
            return;

        DebugMode.DespawnCollisionCounter++;
        if (!collisionRectangle.Overlaps(screenLimits))
        {
            if (DespawnCounter > DespawnDelay)
                EntityManager.DeleteEntity(Owner);
        }
        else
            DespawnCounter = 0;
    }

    private IntRectangle GetDespawnRectangle()
    {
        var entityRectangle = new IntRectangle(Owner.Position.Pixel - Owner.Sprite.FinalOrigin, Owner.Sprite.FinalSize);

        // Shield rectangle
        IntRectangle? shieldRectangle = null;
        var shield = Owner.LinkedEntitiesManager?.ShieldEntity;
        if (shield == null)
            return entityRectangle;
        shieldRectangle = new IntRectangle(shield.Position.Pixel - shield.Sprite.FinalOrigin, shield.Sprite.FinalSize);

        // Combine entity and shield rectangles
        return IntRectangle.GetRectangleCombination(entityRectangle, shieldRectangle.Value);
    }

    public void AddSpawnBehavior(Behavior spawnBehavior)
    {
        spawnBehavior.Owner = Owner;
        SpawnBehaviors.Add(spawnBehavior);
    }
}
