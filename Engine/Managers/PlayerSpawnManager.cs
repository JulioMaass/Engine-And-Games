using Engine.Managers.Graphics;

namespace Engine.Managers;

public static class PlayerSpawnManager
{
    private static int DeathTime => 180;
    private static int DeathTimer { get; set; }

    public static void StartDeathCounter()
    {
        DeathTimer = DeathTime;
        ScreenDimmer.DimScreen(1, 0, 30, 120);
    }

    public static bool DeathTimeReached()
    {
        DeathTimer--;
        return DeathTimer <= 0;
    }
}
