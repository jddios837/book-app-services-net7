namespace WorkingWithTasks;

partial class Program
{
    static void MethodA()
    {
        TaskTitle("Starting Method A...");
        OutputThreadInfo();
        Thread.Sleep(3000);
        TaskTitle("Finishing Method A...");
    }
    
    static void MethodB()
    {
        TaskTitle("Starting Method B...");
        OutputThreadInfo();
        Thread.Sleep(2000);
        TaskTitle("Finishing Method B...");
    }
    
    static void MethodC()
    {
        TaskTitle("Starting Method C...");
        OutputThreadInfo();
        Thread.Sleep(1000);
        TaskTitle("Finishing Method C...");
    }
}