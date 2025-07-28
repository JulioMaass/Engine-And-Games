using Engine.ECS.Entities;
using Engine.GameSpecific;
using Engine.Managers;
using Engine.Managers.Audio;
using Engine.Managers.GameModes;
using Engine.Managers.GlobalManagement;
using Engine.Managers.Graphics;
using Engine.Managers.StageEditing;
using Engine.Managers.StageHandling;
using Microsoft.Xna.Framework;

namespace Engine.Main;

public static class GameManager // Role: Control game states and main loop
{
    public static Game Game { get; set; }
    public static GameSpecificSettings GameSpecificSettings { get; set; }
    public static bool Paused { get; private set; }
    private static bool WindowIsOnFocus { get; set; } = true;

    public static void Initialize()
    {
        GlobalManager.Initialize();
        StageManager.Initialize();
        Camera.Initialize();
        Video.Initialize();
        Drawer.Initialize();
        AudioManager.Initialize();
        MenuManager.CreateMenu(GameSpecificSettings.InitialMenu);
    }

    public static void Load()
    {
        Drawer.LoadContent();
        AudioLibrary.LoadAudioLibrary();
    }

    public static void CheckToPauseEngine()
    {
        WindowIsOnFocus = false;
        AudioManager.PauseMusic();
    }

    public static void CheckToUnpauseEngine()
    {
        if (WindowIsOnFocus) return;

        WindowIsOnFocus = true;
        AudioManager.ResumeMusic();
    }

    public static void Update()
    {
        EntityManager.RunComponentEnforcerCheckingList();

        // Audio
        AudioManager.Update();

        // Debug
        Input.UpdateDebugInput();
        DebugMode.Update();
        if (DebugMode.Paused) return;
        DebugMode.CleanUp();

        // Video // TODO: Check where to place this so the screen dimmer works as intended at every intended use
        ScreenDimmer.Update();
        ScreenTextManager.Update();

        // Run stage editor loop
        StageEditor.Update();
        if (StageEditor.IsOn) return;

        // Game state
        Input.UpdateGameInput();
        CheckToPause();
        if (MenuManager.IsActive)
            MenuManager.Update();
        else if (!Paused)
            GameLoopManager.Update();
    }

    public static void CheckToPause()
    {
        if (!Input.Start.Pressed)
            return;
        if (Paused || GameLoopManager.GameCurrentLoop == GameLoopManager.GameMainLoop) // Only pause on main loop
            Paused = !Paused;

        if (Paused)
        {
            if (GameSpecificSettings.PauseMenu != null)
                MenuManager.CreateMenu(GameSpecificSettings.PauseMenu);
        }
        else
            MenuManager.Clear();
    }
}
