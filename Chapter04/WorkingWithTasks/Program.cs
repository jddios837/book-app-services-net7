// See https://aka.ms/new-console-template for more information
using System.Diagnostics;

namespace WorkingWithTasks;

partial class Program
{
    public static void Main(string[] args)
    {
        OutputThreadInfo();
        Stopwatch timer = Stopwatch.StartNew();
        
        SectionTitle("Running methods sync on one thread");
        MethodA();
        MethodB();
        MethodC();
        
        WriteLine($"{timer.ElapsedMilliseconds: #, ##0}ms elapsed.");
    }
}