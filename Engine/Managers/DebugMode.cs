using Engine.ECS.Entities;
using Engine.Main;
using Engine.Managers.Audio;
using Engine.Managers.GameModes;
using Engine.Managers.Graphics;

namespace Engine.Managers;

public static class DebugMode
{
    public static bool Paused { get; private set; }
    public static bool PauseNextFrame { get; private set; }
    private static bool IsOn { get; set; } // = true;
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
        if (Input.Fullscreen.Pressed) Video.ToggleFullScreen();
        if (Input.ZoomIn.Pressed && !StringInput.IsOn) Camera.ZoomIn();
        if (Input.ZoomOut.Pressed && !StringInput.IsOn) Camera.ZoomOut();
        if (Input.Mute.Pressed) AudioManager.ToggleMute();
        if (Input.Mask.Pressed) ShowMasks = !ShowMasks;
        if (Input.Kill.Pressed) EntityManager.TriggerDeath(EntityManager.PlayerEntity);
        if (Input.Reset.Pressed)
        {
            // TODO: Rough reset, may have bugs
            MenuManager.CreateMenu(GameManager.GameSpecificSettings.InitialMenu);
            GameLoopManager.QuitGameLoop();
            ScreenTextManager.CancelText();
        }

        // Pause/Unpause
        if (Input.Pause.Pressed) Paused = !Paused;
        // Step loop
        if (PauseNextFrame)
        {
            Paused = true;
            PauseNextFrame = false;
        }
        if (Input.Step.Pressed)
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
        if (!Input.CtrlCommand(Input.ToggleDebugMode))
            return;

        IsOn = !IsOn;
    }
}
