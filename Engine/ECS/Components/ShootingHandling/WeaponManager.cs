using Engine.ECS.Entities;
using Engine.ECS.Entities.EntityCreation;
using System.Collections.Generic;
using System.Linq;

namespace Engine.ECS.Components.ShootingHandling;

public class WeaponManager : Component
{
    public int ShotLimit { get; set; }
    public List<Entity> CountedShots { get; } = new();

    public WeaponManager(Entity owner)
    {
        Owner = owner;
    }

    public int GetShotCount()
    {
        var shotCount = 0;
        foreach (var shot in CountedShots.ToList())
        {
            if (!EntityManager.GetAllEntities().Contains(shot))
                CountedShots.Remove(shot);
            else
                shotCount += shot.ShotProperties.ShotScreenPrice;
        }
        return shotCount;
    }
}