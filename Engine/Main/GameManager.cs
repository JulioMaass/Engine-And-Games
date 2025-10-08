using Engine.ECS.Entities;
using Engine.GameSpecific;
using Engine.Managers;
using Engine.Managers.Audio;
using Engine.Managers.GameModes;
using Engine.Managers.GlobalManagement;
using Engine.Managers.Graphics;
using Engine.Managers.Input;
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
        InputHandler.Initialize();
        GlobalManager.Initialize();
        StageManager.Initialize();
        Camera.Initialize();
        Video.Initialize();
        Drawer.Initialize();
        ScreenTest.Initialize();
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
        InputHandler.Update();

        // Audio
        AudioManager.Update();

        // Debug
        InputHandler.UpdateInputList(InputHandler.DebugInputList);
        DebugMode.Update();
        if (DebugMode.Paused)
            return;
        DebugMode.CleanUp();

        // Video // TODO: Check where to place this so the screen dimmer works as intended at every intended use
        ScreenTest.Update();
        CrtManager.Update();
        ScreenDimmer.Update();
        ScreenTextManager.Update();

        // Run stage editor loop
        InputHandler.UpdateInputList(InputHandler.EditorInputList);
        StageEditor.Update();
        if (StageEditor.IsOn)
            return;

        // Game state
        InputHandler.UpdateInputList(InputHandler.GameInputList);
        if (MenuManager.IsActive)
            MenuManager.Update();
        else
        {
            CheckToPause();
            if (!Paused)
                GameLoopManager.Update();
        }
    }

    public static void CheckToPause()
    {
        if (!GameInput.Start.Pressed)
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
