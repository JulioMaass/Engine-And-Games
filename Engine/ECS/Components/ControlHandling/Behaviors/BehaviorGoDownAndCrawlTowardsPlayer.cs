using Engine.ECS.Entities;

namespace Engine.ECS.Components.ControlHandling.Behaviors;

public class SettingBehaviorGoDownAndCrawlTowardsPlayer : Behavior // TODO: Check what other behaviors should be named "SettingBehavior"
                                                                   // TODO: Check what behaviors, states, etc. should be on MMDB/CANDLE/shared
{
    public override void Action()
    {
        Owner.Speed.SetXSpeed(0);
        Owner.Speed.SetYSpeed(Owner.Speed.MoveSpeed);

        // Set crawl direction
        if (Owner.Position.Pixel.X < EntityManager.PlayerEntity.Position.Pixel.X)
            Owner.Speed.CrawlTurnDirection = -1;
        else
            Owner.Speed.CrawlTurnDirection = 1;
    }
}
