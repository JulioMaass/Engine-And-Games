using System.Diagnostics;

namespace Engine.Managers;

public static class PerformanceMonitor
{
    private static Stopwatch UpdateStopwatch { get; } = Stopwatch.StartNew();
    private static bool DidDrawCall { get; set; }
    private static double FrameUpdateLength { get; set; }
    private static double FrameUpdateAndDrawLength { get; set; }
    private static int FrameCount { get; set; }
    private static int FrameSkips { get; set; }

    // Properties to check performance in other parts of the code
    private static int LineCounter1 { get; set; }
    private static int LineCounter2 { get; set; }
    private static int LineCounter3 { get; set; }

    public static void Start(bool isActive)
    {
        // Stress test mode (each frame takes 16 ms)
        // Set to 15 to use only ~10% processing power
        // Set to 12 to use ~25% processing power (phone-like)
        // Set to 10 to use ~40% processing power (old computer-like)
#if DEBUG
        System.Threading.Thread.Sleep(12);
#endif        

        LineCounter1 = 0;
        LineCounter2 = 0;
        LineCounter3 = 0;

        // Update counters
        FrameCount++;
        if (!DidDrawCall && isActive)
            FrameSkips++;

        // Reset for the new frame
        UpdateStopwatch.Restart();
        DidDrawCall = false;
    }

    public static void AddLineCount1() =>
        LineCounter1++;
    public static void AddLineCount2() =>
        LineCounter2++;
    public static void AddLineCount3() =>
        LineCounter3++;

    public static void End(bool isActive)
    {
        UpdateStopwatch.Stop();

        // Finish measurement for the last frame
        FrameUpdateLength = UpdateStopwatch.Elapsed.TotalMilliseconds;

        if (isActive && DebugMode.PauseNextFrame && !DebugMode.Paused)
        { } // Set breakpoint to break when stepping in debug mode
    }

    public static void CheckForDrawCall()
    {
        DidDrawCall = true;
    }
}
