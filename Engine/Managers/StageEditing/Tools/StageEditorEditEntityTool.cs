using Engine.ECS.Entities;
using Engine.Helpers;
using Engine.Main;
using Engine.Managers.Graphics;
using Engine.Managers.Input;
using Engine.Managers.StageHandling;
using Engine.Types;
using Microsoft.Xna.Framework.Input;
using System.Linq;

namespace Engine.Managers.StageEditing.Tools;

class StageEditorEditEntityTool : StageEditorTool
{
    public override MouseCursor MouseCursor { get; } = MouseCursor.Hand;
    public override Button Shortcut { get; } = EditorInput.EntityEditTool;
    private EntityInstance SelectedEntityInstance { get; set; }
    private int SelectedCustomValueIndex { get; set; }

    public override void Run()
    {
        CheckToSelectEntity();
        EditEntityValues();
        CheckToSelectCustomValue();
    }

    private void CheckToSelectEntity()
    {
        if (!MouseHandler.ClickedOnGameScreen())
            return;

        var magnetMousePosition = (MouseHandler.MousePositionOnGame + Settings.TileSize / 4).RoundDownDivision(Settings.TileSize / 2) * (Settings.TileSize / 2);
        var positionRoom = StageEditor.CurrentStage.GetRoomAtPixel(magnetMousePosition);
        var entityInstance = positionRoom?.GetEntityLayout().GetEntityInstanceAt(magnetMousePosition);
        SelectedEntityInstance = entityInstance;
        if (entityInstance?.CustomValues.Count == 0)
            SelectedEntityInstance = null;
        if (SelectedEntityInstance == null)
            return;
        SelectedCustomValueIndex = 0;
        StringInput.EditString(SelectedEntityInstance.CustomValues[SelectedCustomValueIndex].ToString());
    }

    private void CheckToSelectCustomValue()
    {
        if (SelectedEntityInstance == null)
            return;

        if (EditorInput.SelectionUp.Pressed)
            SelectedCustomValueIndex += 1;
        if (EditorInput.SelectionDown.Pressed)
            SelectedCustomValueIndex -= 1;

        if (EditorInput.Enter.Pressed)
        {
            SelectedCustomValueIndex += 1;
            if (SelectedCustomValueIndex >= SelectedEntityInstance.CustomValues.Count)
            {
                SelectedCustomValueIndex = 0;
                SelectedEntityInstance = GetNextEntityInstance();
            }
        }

        SelectedCustomValueIndex = (SelectedCustomValueIndex + SelectedEntityInstance.CustomValues.Count) % SelectedEntityInstance.CustomValues.Count;
        if (EditorInput.SelectionUp.Pressed || EditorInput.SelectionDown.Pressed || EditorInput.Enter.Pressed)
            StringInput.EditString(SelectedEntityInstance.CustomValues[SelectedCustomValueIndex].ToString());
    }

    private EntityInstance GetNextEntityInstance()
    {
        var entityList = SelectedEntityInstance.Room?.GetEntityLayout().List
            .Where(entity => entity.CustomValues.Count > 0).ToList();
        var index = entityList!.IndexOf(SelectedEntityInstance);
        index += 1;
        if (index >= entityList.Count)
            index = 0;
        return entityList[index];
    }

    private void EditEntityValues()
    {
        StringInput.Update();
        if (StringInput.Finalized)
            SelectedEntityInstance.CustomValues[SelectedCustomValueIndex] = StringInput.OutputAsInt();
    }

    public override void Draw()
    {
        // Draw all entity properties (besides the selected one)
        foreach (var room in StageEditor.CurrentStage.RoomList)
        {
            foreach (var entityInstance in room.GetEntityLayout().List)
            {
                var skippedValueId = entityInstance == SelectedEntityInstance
                    ? SelectedCustomValueIndex
                    : -1; // don't skip any
                entityInstance.DrawCustomValues(skippedValueId);
            }
        }

        // draw entity selection
        if (SelectedEntityInstance == null)
            return;
        var entity = CollectionManager.GetEntityFromType(SelectedEntityInstance.EntityType);
        var selectedInstancePosition = SelectedEntityInstance.PositionAbsolute - entity.Sprite.Origin;
        Drawer.DrawRectangleOutline(selectedInstancePosition, entity.Sprite.Size, CustomColor.TransparentRed);

        // draw edited string
        if (string.IsNullOrEmpty(entity.CustomValueHandler.CustomValues[SelectedCustomValueIndex]?.ValueName))
            return;
        var position = selectedInstancePosition + entity.Sprite.Size + (0, SelectedCustomValueIndex * 8);
        var valueName = entity.CustomValueHandler.CustomValues[SelectedCustomValueIndex].ValueName;
        StringInput.Draw(position, valueName);
    }
}
