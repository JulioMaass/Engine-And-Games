using Engine.ECS.Components.ControlHandling.States;
using Engine.ECS.Entities.EntityCreation;
using Engine.Helpers;
using Engine.Managers.Graphics;
using Engine.Types;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Engine.ECS.Components.VisualsHandling;

public class Sprite : Component
{
    public Texture2D Texture { get; private set; }

    // Flipping
    public bool FlippedFacing => Owner.Facing.X == -1;
    public bool IsFlipped => OwnerState.GetFlipAnimation() != FlippedFacing && !AutoRotation; // XOR

    // Sizes
    public bool Resizable { get; set; }
    public IntVector2 Size { get; set; }
    public IntVector2 StretchedSize { get; set; }
    public IntVector2 FinalSize => StretchedSize == default ? Size : StretchedSize; // If StretchedSize is not set, use Size

    // Rotation
    public bool AutoRotation { get; set; }

    // Origin
    public IntVector2 Origin { get; set; }
    private IntVector2 StretchedOrigin => (Vector2)Origin * (Vector2)StretchedSize / (Vector2)Size; // If StretchedSize is not set, use Origin
    public IntVector2 FinalOrigin => StretchedSize == default ? Origin : StretchedOrigin;

    public IntVector2 SpriteSheetOrigin { get; set; } // Choose position for default/first sprite in sprite sheet
    public Color Color { get; set; } = CustomColor.White;
    private State OwnerState => Owner.StateManager.GetCurrentStateOrDefault();
    private IntVector2 OwnerPosition => Owner.Position.Pixel;
    public bool IsVisible { get; set; } = true;
    public bool HudSprite { get; set; } = false;
    public (int DirectionsAmount, int FrameOffset) DirectionOffset { get; set; } // Ex.: (8, 3) means that the sprite sheet has 8 directions, and each direction sheet starts 3 frames after the other
    public bool ApplyJitterCorrection { get; set; } // Only use for entities that move at speeds similar to player/camera speed, and don't have complex physics (carry/push) Ex.: Suzy moving at 1.5 speed, slow shots, etc
    public int VariationOffset { get; set; }

    public Sprite(Entity owner, string textureName, IntVector2 size, IntVector2 origin, IntVector2 spriteSheetOrigin = default)
    {
        Owner = owner;
        Texture = Drawer.TextureDictionary[textureName];
        Size = size;
        Origin = origin;
        SpriteSheetOrigin = spriteSheetOrigin;
    }

    public void Draw()
    {
        var spriteId = GetSpriteId();
        DrawId(spriteId);
    }

    public void DrawId(int spriteId, IntVector2 offset = default)
    {
        var position = GetPosition() + offset;
        var sourceRectangle = Drawer.GetSourceRectangleFromId(Texture, SpriteSheetOrigin, Size, spriteId);
        if (!HudSprite
            && !Camera.GetDrawScreenLimits().Overlaps(new IntRectangle(position, sourceRectangle.Size))
            && Owner.EntityKind != EntityKind.Paralax)
            return;
        if (Owner.Paralax != null && Owner.Paralax.Repeat != ParalaxRepeat.None)
            Owner.Paralax?.DrawRepetitions(sourceRectangle, position);
        else
        {
            var jitterCorrection = GetJitterCorrection();
            DrawSpriteTexture(sourceRectangle, position + jitterCorrection);
        }
    }

