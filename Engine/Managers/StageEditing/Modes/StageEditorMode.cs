using Engine.Managers.Input;
using Engine.Managers.StageEditing.Tools;
using Engine.Types;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Managers.StageEditing.Modes;

public abstract class StageEditorMode
{
    public abstract Button Shortcut { get; }
    public List<StageEditorTool> AvailableTools { get; set; }
    public StageEditorTool DefaultTool => AvailableTools[0];
    public StageEditorTool CurrentTool { get; set; }
    public abstract void Run();
    public abstract void Draw();
    public abstract void DrawMenu();

    public void CheckToToggleTools()
    {
        foreach (var tool in AvailableTools)
            if (InputHandler.ShortcutCommand(tool.Shortcut))
                ToggleTool(tool);
    }

    public void ToggleTool(StageEditorTool tool)
    {
        CurrentTool = tool;
        if (CurrentTool == null)
            Mouse.SetCursor(MouseCursor.Arrow);
        else
            CurrentTool.SetCursor();
    }

    public void ToggleTool(Type toolType)
    {
        foreach (var tool in AvailableTools.Where(tool => tool.GetType() == toolType))
        {
            ToggleTool(tool);
            return;
        }
    }
}
