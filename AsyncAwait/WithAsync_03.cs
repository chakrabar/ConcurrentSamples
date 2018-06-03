using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwait
{
    class WithAsync_03
    {
        //await can be used ONLY inside a async method
        //async methods need NOT have await. But then it runs synchronously
        //having the Async suffix in method name is totally optional, but that is the general convention
        internal async static Task<int> GetMessageLengthAsync()
        {
            Console.WriteLine($"#1 Starting GetMessageLengthAsync on thread {Thread.CurrentThread.ManagedThreadId}");
            Task<string> task = Task.Run(() => DoTimeTakingWork());
            DoIndependentWork();
            var message = await task; //awaits the result
            var length = message.Length; //does some work on awaited result
            Console.WriteLine($"#1 GetMessageLengthAsync completed on thread {Thread.CurrentThread.ManagedThreadId}");
            return length; //then returns the final result
            //the result is int, but because of the async-await, it returns as Task<int>
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
