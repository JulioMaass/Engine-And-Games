using Engine.ECS.Entities;
using Engine.GameSpecific;
using Engine.Helpers;
using Engine.Main;
using Engine.Managers.Graphics;
using Engine.Managers.StageEditing.Tools;
using Engine.Types;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Engine.Managers.StageEditing.Modes;

public class StageEditorEntityMode : StageEditorMode
{
    public override Input.Button Shortcut { get; } = Input.EntityMode;
    public List<Type> EntityNames { get; } = new();
    public Type SelectedEntity { get; set; }

    public StageEditorEntityMode()
    {
        AvailableTools = new List<StageEditorTool>
        {
            new StageEditorPlaceEntityTool(),
            new StageEditorEditEntityTool(),
        };
        GenerateEntityList();
    }

    private void GenerateEntityList()
    {
        foreach (var entityType in GameManager.GameSpecificSettings.EditorEntityTypes)
            EntityNames.Add(entityType);
        SelectedEntity = EntityNames[0];
    }

    public override void Run()
    {
        EntityPlaceToolMenuClick();
        UpdateEntitySelectedViaShortcut();
        CheckToToggleTools();
        CurrentTool?.Run();
    }

    private void EntityPlaceToolMenuClick()
    {
        if (!Input.ClickedOnEditingMenu())
            return;
        if (!Input.MouseLeftPressed)
            return;

        var entityPosition = Input.MousePositionOnMenu().RoundDownToTileCoordinate() / Settings.EditingMenuScale;
        var entityIndex = entityPosition.Y * Settings.EditingMenuSizeInTiles.X + entityPosition.X;
        if (entityIndex < EntityNames.Count)
        {
            SelectedEntity = EntityNames[entityIndex];
            ToggleTool(typeof(StageEditorPlaceEntityTool));
        }
    }

    private void UpdateEntitySelectedViaShortcut()
    {
        Keys[] keys = { Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.D5, Keys.D6, Keys.D7, Keys.D8, Keys.D9, Keys.D0 };

        for (var i = 0; i < keys.Length; i++)
            if (Input.KeyboardState.IsKeyDown(keys[i]))
            {
                SetEntityOfIndex(i);
                ToggleTool(typeof(StageEditorPlaceEntityTool));
            }
    }

    private void SetEntityOfIndex(int index)
    {
        if (index < EntityNames.Count)
            SelectedEntity = EntityNames[index];
    }

    public override void Draw()
    {
        // Draw cross-hair on selected point
        var magnetMouse = (Input.MousePositionOnGame + Settings.TileSize / 4).RoundDownDivision(Settings.TileSize / 2) * (Settings.TileSize / 2);
        var crossHairRadius = Settings.TileSize / 2;
        // Horizontal line
        var position = magnetMouse - (crossHairRadius.X, 1);
        var size = IntVector2.New(Settings.TileSize.Width, 2);
        Drawer.DrawRectangle(position, size, CustomColor.TransparentRed);
        // Vertical line
        position = magnetMouse - (1, crossHairRadius.Y);
        size = IntVector2.New(2, Settings.TileSize.Height);
        Drawer.DrawRectangle(position, size, CustomColor.TransparentRed);

        CurrentTool?.Draw();
    }

    public override void DrawMenu()
    {
        // Draw entity list
        var backgroundSize = StageEditor.TileMode.CurrentTileset.Texture.GetSize() / Settings.TileSize;
        var entityListPosition = Settings.TileSize / 2;
        var entitiesPerRow = backgroundSize.X;
        for (var i = 0; i < EntityNames.Count; i++)
        {
            var entityPosition = entityListPosition + (i % entitiesPerRow * Settings.TileSize.X, i / entitiesPerRow * Settings.TileSize.Y);
            var entityPositionScaled = entityPosition * Settings.EditingMenuScale;
            CollectionManager.DrawEntityPreview(EntityNames[i], entityPositionScaled, CustomColor.White, 32);
        }
        // Draw entity selection
        var selectedEntityPosition = (EntityNames.IndexOf(SelectedEntity) % entitiesPerRow, EntityNames.IndexOf(SelectedEntity) / entitiesPerRow) * Settings.TileSize * Settings.EditingMenuScale;
        Drawer.DrawRectangleOutline(selectedEntityPosition, Settings.TileSize * Settings.EditingMenuScale, CustomColor.White);
    }

}
