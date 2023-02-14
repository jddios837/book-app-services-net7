// See https://aka.ms/new-console-template for more information
using System.Diagnostics;

namespace SynchronizingResourceAccess;

partial class Program
{
    static void Main(string[] args)
    {
        WriteLine("Please wait for the tasks to complete.");
        Stopwatch watch = Stopwatch.StartNew();
        Task a = Task.Factory.StartNew(MethodA);
        Task b = Task.Factory.StartNew(MethodB);

        Task.WaitAll(new Task[] { a, b });
        WriteLine();
        WriteLine($"Results:: {SharedObjects.Message}.");
        WriteLine($"{watch.ElapsedMilliseconds:N0} elapsed milliseconds.");
    }
}

