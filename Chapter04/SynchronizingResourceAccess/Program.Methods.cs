namespace SynchronizingResourceAccess;

partial class Program
{
    static void MethodA()
    {
        // lock (SharedObjects.Conch)
        // {
        //     for (int i = 0; i < 5; i++)
        //     {
        //         Thread.Sleep(Random.Shared.Next(2000));
        //         SharedObjects.Message.Append("A");
        //         Write(".");
        //     }
        // }
        try
        {
            if (Monitor.TryEnter(SharedObjects.Conch, TimeSpan.FromSeconds(15)))
            {
                for (int i = 0; i < 5; i++)
                {
                    Thread.Sleep(Random.Shared.Next(2000));
                    SharedObjects.Message.Append("A");
                    Interlocked.Increment(ref SharedObjects.Counter);
                    Write(".");
                }
            }
            else
            {
                WriteLine("Method A timed out when entering a monitor on conch.");
            }
        }
        finally
        {
            Monitor.Exit(SharedObjects.Conch);
        }
    }

    static void MethodB()
    {
        // lock (SharedObjects.Conch)
        // {
        //     for (int i = 0; i < 5; i++)
        //     {
        //         Thread.Sleep(Random.Shared.Next(2000));
        //         SharedObjects.Message.Append("B");
        //         Write(".");
        //     }
        // }
        try
        {
            if (Monitor.TryEnter(SharedObjects.Conch, TimeSpan.FromSeconds(15)))
            {
                for (int i = 0; i < 5; i++)
                {
                    Thread.Sleep(Random.Shared.Next(2000));
                    SharedObjects.Message.Append("B");
                    Interlocked.Increment(ref SharedObjects.Counter);
                    Write(".");
                }
            }
            else
            {
                WriteLine("Method B timed out when entering a monitor on conch.");
            }
        }
        finally
        {
            Monitor.Exit(SharedObjects.Conch);
        }
    }
}