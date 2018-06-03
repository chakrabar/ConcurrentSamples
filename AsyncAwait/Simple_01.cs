using System;
using System.Threading;

namespace AsyncAwait
{
    class Simple
    {
        internal static int GetMessageLength()
        {
            Console.WriteLine($"#1 Starting GetMessageLength Simple on thread {Thread.CurrentThread.ManagedThreadId}");
            var message = DoTimeTakingWork();
            DoIndependentWork();
            var length = message.Length; //does some work on result
            Console.WriteLine($"#1 GetMessageLength Simple completed on thread {Thread.CurrentThread.ManagedThreadId}");
            return length; //then returns the final result
        }

        internal static void DoIndependentWork()
        {
            Console.WriteLine($"#3 Starting independent work on thread {Thread.CurrentThread.ManagedThreadId}");
            Thread.Sleep(2000); //works for 5 seconds
            Console.WriteLine($"#3 Independent work completed on thread {Thread.CurrentThread.ManagedThreadId}");
        }

        internal static string DoTimeTakingWork()
        {
            Console.WriteLine($"#2 Started time taking work on thread {Thread.CurrentThread.ManagedThreadId}");
            Thread.Sleep(5000); //works for 5 seconds
            Console.WriteLine($"#2 Time taking work completed on thread {Thread.CurrentThread.ManagedThreadId}");
            return $"Current time : {DateTime.Now.ToLongDateString()}";
        }
    }
}
