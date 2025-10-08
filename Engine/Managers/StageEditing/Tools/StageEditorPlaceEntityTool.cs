using Engine.ECS.Entities;
using Engine.ECS.Entities.Shared;
using Engine.Helpers;
using Engine.Main;
using Engine.Managers.Input;
using Engine.Managers.StageHandling;
using Engine.Types;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;

namespace Engine.Managers.StageEditing.Tools;

class StageEditorPlaceEntityTool : StageEditorTool
{
    public override MouseCursor MouseCursor { get; } = MouseCursor.Crosshair;
    public override Button Shortcut { get; } = EditorInput.EntityPlaceTool;

    public override void Run()
    {
        EntityPlaceToolGameScreenClick();
    }

    private void EntityPlaceToolGameScreenClick()
    {
        if (!MouseHandler.ClickedOnGameScreen())
            return;
        if (StageEditor.SelectedRoom == null)
            return;

        var magnetMousePosition = (MouseHandler.MousePositionOnGame + Settings.TileSize / 4).RoundDownDivision(Settings.TileSize / 2) * (Settings.TileSize / 2);

        // Create entity
        if (MouseHandler.MouseLeftPressed)
            CreateEntityInstance(StageEditor.EntityMode.SelectedEntity, magnetMousePosition);

        // Delete entity
        if (MouseHandler.MouseRightPressed)
            DeleteEntityLayoutAt(magnetMousePosition);
    }

    private void CreateEntityInstance(Type entityType, IntVector2 position)
    {
        var entityLayout = StageEditor.SelectedRoom.GetEntityLayout();
        var positionOnRoom = position - StageEditor.SelectedRoom.PositionInPixels;
        var entityInstance = new EntityInstance(entityLayout, entityType, positionOnRoom);
        HandlePlacingRespawnPoint();
        entityLayout.List.Add(entityInstance);
        HandlePlacingPlayer(position);
    }

    private void HandlePlacingPlayer(IntVector2 magnetMousePosition)
    {
        if (!GameManager.GameSpecificSettings.PlayerTypes.Contains(StageEditor.EntityMode.SelectedEntity)) return;

        // Delete player entity layout if it exists
        DeleteEntityLayoutAt(StageEditor.CurrentStage.PlayerStartingPosition);

        // Update player starting position if entity is player
        StageEditor.CurrentStage.PlayerStartingPosition = magnetMousePosition;

        // Delete player entity if it exists
        EntityManager.DeleteEntity(EntityManager.PlayerEntity);
    }

    private void HandlePlacingRespawnPoint()
    {
        if (StageEditor.EntityMode.SelectedEntity != typeof(RespawnPoint)) return;

        var currentRespawnPoint = StageEditor.SelectedRoom.GetEntityLayout().List
            .FirstOrDefault(entityInstance => entityInstance.EntityType == typeof(RespawnPoint));
        if (currentRespawnPoint != null)
            DeleteEntityLayoutAt(currentRespawnPoint.PositionAbsolute);
    }

    private void DeleteEntityLayoutAt(IntVector2 positionAbsolute)
    {
        var positionRoom = StageEditor.CurrentStage.GetRoomAtPixel(positionAbsolute);
        var entityInstance = positionRoom?.GetEntityLayout().GetEntityInstanceAt(positionAbsolute);

        if (entityInstance == null)
            return;

        positionRoom.GetEntityLayout().List.Remove(entityInstance);
        EntityManager.DeleteEntity(entityInstance.SpawnedEntity);
    }

    public override void Draw()
    {
        var magnetMouse = (MouseHandler.MousePositionOnGame + Settings.TileSize / 4).RoundDownDivision(Settings.TileSize / 2) * (Settings.TileSize / 2);
        CollectionManager.DrawEntityPreview(StageEditor.EntityMode.SelectedEntity, magnetMouse, CustomColor.TransparentWhite);
    }
}