    private void DrawSpriteTexture(IntRectangle sourceRectangle, IntVector2 position)
    {
        var effects = SpriteEffects.None;
        if (IsFlipped)
            effects = SpriteEffects.FlipHorizontally;
        var destinationRectangle = new IntRectangle(position + FinalOrigin, FinalSize);
        Video.SpriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, CalculateSpriteColor(), GetRotation(), Origin, effects, Drawer.DefaultDepth);
    }

    private float GetRotation()
    {
        if (AutoRotation)
            return Owner.MoveDirection.Angle.GetAsRadian();
        return 0f;
    }

    private Color CalculateSpriteColor()
    {
        var color = Color;
        if (!BloomManager.DrawingBloom)
            return color;

        if (Owner.BloomSource != null)
        {
            var bloomOscillation = (float)Math.Sin(Owner.FrameHandler.CurrentFrame / 10f) / 40f;
            color *= Owner.BloomSource.Intensity + bloomOscillation;
        }
        else
            color = CustomColor.Black;
        return color;
    }

    private Vector2 GetJitterCorrection() // To avoid jitter when both camera and entity are moving at fractional speeds
    {
        var jitterCorrection = Vector2.Zero;
        if (ApplyJitterCorrection) // Ignore jitter correction if the camera is not moving
        {
            if (Camera.MovedXThisFrame)
            {
                jitterCorrection.X = Camera.Panning.X - Camera.FractionalPanning.X + Owner.Position.Fraction.X;
                jitterCorrection.X = Owner.Speed.X == 0 ? 0 : XMidpointRound(jitterCorrection.X);
            }
            if (Camera.MovedXThisFrame)
            {
                jitterCorrection.Y = Camera.Panning.Y - Camera.FractionalPanning.Y + Owner.Position.Fraction.Y;
                jitterCorrection.Y = Owner.Speed.Y == 0 ? 0 : YMidpointRound(jitterCorrection.Y);
            }
        }
        return jitterCorrection;
    }

    private float XMidpointRound(float fractionCorrectionX) // To fix bugs related to 0.5 values rounding
    {
        var isHalfwayXFraction = (fractionCorrectionX + 1) % 1 == 0.5f;
        if (isHalfwayXFraction)
        {
            // If entity position is halfway, undoes the correction
            if (Math.Abs(Owner.Position.Fraction.X) == 0.5f)
                fractionCorrectionX -= Owner.Position.Fraction.X;
            // If the camera is halfway, change correction according to the direction of the camera and entity
            else if (Math.Abs(Camera.Panning.X - Camera.FractionalPanning.X) == 0.5f)
                if (Camera.LastXDir == Math.Sign(Owner.Speed.X))
                    fractionCorrectionX += Camera.Panning.X - Camera.FractionalPanning.X;
                else
                    fractionCorrectionX -= Camera.Panning.X - Camera.FractionalPanning.X;
            // If still halfway round depending on camera direction
            if (Camera.LastXDir > 0)
                fractionCorrectionX = (float)Math.Round(fractionCorrectionX, MidpointRounding.ToNegativeInfinity);
            else
                fractionCorrectionX = (float)Math.Round(fractionCorrectionX, MidpointRounding.ToPositiveInfinity);
        }
        return fractionCorrectionX;
    }

    private float YMidpointRound(float fractionCorrectionY) // To fix bugs related to 0.5 values rounding
    {
        var isHalfwayYFraction = (fractionCorrectionY + 1) % 1 == 0.5f;
        if (isHalfwayYFraction)
        {
            // If entity position is halfway, undoes the correction
            if (Math.Abs(Owner.Position.Fraction.Y) == 0.5f)
                fractionCorrectionY -= Owner.Position.Fraction.Y;
            // If the camera is halfway, change correction according to the direction of the camera and entity
            else if (Math.Abs(Camera.Panning.Y - Camera.FractionalPanning.Y) == 0.5f)
                if (Camera.LastYDir == Math.Sign(Owner.Speed.Y))
                    fractionCorrectionY += Camera.Panning.Y - Camera.FractionalPanning.Y;
                else
                    fractionCorrectionY -= Camera.Panning.Y - Camera.FractionalPanning.Y;
            // If still halfway round depending on camera direction
            if (Camera.LastYDir > 0)
                fractionCorrectionY = (float)Math.Round(fractionCorrectionY, MidpointRounding.ToNegativeInfinity);
            else
                fractionCorrectionY = (float)Math.Round(fractionCorrectionY, MidpointRounding.ToPositiveInfinity);
        }
        return fractionCorrectionY;
    }

    public void DrawOrigin()
    {
        Drawer.DrawRectangle(OwnerPosition, (1, 1), CustomColor.White);
    }

    private IntVector2 GetPosition()
    {
        var position = OwnerPosition - FinalOrigin;
        if (Owner.Paralax != null)
            position += Owner.Paralax.GetParalaxOffset();
        if (IsFlipped)
            position.X = OwnerPosition.X + FinalOrigin.X - FinalSize.Width;
        return position + OwnerState.GetAnimationOffset();
    }

    private int GetSpriteId()
    {
        var spriteId = OwnerState.GetSpriteId();
        // apply special offsets (secondary state, direction, variation)
        if (Owner.StateManager.CurrentSecondaryState != null)
            spriteId += Owner.StateManager.CurrentSecondaryState.SpriteIdOffset;
        if (DirectionOffset.DirectionsAmount > 0)
            spriteId += Owner.MoveDirection.GetRoundedIndex(DirectionOffset.DirectionsAmount) * DirectionOffset.FrameOffset;
        if (VariationOffset > 0)
            spriteId += VariationOffset;
        return spriteId;
    }

    public void SetColor(Color color) =>
        Color = color;

    public void SetColor(int r, int g, int b, int a) =>
        Color = new Color(r, g, b, a);
}