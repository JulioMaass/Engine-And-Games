using Engine.ECS.Entities;
using Engine.Main;
using Engine.Managers.Audio;
using Engine.Managers.GameModes;
using Engine.Managers.Graphics;
using Engine.Managers.Input;

namespace Engine.Managers;

public static class DebugMode
{
    public static bool Paused { get; private set; }
    public static bool PauseNextFrame { get; private set; }
    public static bool IsOn { get; private set; } // = true;
    public static bool ShowMasks { get; private set; }
    public static int EntityCollisionCounter { get; set; }
    public static int SpawnCollisionCounter { get; set; }
    public static int DespawnCollisionCounter { get; set; }
    public static int TileCollisionCounter { get; set; }
    public static int TileCheckCounter { get; set; }
    public static int SolidScanCounter { get; set; }

    public static void Update() // TODO: Break down in smaller functions
    {
        // On/Off handling
        CheckToTurnOnAndOff();
        if (!IsOn) return;

        // Hard coded debug keys
        if (DebugInput.Fullscreen.Pressed) Video.ToggleFullScreen();
        if (DebugInput.ZoomIn.Pressed && !StringInput.IsOn) Camera.ZoomIn();
        if (DebugInput.ZoomOut.Pressed && !StringInput.IsOn) Camera.ZoomOut();
        if (DebugInput.Mute.Pressed) AudioManager.ToggleMute();
        if (DebugInput.Mask.Pressed) ShowMasks = !ShowMasks;
        if (DebugInput.Kill.Pressed) EntityManager.TriggerDeath(EntityManager.PlayerEntity);
        if (DebugInput.Reset.Pressed)
        {
            // TODO: Rough reset, may have bugs
            MenuManager.CreateMenu(GameManager.GameSpecificSettings.InitialMenu);
            GameLoopManager.QuitGameLoop();
            ScreenTextManager.CancelText();
        }

        // Pause/Unpause
        if (DebugInput.Pause.Pressed) Paused = !Paused;
        // Step loop
        if (PauseNextFrame)
        {
            Paused = true;
            PauseNextFrame = false;
        }
        if (DebugInput.Step.Pressed)
        {
            Paused = false;
            PauseNextFrame = true;
        }
    }

    public static void CleanUp()
    {
        EntityCollisionCounter = 0;
        SpawnCollisionCounter = 0;
        DespawnCollisionCounter = 0;
        TileCollisionCounter = 0;
        TileCheckCounter = 0;
        SolidScanCounter = 0;
    }

    private static void CheckToTurnOnAndOff()
    {
#if DEBUG
        if (!InputHandler.CtrlCommand(DebugInput.ToggleDebugMode))
            return;

        IsOn = !IsOn;
#endif
    }
}
