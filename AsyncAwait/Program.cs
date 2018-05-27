using System;
using System.Diagnostics;
using System.Threading;

namespace AsyncAwait
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"STARTING PROGRAM on thread {Thread.CurrentThread.ManagedThreadId}");
            var sw = new Stopwatch();
            sw.Start();

            //var messageLength = Simple.GetMessageLength();
            //var messageLength = WithTasks.GetMessageLength();

            //var lengthTask = WithAsync.GetMessageLengthAsync();
            var lengthTask = WithAsyncLikeTask.GetMessageLength();

            DoSomeOtherWork();

            var messageLength = lengthTask.Result;
            Console.WriteLine($"The messsage length is {messageLength}");

            Console.WriteLine($"TERMINATING PROGRAM on thread {Thread.CurrentThread.ManagedThreadId}");
            sw.Stop();
            Console.WriteLine($"Total Time taken = {sw.ElapsedMilliseconds} ms");
            Console.ReadLine();
        }

        static void DoSomeOtherWork()
        {
            Thread.Sleep(1000);
            Console.WriteLine($"#4 Doing IMPORTANT work on thread {Thread.CurrentThread.ManagedThreadId}");
        }
    }
}
