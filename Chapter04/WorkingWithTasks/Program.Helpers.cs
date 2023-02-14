namespace WorkingWithTasks;

partial class Program
{
    static void SectionTitle(string title)
    {
        ConsoleColor previousColor = ForegroundColor;
        ForegroundColor = ConsoleColor.DarkYellow;
        WriteLine("*");
        WriteLine($"* {title}");
        WriteLine("*");
        ForegroundColor = previousColor;
    }

    static void TaskTitle(string title)
    {
        ConsoleColor previousColor = ForegroundColor;
        ForegroundColor = ConsoleColor.Green;
        WriteLine("*");
        WriteLine($"* {title}");
        WriteLine("*");
        ForegroundColor = previousColor;
    }

    static void OutputThreadInfo()
    {
        Thread t = Thread.CurrentThread;
        ConsoleColor previousColor = ForegroundColor;
        ForegroundColor = ConsoleColor.DarkCyan;
        WriteLine("Thread ID: {0}, Priority: {1}, Background: {2}, Name: {3}",
            t.ManagedThreadId, t.Priority, t.IsBackground, t.Name ?? "null");

        ForegroundColor = previousColor;
    }

    static decimal CallWebService()
    {
        TaskTitle("Starting call to web service...");
        OutputThreadInfo();
        Thread.Sleep((new Random()).Next(2000, 4000));
        TaskTitle("Finished call to web service.");
        return 89.99M;
    }

    static string CallStoredProcedure(decimal amount)
    {
        TaskTitle("Starting call to stored procedure...");
        OutputThreadInfo();
        Thread.Sleep((new Random()).Next(2000, 4000));
        TaskTitle("Finished call to stored procedure.");

        return $"12 products cost more than {amount:C}";
    }

    static void OuterMethod()
    {
        TaskTitle("Outer method starting...");
        Task innerTask = Task.Factory
            .StartNew(InnerMethod, TaskCreationOptions.AttachedToParent);
        TaskTitle("Outer method finished.");
    }

    private static void InnerMethod()
    {
        TaskTitle("Inner method starting...");
        Thread.Sleep(2000);
        TaskTitle("Inner method finished.");
    }
}