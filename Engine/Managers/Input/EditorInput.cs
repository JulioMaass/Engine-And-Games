using Engine.Types;
using Microsoft.Xna.Framework.Input;

namespace Engine.Managers.Input;

public static class EditorInput
{
    // Stage editor input
    public static Button Save { get; private set; } = new(Keys.S);
    public static Button Edit { get; private set; } = new(Keys.E);
    public static Button Panning { get; private set; } = new(Keys.Space);
    public static Button LaptopMode { get; private set; } = new(Keys.L);
    // Tile mode
    public static Button TileMode { get; private set; } = new(Keys.T);
    public static Button TileBrushTool { get; private set; } = new(Keys.D);
    public static Button TileRectangleTool { get; private set; } = new(Keys.C);
    // Entity mode
    public static Button EntityMode { get; private set; } = new(Keys.E);
    public static Button EntityEditTool { get; private set; } = new(Keys.D);
    public static Button EntityPlaceTool { get; private set; } = new(Keys.E);
    public static Button SelectionUp { get; private set; } = new(Keys.Up);
    public static Button SelectionDown { get; private set; } = new(Keys.Down);
    public static Button Enter { get; private set; } = new(Keys.Enter);
    // Room mode
    public static Button RoomMode { get; private set; } = new(Keys.R);
    public static Button RoomMoveTool { get; private set; } = new(Keys.M);
    public static Button RoomCutAndGlueTool { get; private set; } = new(Keys.C);
    public static Button RoomResizeTool { get; private set; } = new(Keys.S);

    public static void Initialize()
    {
        InputHandler.DebugInputList.Add(Save);
        InputHandler.DebugInputList.Add(Edit);
        InputHandler.DebugInputList.Add(Panning);
        InputHandler.DebugInputList.Add(LaptopMode);

        InputHandler.DebugInputList.Add(TileMode);
        InputHandler.DebugInputList.Add(TileBrushTool);
        InputHandler.DebugInputList.Add(TileRectangleTool);

        InputHandler.DebugInputList.Add(EntityMode);
        InputHandler.DebugInputList.Add(EntityEditTool);
        InputHandler.DebugInputList.Add(EntityPlaceTool);
        InputHandler.DebugInputList.Add(SelectionUp);
        InputHandler.DebugInputList.Add(SelectionDown);
        InputHandler.DebugInputList.Add(Enter);

        InputHandler.DebugInputList.Add(RoomMode);
        InputHandler.DebugInputList.Add(RoomMoveTool);
        InputHandler.DebugInputList.Add(RoomCutAndGlueTool);
        InputHandler.DebugInputList.Add(RoomResizeTool);
    }
}
