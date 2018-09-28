using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwait
{
    class ThreadAndAsyncLoacl
    {
        AsyncLocal<string> _asyncLocalString = new AsyncLocal<string>();
        ThreadLocal<string> _threadLocalString = new ThreadLocal<string>();

        internal async Task EntryPoint()
        {
            Console.WriteLine($"[Main] Starting A, CurrentThread: {Thread.CurrentThread.ManagedThreadId}");

            _asyncLocalString.Value = "Value 1";
            _threadLocalString.Value = "Value 1";
            var t1 = AsyncMethodA("Value 1");

            Console.WriteLine($"[Main] Starting B, CurrentThread: {Thread.CurrentThread.ManagedThreadId}");

            _asyncLocalString.Value = "Value 2";
            _threadLocalString.Value = "Value 2";
            var t2 = AsyncMethodB("Value 2");

            Console.WriteLine($"[Main] A, B Started. CurrentThread: {Thread.CurrentThread.ManagedThreadId}");

            // Await both calls
            await t1;
            await t2;

            Console.WriteLine($"[Main] A, B Completed. CurrentThread: {Thread.CurrentThread.ManagedThreadId}");
            return;
        }

        async Task AsyncMethodA(string expectedValue)
        {
            Console.WriteLine($"[A] Entering AsyncMethodA, Thread: {Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine($"[A] Expected: {expectedValue}, ThreadLoacl: {_threadLocalString.Value}, AsyncLocal: {_asyncLocalString.Value}");
            await Task.Delay(300);
            Console.WriteLine($"[A] Exiting AsyncMethodA, Thread: {Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine($"[A] Expected: {expectedValue}, ThreadLoacl: {_threadLocalString.Value}, AsyncLocal: {_asyncLocalString.Value}");
        }

        async Task AsyncMethodB(string expectedValue)
        {
            Console.WriteLine($"[B] Entering AsyncMethodB, Thread: {Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine($"[B] Expected: {expectedValue}, ThreadLoacl: {_threadLocalString.Value}, AsyncLocal: {_asyncLocalString.Value}");
            await Task.Delay(300);
            Console.WriteLine($"[B] Exiting AsyncMethodB, Thread: {Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine($"[B] Expected: {expectedValue}, ThreadLoacl: {_threadLocalString.Value}, AsyncLocal: {_asyncLocalString.Value}");
        }
    }
//OUTPUT
//[Main] Starting A, CurrentThread: 1
//[A] Entering AsyncMethodA, Thread: 1
//[A] Expected: Value 1, ThreadLoacl: Value 1, AsyncLocal: Value 1
//[Main] Starting B, CurrentThread: 1
//[B] Entering AsyncMethodB, Thread: 1
//[B] Expected: Value 2, ThreadLoacl: Value 2, AsyncLocal: Value 2
//[Main] A, B Started. CurrentThread: 1
//[B] Exiting AsyncMethodB, Thread: 4
//[B] Expected: Value 2, ThreadLoacl: , AsyncLocal: Value 2
//[A] Exiting AsyncMethodA, Thread: 5
//[A] Expected: Value 1, ThreadLoacl: , AsyncLocal: Value 1
//[Main] A, B Completed. CurrentThread: 5
}