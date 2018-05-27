using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Threading
{
    class ManualResetEvent_11
    {
        static EventWaitHandle manualResetEvent =
            new EventWaitHandle(false, EventResetMode.ManualReset);
        internal static void Execute()
        {
            var tasks = new Task[3];
            for (int i = 0; i < 3; i++)
            {
                tasks[i] = Task.Run(() => DoWork());
            }
            Task.Delay(5000)
                .ContinueWith((t) => manualResetEvent.Set());
            Task.WaitAll(tasks);
        }
        private static void DoWork()
        {
            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} waiting for signal");
            manualResetEvent.WaitOne();
            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} got signal");
            Thread.Sleep(1000); //do some work
            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} leaving...");
        }
    }
}
