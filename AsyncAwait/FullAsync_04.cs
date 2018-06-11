using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwait
{
    class FullAsync_04
    {
        internal async static Task<int> GetMessageLengthAsync()
        {
            Console.WriteLine($"#1 Starting GetMessageLengthAsync on thread {Thread.CurrentThread.ManagedThreadId}");
            var stringTask = DoTimeTakingWorkAsync();
            DoIndependentWork();
            var message = await stringTask; //awaits the result
            var length = message.Length; //does some work on awaited result
            Console.WriteLine($"#1 GetMessageLengthAsync completed on thread {Thread.CurrentThread.ManagedThreadId}");
            return length; //then returns the final result
        }

        internal static void DoIndependentWork()
        {
            Console.WriteLine($"#3 Starting independent work on thread {Thread.CurrentThread.ManagedThreadId}");
            Thread.Sleep(2000); //works for 5 seconds
            Console.WriteLine($"#3 Independent work completed on thread {Thread.CurrentThread.ManagedThreadId}");
        }

        internal async static Task<string> DoTimeTakingWorkAsync()
        {
            Console.WriteLine($"#2 Started time taking work on thread {Thread.CurrentThread.ManagedThreadId}");
            var messageTask = Task.Run(() =>
            {
                Thread.Sleep(5000); //works for 5 seconds
                Console.WriteLine($"#2 Time taking work completed on thread {Thread.CurrentThread.ManagedThreadId}");
            });
            await messageTask;
            Console.WriteLine($"#2 returns from thread {Thread.CurrentThread.ManagedThreadId}");
            return $"Current time : {DateTime.Now.ToLongDateString()}";
        }
    }
}
