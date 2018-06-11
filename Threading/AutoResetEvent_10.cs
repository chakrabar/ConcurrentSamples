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

        static void AlternativeInitialization()
        {
            var k = new AutoResetEvent(false);
            var l = new ManualResetEvent(false);
        }

        private static void DoWork() //critical section that works based on signal
        {
            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} waiting for signal");
            autoResetEvent.WaitOne();
            //once it gets signal, wait handle AutoReset making it unsignaled again
            //so only one thread will proceed, others keep waiting for signal
            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} got signal");
            Thread.Sleep(3000); //do some work
            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} leaving...");
            autoResetEvent.Set(); //signal to make it open, i.e. state = signaled
        }

        internal static void Execute2()
        {
            var tasks = new Task[3]; //get 3 threads to do some work
            for (int i = 0; i < 3; i++)
            {
                tasks[i] = Task.Run(() => DoWork());
            }
            Thread.Sleep(5000); //indicating, it's doing some other work
            autoResetEvent.Set(); //first signal to open i.e. state = signaled
            Task.WaitAll(tasks); //wait for all 3 tasks to complete
        }
    }
}
