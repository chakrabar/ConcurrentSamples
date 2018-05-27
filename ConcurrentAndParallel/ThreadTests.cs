using System;
using System.Threading;

namespace ConcurrentAndParallel
{
    class ThreadTests
    {
        public void SlowBuggyMethod()
        {
            Thread.Sleep(1500);
            Console.WriteLine("Current thread: " + Thread.CurrentThread.ManagedThreadId);
        }

        public void DoAsyncWork()
        {
            var thread = new Thread(SlowBuggyMethod);
            thread.Start(); //starts work on new thread
        }

        public void ValueReturningThread()
        {
            string result = null;
            Thread thread = new Thread(() => {
                Thread.Sleep(8000);
                result = "Thread work completed";
            });
            thread.Start();
            thread.Join(); //wait for thread to terminate
            Console.WriteLine(result);
        }
    }
}
