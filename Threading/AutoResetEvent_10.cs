using System;
using System.Threading;
using System.Threading.Tasks;

namespace Threading
{
    class AutoResetEvent_10
    {
        static EventWaitHandle autoResetEvent = new EventWaitHandle(false, EventResetMode.AutoReset);
        //false means initially the handle is unsignaled or reset
        internal static void Execute()
        {
            Task.Run(() => DoWork()); //another thread to wait for signal
            Thread.Sleep(5000); //do some work
            autoResetEvent.Set(); //signal to opne i.e. state = signaleded
            //autoResetEvent.Reset(); //signal to close i.e. state = unsignaled            
        }

        private static void DoWork()
        {
            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} waiting for signal");
            autoResetEvent.WaitOne();
            //once it gets signal, wait handle AutoReset making it unsignaled
            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} got signal");
            Thread.Sleep(3000); //do some work
            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} leaving...");
            autoResetEvent.Set(); //signal to let other thread work
        }

        internal static void Execute2()
        {
            var tasks = new Task[3];
            for (int i = 0; i < 3; i++)
            {
                tasks[i] = Task.Run(() => DoWork());
            }
            Thread.Sleep(5000); //do some work
            autoResetEvent.Set(); //signal to opne i.e. state = signaleded
            Task.WaitAll(tasks);
        }
    }
}
