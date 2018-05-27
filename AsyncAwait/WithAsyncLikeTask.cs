using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwait
{
    class WithAsyncLikeTask
    {
        internal static Task<int> GetMessageLength()
        {
            Console.WriteLine($"#1 Starting GetMessageLengthAsync on thread {Thread.CurrentThread.ManagedThreadId}");
            Task<string> messageTask = Task.Run(() => DoTimeTakingWork());            
            var lengthTask = messageTask.ContinueWith((stringTask) =>
            {
                var message = stringTask.Result;
                var length = message.Length; //does some work on result
                return length; //then returns the final result
            });
            DoIndependentWork();
            Console.WriteLine($"#1 GetMessageLengthAsync completed on thread {Thread.CurrentThread.ManagedThreadId}");
            return lengthTask;
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
