using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwait
{
    class WithAsyncLikeTask_05
    {
        internal static Task<int> GetMessageLength()
        {
            Console.WriteLine($"#1 Starting GetMessageLengthAsync on thread {Thread.CurrentThread.ManagedThreadId}");
            Task<string> messageTask = DoTimeTakingWorkAsync();            
            var lengthTask = messageTask.ContinueWith((stringTask) =>
            {
                Console.WriteLine($"#1.2 Continuation runs on thread {Thread.CurrentThread.ManagedThreadId}");
                var message = stringTask.Result; //gets the result
                var length = message.Length; //does some work on result
                return length; //then returns the final result
            });
            //1. The continuation is implemented in a CLR auto-generated class with a state machine
            //2. Here, await also captures the current SynchronizationContext  & TaskScheduler
            //3. If SynchronizationContext was null, the continuation will run on the TaskScheduler
            //4. Else the continuation is posted to the capctured SynchronizationContext (has thread affinity)
            DoIndependentWork();
            Console.WriteLine($"#1 GetMessageLengthAsync returning from thread {Thread.CurrentThread.ManagedThreadId}");
            return lengthTask; //it has to return, cannot resume work after awaited work is done
        }

        internal static Task<int> GetMessageLengthV2()
        {
            Console.WriteLine($"#1 Starting GetMessageLengthAsync on thread {Thread.CurrentThread.ManagedThreadId}");
            Task<string> messageTask = DoTimeTakingWorkAsync();

            TaskScheduler targetScheduler = SynchronizationContext.Current != null
                ? TaskScheduler.FromCurrentSynchronizationContext() //e.g. app with UI thread
                : TaskScheduler.Current; //when there is NO SynchronizationContext e.g. Console

            var lengthTask = messageTask.ContinueWith((stringTask) =>
            {
                Console.WriteLine($"#1.2 Continuation runs on thread {Thread.CurrentThread.ManagedThreadId}");
                var message = stringTask.Result; //gets the result
                var length = message.Length; //does some work on result
                return length; //then returns the final result
            }, targetScheduler);
            DoIndependentWork();
            Console.WriteLine($"#1 GetMessageLengthAsync returning from thread {Thread.CurrentThread.ManagedThreadId}");
            return lengthTask; //it has to return, cannot resume work after awaited work is done
        }

        internal static void DoIndependentWork()
        {
            Console.WriteLine($"#3 Starting independent work on thread {Thread.CurrentThread.ManagedThreadId}");
            Thread.Sleep(2000); //works for 5 seconds
            Console.WriteLine($"#3 Independent work completed on thread {Thread.CurrentThread.ManagedThreadId}");
        }

        internal static async Task<string> DoTimeTakingWorkAsync()
        {
            Console.WriteLine($"#2 Started time taking work on thread {Thread.CurrentThread.ManagedThreadId}");
            var messageTask = Task.Run(() =>
            {
                Thread.Sleep(5000); //works for 5 seconds
                Console.WriteLine($"#2 Time taking work completed on thread {Thread.CurrentThread.ManagedThreadId}");
            });
            await messageTask;
            return $"Current time : {DateTime.Now.ToLongDateString()}";
        }

        //All tasks are awaitable
        Task Crap()
        {
            //File.WriteAllBytesAsync();
            //new HttpClient().GetStreamAsync();
            return Task.Run(() => Thread.Sleep(2000));
        }

        //Non-Task awaitable
        async Task<int> Crap2()
        {
            await Task.Yield();
            return 5;
        }
    }
}
