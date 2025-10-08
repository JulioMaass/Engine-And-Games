using Engine.Types;
using Microsoft.Xna.Framework.Input;

namespace Engine.Managers.StageEditing.Tools;

public abstract class StageEditorTool
{
    public abstract MouseCursor MouseCursor { get; }
    public abstract Button Shortcut { get; }
    public virtual void SetCursor() => Mouse.SetCursor(MouseCursor);
    public abstract void Run();
    public abstract void Draw();
}
