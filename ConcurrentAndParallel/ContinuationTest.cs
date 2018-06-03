using System;
using System.Threading;
using System.Threading.Tasks;

namespace ConcurrentAndParallel
{
    class ContinuationTest
    {
        public static void Execute()
        {
            var t1 = Task.Run(() =>
            {
                Thread.Sleep(3500);
                Console.WriteLine("Task 1 completed");
            });

            var t2 = t1.ContinueWith(t =>
            {
                Thread.Sleep(5000);
                Console.WriteLine("task 2 completed");
            });

            t2.Wait();
        }
    }
}
