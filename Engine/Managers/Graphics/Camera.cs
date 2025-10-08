using Engine.ECS.Entities;
using Engine.ECS.Entities.EntityCreation;
using Engine.Main;
using Engine.Managers.Input;
using Engine.Managers.StageEditing;
using Engine.Managers.StageHandling;
using Engine.Types;
using Microsoft.Xna.Framework;
using System;

namespace Engine.Managers.Graphics;

public static class Camera
{
    public static Matrix Matrix;
    public static IntVector2 Panning { get; private set; }
    public static bool MovedXThisFrame { get; private set; }
    public static bool MovedYThisFrame { get; private set; }
    public static int LastXDir { get; private set; }
    public static int LastYDir { get; private set; }
    public static Vector2 FractionalPanning { get; private set; }
    public static IntVector2 ZoomPanning => GetZoomPanning();
    private static IntVector2 TransitionStartingPoint { get; set; }
    public static IntVector2 ZoomScale { get; private set; }
    public static IntVector2 CameraCenter => Panning + Settings.ScreenSize / 2;
    public static IntVector2 FullScreenOffset { get; private set; }
    public static IntRectangle DrawScreenLimits { get; private set; } // Used to avoid drawing things outside the screen

    public static void Initialize()
    {
        ZoomIn();
    }

    public static void Update()
    {
        if (!StageEditor.IsOn)
        {
            if (StageManager.IsTransitioning)
                Transition();
            else
                UpdatePanning();
        }
        else
            EditorPanning();
    }

    public static void MatrixUpdate()
    {
        Matrix = Matrix.CreateTranslation(-ZoomPanning.X, -ZoomPanning.Y, 0);
    }

    public static void ZoomOut()
    {
        if (ZoomScale == (1, 1))
            return;
        ZoomScale = (1, 1);

        var zoomSize = Settings.ScreenScaledSize / ZoomScale;
        Video.UpdateScreenRender(zoomSize);
        UpdateDrawScreenLimits();
    }

    public static void ZoomIn()
    {
        if (ZoomScale == Settings.ScreenScale)
            return;
        ZoomScale = Settings.ScreenScale;

        var zoomSize = Settings.ScreenScaledSize / ZoomScale;
        Video.UpdateScreenRender(zoomSize);
        UpdateDrawScreenLimits();
    }

    public static void UpdatePanning()
    {
        FocusOnPlayer();
    }

    private static IntVector2 GetZoomPanning()
    {
        if (ZoomScale == (1, 1))
            return Panning - Settings.ZoomOutOffset;
        return Panning;
    }

    public static void FocusOnPlayer()
    {
        if (EntityManager.PlayerEntity == null)
            return;

        FocusOnEntity(EntityManager.PlayerEntity);
    }

    private static void FocusOnEntity(Entity entity)
    {
        var oldPanning = Panning;
        var oldFractionalPanning = FractionalPanning;

        var min = StageManager.CurrentRoom.PositionInPixels;
        var max = StageManager.CurrentRoom.PositionInPixels + StageManager.CurrentRoom.SizeInPixels - Settings.ScreenSize;
        var position = entity.Position.Pixel - Settings.ScreenSize / 2;
        position = Vector2.Clamp(position, min, max);
        SetPanning(position);

        // Update last direction (used for midpoint rounding - when camera is on a .5 position)
        if (Panning.X != oldPanning.X)
            LastXDir = Math.Sign(Panning.X - oldPanning.X);
        if (Panning.Y != oldPanning.Y)
            LastYDir = Math.Sign(Panning.Y - oldPanning.Y);

        var realPosition = entity.Position.Pixel + entity.Position.Fraction;
        FractionalPanning = realPosition - (Vector2)Settings.ScreenSize / 2;
        FractionalPanning = Vector2.Clamp(FractionalPanning, min, max);
        MovedXThisFrame = oldFractionalPanning.X != FractionalPanning.X;
        MovedYThisFrame = oldFractionalPanning.Y != FractionalPanning.Y;
    }

    private static void SetPanning(IntVector2 panningPosition)
    {
        Panning = panningPosition;
        UpdateDrawScreenLimits();
    }

    public static void SaveTransitionReferencePoints()
    {
        TransitionStartingPoint = Panning;
    }

    private static void Transition()
    {
        // TODO: Fix transition issue
        var transitionTotalDistance = Settings.RoomSizeInPixels * StageManager.TransitionDirection;
        var transitionDistance = transitionTotalDistance / StageManager.TransitionFrames * StageManager.TransitionFrame;
        var panning = TransitionStartingPoint + transitionDistance;
        SetPanning(panning);
        FractionalPanning = panning;
    }

    private static void EditorPanning()
    {
        if (!EditorInput.Panning.Holding)
            return;
        if (!MouseHandler.MouseLeftHold && !Settings.LaptopModeIsOn)
            return;

        SetPanning(Panning - MouseHandler.GetMouseMovement());
    }

    public static void UpdateFullscreenOffset()
    {
        var currentWindowSize = GameManager.Game.Window.ClientBounds.Size;
        FullScreenOffset = (currentWindowSize - Settings.ScreenScaledSize) / 2;
    }

    public static IntRectangle GetSpawnScreenLimits()
    {
        return StageManager.IsTransitioning
            ? new IntRectangle(TransitionStartingPoint + Settings.RoomSizeInPixels * StageManager.TransitionDirection, Settings.ScreenSize)
            : new IntRectangle(Panning, Settings.ScreenSize);
    }

    public static IntRectangle GetSpawnScreenLimitsWithBorder(IntVector2 borderSize)
    {
        var screenLimits = GetSpawnScreenLimits();
        return new IntRectangle(screenLimits.Position - borderSize, screenLimits.Size + borderSize * 2);
    }

    private static void UpdateDrawScreenLimits()
    {
        var screenLimits = new IntRectangle(Panning, Settings.ScreenSize);
        var currentZoomOutOffset = ZoomScale == (1, 1) ? Settings.ZoomOutOffset : IntVector2.Zero;
        DrawScreenLimits = new IntRectangle(screenLimits.Position - currentZoomOutOffset, screenLimits.Size + currentZoomOutOffset * 2);
    }
}
