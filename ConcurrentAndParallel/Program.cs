using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ConcurrentAndParallel
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting process...");
            Console.WriteLine("Main thread: " + Thread.CurrentThread.ManagedThreadId);

            //new ThreadTests().ValueReturningThread();
            //new TaskTests().DoAsyncWork_6();
            //RunCancellableTask();
            //new ParallelExamples().RunAll();
            ContinuationTest.Execute();

            Console.WriteLine("Process completed");
            //Console.ReadKey();
        }

        internal static void RunCancellableTask()
        {
            CancellationTokenSource source = new CancellationTokenSource();
            var task = new CancellationTest().CancellableTask(source.Token);

            Console.WriteLine("Method invoked");

            Console.WriteLine("Press C to cancel");
            Console.WriteLine("");

            char ch = Console.ReadKey().KeyChar;
            if (ch == 'c' || ch == 'C')
            {
                source.Cancel();
                Console.WriteLine("\nTask cancellation requested.");
            }

            try
            {
                task.Wait();
            }
            catch (AggregateException ae)
            {
                if (ae.InnerExceptions.Any(e => e is TaskCanceledException))
                    Console.WriteLine("Task canceled exception detected");
                else
                    throw;

                //foreach (var e in ae.InnerExceptions)
                //{
                //    if (e is TaskCanceledException)
                //        Console.WriteLine("Task canceled exception detected");
                //    else
                //        throw;
                //}
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                source.Dispose();
            }
        }
    }
}
