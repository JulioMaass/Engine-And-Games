using Engine.ECS.Components.CombatHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Helpers;
using Engine.Managers;
using Engine.Managers.Graphics;
using Engine.Types;
using Microsoft.Xna.Framework;

namespace Engine.ECS.Components.PositionHandling;

public class CollisionBox : Component
{
    public IntVector2 Size { get; private set; }
    public IntVector2 Origin { get; private set; }
    public BodyType BodyType { get; set; } = BodyType.Vulnerable;

    // Mask (distance from origin to sides of the collision box)
    public int MaskTop { get; private set; }
    public int MaskBottom { get; private set; }
    public int MaskLeft { get; private set; }
    public int MaskRight { get; private set; }
    public IntRectangle CurrentHitbox => new(Origin, Size); // Readable as a rectangle for easy comparisons
    public IntRectangle DefaultHitbox { get; }
    public bool Dynamic { get; set; } // If true, hitbox size/origin can be changed during gameplay (e.g., Mega Man slide)

    public CoordinateType CoordinateType { get; set; } // TODO - ARCHITECTURE: Both collisions and visuals should take this into account. Marge and put into a component that makes sense (Position?)

    public CollisionBox(Entity owner, int w = 0, int h = 0, int ox = 0, int oy = 0)
    {
        Owner = owner;
        Size = IntVector2.New(w, h);
        Origin = IntVector2.New(ox, oy);
        DefaultHitbox = new IntRectangle(ox, oy, w, h);
        CalculateMask();
    }

    private void CalculateMask()
    {
        MaskTop = Origin.Y;
        MaskBottom = Size.Height - Origin.Y;
        MaskLeft = Origin.X;
        MaskRight = Size.Width - Origin.X;
    }

    public void SetNewHitboxIfFree(IntRectangle newHitbox)
    {
        if (newHitbox == Owner.CollisionBox.CurrentHitbox)
            return;

        var oldHitbox = Owner.CollisionBox.CurrentHitbox;
        Owner.CollisionBox.Resize(newHitbox);
        if (Owner.Physics.SolidCollisionChecking.IsCollidingWithSolid())
            Owner.CollisionBox.Resize(oldHitbox);
    }

    public bool DefaultHitboxCollidesWithSolid()
    {
        var currentHitbox = Owner.CollisionBox.CurrentHitbox;
        Owner.CollisionBox.Resize(DefaultHitbox);
        var collides = Owner.Physics.SolidCollisionChecking.IsCollidingWithSolid();
        Owner.CollisionBox.Resize(currentHitbox);
        return collides;
    }

    private void Resize(IntRectangle hitbox)
    {
        Size = IntVector2.New(hitbox.Width, hitbox.Height);
        Origin = IntVector2.New(hitbox.X, hitbox.Y);
        CalculateMask();
    }

    public IntRectangle GetCollisionRectangle() =>
        GetCollisionRectangleAt(Owner.Position.Pixel);

    public IntRectangle GetCollisionRectangleAt(IntVector2 position)
    {
        var x = position.X - MaskLeft;
        var y = position.Y - MaskTop;
        return new IntRectangle(x, y, Size.Width, Size.Height);
    }

    public IntRectangle GetTileCollisionRectangle() =>
        GetCollisionRectangle().RoundDownToTileCoordinate();

    public IntRectangle GetTileCollisionRectangleAt(IntVector2 position) =>
        GetCollisionRectangleAt(position).RoundDownToTileCoordinate();

    public IntVector2 GetEdgePosition(IntVector2 dir)
    {
        var rectangle = GetCollisionRectangle();
        return rectangle.GetEdgePosition(dir);
    }

    public void DrawBox()
    {
        var position = Owner.Position.Pixel - Origin;

        var color = BodyType == BodyType.Vulnerable
            ? GetAlignmentColor()
            : GetBodyTypeColor();

        Drawer.DrawRectangle(position, Size, color);
    }

    private Color GetAlignmentColor()
    {
        if (Owner.Alignment == null)
            return CustomColor.TransparentWhite;

        return Owner.Alignment.Type switch
        {
            AlignmentType.None => CustomColor.TransparentGray,
            AlignmentType.Friendly => CustomColor.TransparentGreen,
            AlignmentType.Neutral => CustomColor.TransparentYellow,
            AlignmentType.Hostile => CustomColor.TransparentRed,
            _ => CustomColor.TransparentWhite
        };
    }

    private Color GetBodyTypeColor()
    {
        return BodyType switch
        {
            BodyType.Vulnerable => CustomColor.TransparentRed,
            BodyType.Shield => CustomColor.TransparentBlue,
            BodyType.FrontShield => CustomColor.TransparentMagenta,
            _ => CustomColor.TransparentWhite
        };
    }

    public bool CollidesWithEntityPixel(Entity entity)
    {
        if (entity == null)
            return false;
        DebugMode.EntityCollisionCounter++;
        return HighPerformanceCollisionTest(entity);
    }

    private bool HighPerformanceCollisionTest(Entity entity) // Avoids using objects and function calls
    {
        // Current entity bounds
        var pos1 = Owner.Position.Pixel;
        var left1 = pos1.X - MaskLeft;
        var right1 = pos1.X + MaskRight - 1;
        var top1 = pos1.Y - MaskTop;
        var bottom1 = pos1.Y + MaskBottom - 1;
        // Other entity bounds
        var pos2 = entity.Position.Pixel;
        var collisionBox = entity.CollisionBox;
        var left2 = pos2.X - collisionBox.MaskLeft;
        var right2 = pos2.X + collisionBox.MaskRight - 1;
        var top2 = pos2.Y - collisionBox.MaskTop;
        var bottom2 = pos2.Y + collisionBox.MaskBottom - 1;
        // Collision check
        return left1 <= right2 &&
               right1 >= left2 &&
               top1 <= bottom2 &&
               bottom1 >= top2;
    }

    public bool BodyTypeDealsDamage() =>
        BodyType != BodyType.Bypass;

    public bool BodyTypeGetsDamaged() =>
        BodyType != BodyType.Bypass && BodyType != BodyType.Invincible;
}

public enum BodyType
{
    Vulnerable,
    Shield,
    FrontShield,
    Bypass,
    Invincible
}

public enum CoordinateType
{
    Undefined,
    Game,
    Hud
}