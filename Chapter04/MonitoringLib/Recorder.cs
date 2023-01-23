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
        
    }
}