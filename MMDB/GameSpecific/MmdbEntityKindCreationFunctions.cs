using Engine.ECS.Components.CombatHandling;
using Engine.ECS.Components.ControlHandling.Behaviors;
using Engine.ECS.Components.ControlHandling.Behaviors.Death;
using Engine.ECS.Components.MenuHandling;
using Engine.ECS.Components.PhysicsHandling;
using Engine.Helpers;
using Engine.Managers.Graphics;
using Engine.Types;
using MMDB.GameSpecific.Behaviors;
using MMDB.GameSpecific.Entities;
using MMDB.GameSpecific.Entities.Items;

namespace MMDB.GameSpecific;

public abstract class Entity : Engine.ECS.Entities.EntityCreation.Entity
{
    public void AddMmdbPlayerComponents()
    {
        AddPlayerComponents();
        AddCollisionBox(16, 22, 8, 6);

        AddGravity();
        AddSolidBehavior(SolidType.NotSolid, SolidInteractionType.StopOnSolids);

        AddAlignment(AlignmentType.Friendly);
        AddDamageTaker(28);

        AddItemGetter();
        AddDeathHandler(new BehaviorPlayerStartDeathCounter(), new BehaviorRobotMasterExplosion());
    }

    public void AddMegaManComponents()
    {
        RelativePosition = new(this);
        RelativePosition.ShotCreation = IntVector2.New(16, 5);
        WeaponManager = new(this);
        WeaponManager.ShotLimit = 3;
    }

    public void AddGimmickComponents(float gravity, SolidType solidType, SolidInteractionType solidInteractionType = SolidInteractionType.GoThroughSolids)
    {
        // Position components
        AddFullCollisionBox();

        // Physics components
        AddSpeed();
        if (gravity > 0)
            AddGravity(gravity);
        AddSolidBehavior(solidType, solidInteractionType);
    }

    public void AddPlayerShotComponents(int damage, PiercingType piercingType, int screenPrice)
    {
        AddSpeed();

        // Combat components
        AddAlignment(AlignmentType.Friendly);
        AddDamageDealer(damage, piercingType);
        ShotProperties = new(this);
        ShotProperties.ShotScreenPrice = screenPrice;
    }

    public void AddMmdbEnemyComponents(int hp, int damage)
    {
        // Control components
        AiControl = new(this);

        // Position components
        AddCenteredOutlinedCollisionBox();
        AddSpeed();

        // Combat components
        AddAlignment(AlignmentType.Hostile);
        AddDamageTaker(hp);
        AddDamageDealer(damage);
        DamageDealer.SetPiecingType(PiercingType.PierceAll);

        // Item components
        AddItemDropper((typeof(HpSmall), 1));

        // Visuals
        AddDeathHandler(new BehaviorCreateEntity(typeof(ExplosionSmall)));
    }

    public void AddMmdbEnemyShotComponents(int damage)
    {
        // Position components
        AddCenteredOutlinedCollisionBox();
        AddSpeed();

        // Combat components
        AddAlignment(AlignmentType.Hostile);
        AddDamageDealer(damage);
    }

    // Menu Items
    // Stage Menu Item
    public void AddStageMenuItemComponents()
    {
        AddBasicMenuComponents();
        AddHudSprite("StageSelection", 48, 48, 0, 0);
        MenuItem.OnSelectDraw = () =>
            Sprite.DrawId(0);
    }

    // Weapon Menu Item
    public void AddWeaponMenuItemComponents()
    {
        AddBasicMenuComponents();
        var labelOffset = (24, 1);

        MenuItem.Draw = () =>
        {
            Sprite.DrawId(4);
            Sprite.DrawId(5, (112, 0));

            var cursorPosition = Position.Pixel + labelOffset;
            Video.SpriteBatch.DrawString(Drawer.MegaManFont, MenuItem.Label, cursorPosition, CustomColor.MegaManWhite);
        };

        MenuItem.OnSelectDraw = () =>
        {
            Sprite.DrawId(0);
            Sprite.DrawId(1, (112, 0));

            var cursorPosition = Position.Pixel + labelOffset;
            Video.SpriteBatch.DrawString(Drawer.MegaManFont, MenuItem.Label, cursorPosition,
                CustomColor.MegaManWeaponSelect);
        };
    }
}