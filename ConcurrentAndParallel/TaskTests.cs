using System;
using System.Threading;
using System.Threading.Tasks;

namespace ConcurrentAndParallel
{
    public class TaskTests
    {
        public async void SlowMethod()
        {
            await Task.Delay(6000);
            Console.WriteLine("Current thread: " + Thread.CurrentThread.ManagedThreadId);
        }

        public string SlowBuggyMethod()
        {
            Console.WriteLine("SlowBuggyMethod Thread: " + Thread.CurrentThread.ManagedThreadId);
            Thread.Sleep(2500);
            throw new Exception("You failed me!");
        }

        public void DoAsyncWork()
        {
            Task.Run(() => SlowBuggyMethod());
        }

        //doesn't work
        public void DoAsyncWork_2()
        {
            try
            {
                Task.Run(() => SlowBuggyMethod());
            }
            catch (Exception)
            {
                Console.WriteLine("Exception caught in DoAsyncWork_2");
            }            
        }

        //works
        public void DoAsyncWork_3()
        {
            //the original thread #2
            var task = Task.Run(() => SlowBuggyMethod()); //future thread #3
            task.ContinueWith(t =>
            {
                //future thread #4
                if (task.IsFaulted)
                    Console.WriteLine("Exception: " + t.Exception.Message);
                else
                    Console.WriteLine("Completed successfully!");
            });
            //thread #2 continues
        }

        //works
        public void DoAsyncWork_4()
        {
            var task = Task.Run(() => SlowBuggyMethod());
            task.ContinueWith(t => {
                Console.WriteLine("Thread: " + Thread.CurrentThread.ManagedThreadId);
                Console.WriteLine("Exception: " + t.Exception.Message);
            },
            TaskContinuationOptions.OnlyOnFaulted);
        }

        //well, works. Not that intuitive though
        public void DoAsyncWork_5()
        {
            var task = Task.Run(() => SlowBuggyMethod());
            try
            {
                task.Wait();
                //Task.WhenAny()
                Console.WriteLine("Completed successfully!");
            }
            catch (Exception)
            {
                Console.WriteLine("Exception: " + task.Exception.Message);
            }
        }

        public void DoAsyncWork_6()
        {
            Console.WriteLine("Thread in: " + Thread.CurrentThread.ManagedThreadId);
            var task = Task.Run(() => SlowBuggyMethod());
            task.ConfigureAwait(true) //will try to run the continuation on captured context (same as the task itself), generally useful in UI thread context
                .GetAwaiter()
                .OnCompleted(() =>
                {
                    Console.WriteLine("Continuation Thread: " + Thread.CurrentThread.ManagedThreadId);
                    if (task.IsFaulted)
                        Console.WriteLine("Exception: " + task.Exception.Message);
                    else
                        Console.WriteLine("Completed successfully!");
                });
            Console.WriteLine("Thread out: " + Thread.CurrentThread.ManagedThreadId);
        }

        public void StartNewVsRun()
        {
            var outTask1 = Task.Factory.StartNew(() => {
                Task<int> innerTask = Task.Run(() => 100);
                return innerTask;
            });
            //here outTask1 is Task<Task<int>>

            var outTask2 = Task.Run(() => {
                Task<int> innerTask = Task.Run(() => 100);
                return innerTask;
            });
            //outTask2 is Task<int>

            var outTask3 = Task.Factory.StartNew(() => {
                Task<int> innerTask = Task.Run(() => 100);
                return innerTask;
            }).Unwrap();
            //here outTask3 is Task<int>
        }
    }
}
