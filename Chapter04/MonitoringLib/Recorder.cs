using System.Diagnostics; // Stopwatch

using static System.Diagnostics.Process;

namespace MonitoringLib;

public class Recorder
{
    public static Stopwatch time = new();

    private static long bytesPhysicalBefore = 0;
    private static long bytesVirtualBefore = 0;

    public static void Start()
    {
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
        
        // store the current physical and virtual memory use
        bytesPhysicalBefore = GetCurrentProcess().WorkingSet64;
        bytesVirtualBefore = GetCurrentProcess().VirtualMemorySize64;
        
        time.Restart();
    }

    public static void Stop()
    {
        time.Stop();

        long bytesPhysicalAfter = GetCurrentProcess().WorkingSet64;
        long bytesVirtualAfter  = GetCurrentProcess().VirtualMemorySize64;
        
        WriteLine("{0:N0} physical bytes used.", 
            bytesPhysicalAfter - bytesPhysicalBefore);
        
        WriteLine("{0:N0} virtual bytes used.", 
            bytesVirtualAfter - bytesVirtualBefore);
        
        WriteLine("{0} time span elapsed.", time.Elapsed);
        
        WriteLine("{0:N0} total milliseconds elapsed.", time.ElapsedMilliseconds);
    }
}