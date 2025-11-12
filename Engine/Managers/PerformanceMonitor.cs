using System.Collections.Generic;
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
    private static List<LineCounter> LineCounters { get; } = new();

    private class LineCounter
    {
        public string Name { get; set; }
        public int Frames { get; set; }
        public int LineCount { get; set; }
        public int TotalLineCount { get; set; }
        public int PeakLineCount { get; set; }
        public int AverageLineCount { get; set; }
    }

    public static void Start(bool isActive)
    {
        // Stress test mode (each frame takes 16 ms)
        // Set to 15 to use only ~10% processing power
        // Set to 12 to use ~25% processing power (phone-like)
        // Set to 10 to use ~40% processing power (old computer-like)
#if DEBUG
        //System.Threading.Thread.Sleep(12);
#endif

        UpdateLineCounters();

        // Update counters
        FrameCount++;
        if (!DidDrawCall && isActive)
            FrameSkips++;

        // Reset for the new frame
        UpdateStopwatch.Restart();
        DidDrawCall = false;
    }

    private static void UpdateLineCounters()
    {
        foreach (var counter in LineCounters)
        {
            counter.Frames++;
            counter.TotalLineCount += counter.LineCount;
            if (counter.LineCount > counter.PeakLineCount)
                counter.PeakLineCount = counter.LineCount;
            counter.LineCount = 0;
            counter.AverageLineCount = counter.TotalLineCount / counter.Frames;
        }
    }

    public static void AddLineCount(string name)
    {
        var counter = LineCounters.Find(c => c.Name == name);
        if (counter == null)
        {
            counter = new LineCounter { Name = name };
            LineCounters.Add(counter);
        }
        counter.LineCount++;
    }

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
