using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ConcurrentAndParallel
{
    class TplTest
    {
        //TPL (.NET 4.0) lets us do work asynchronously & concurrently, and let us continue with something else when done
        //The Task represents an asynchronous operation

        public void DamnSlowMethod()
        {
            Thread.Sleep(10000);
        }

        //Making synchronous to asynchronous >> execute on a separate thread
        public void SmartMethod()
        {
            var slowWorkTask = Task.Run(() => DamnSlowMethod());
            //OR new TaskFactory().StartNew()

            var slowTaskAwaiter = slowWorkTask.GetAwaiter();
            slowTaskAwaiter.OnCompleted(() => { var sum = 1 + 1; });

            slowWorkTask.ContinueWith(t => { var sum = 1 + 1; t.Dispose(); }); //t is the same task being continued on
        }

        public int SlowReturnigMethod()
        {
            Thread.Sleep(10000);
            return DateTime.Now.Millisecond;
        }

        public void AnotherSmartMethod()
        {
            var intTask = Task.Run(() => SlowReturnigMethod());
            //if any exception thrown inside Task.Run() that is not thrown back in context
            //for that check in ContinueWith() if Task.IsFaulted
            var result = 0;
            intTask.ContinueWith(t => result = t.Result); //if using t.Result, execution will wait until Result value is available
            //here it works fine, as it is in "ContinueWith", but doing it outside will make the execution to wait for task to return
        }

        public string SlowBuggyMethod()
        {
            Thread.Sleep(1500);
            if (DateTime.Now.Millisecond % 2 == 0)
                return "All good, we are even";
            else
                throw new Exception("You failed me!");
        }

        public string ReallySmartMethod()
        {
            var taskInt = Task.Run(() => SlowBuggyMethod());
            string msg = "Not initialized";
            var t2 = taskInt.ContinueWith(tInt =>
            {
                if (tInt.IsFaulted)
                {
                    msg = "Exception: " + tInt.Exception.InnerExceptions.First().Message;
                }
                msg = tInt.Result;
            });
            //Thread.Sleep(2500); //this can get a result back. But that kills the purpose
            return msg;
        }

        public string WeirdSmartMethod()
        {
            string msg = "Not initialized";
            var task = Task.Run(() => SlowBuggyMethod());
            task.ConfigureAwait(true) //will try to run the continuation on original context, generally useful in UI thread context
                .GetAwaiter()
                .OnCompleted(() =>
                {
                    msg = task.IsFaulted ?
                        "Exception: " + task.Exception.InnerExceptions.First().Message :
                        task.Result;
                });
            return msg;
        }

        public void NewTaskInstance()
        {
            var task = new Task(() => DamnSlowMethod());
            task.Start();
            new Task(() => DamnSlowMethod()).Start();
        }
    }
}
