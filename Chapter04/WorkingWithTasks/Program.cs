// See https://aka.ms/new-console-template for more information
using System.Diagnostics;

namespace WorkingWithTasks;

partial class Program
{
    public static void Main(string[] args)
    {
        OutputThreadInfo();
        Stopwatch timer = Stopwatch.StartNew();
        
        // SectionTitle("Running methods sync on one thread");
        // SectionTitle("Running methods sync on one thread");
        // MethodA();
        // MethodB();
        // MethodC();
        //
        // WriteLine($"{timer.ElapsedMilliseconds: #, ##0}ms elapsed.");
        
        SectionTitle("Running method async on multiple threads.");
        timer.Restart();
        //
        // // OPTION 1
        // Task taskA = new(MethodA);
        // taskA.Start();
        // Task taskB = Task.Factory.StartNew(MethodB);
        // Task taskC = Task.Run(MethodC);
        //
        // // OPTION 2
        // Task[] tasks = { taskA, taskB, taskC };
        // Task.WaitAll(tasks);
        //
        //
        //
        
        //MULTI TASK ONE FOLLOWED FOR ANOTHER
        // Task<string> taskServiceThenSProc = Task.Factory
        //     .StartNew(CallWebService)
        //     .ContinueWith(previousTask => CallStoredProcedure(previousTask.Result));
        //
        // WriteLine($"Result: {taskServiceThenSProc.Result}");
        //
        // WriteLine($"{timer.ElapsedMilliseconds: #, ##0}ms elapsed.");
        
        //NESTED AND CHILD TASKS
        SectionTitle("Nested and Child Tasks");

        Task outerTask = Task.Factory.StartNew(OuterMethod);
        outerTask.Wait();
        WriteLine("Console app is stopping");
    }
}