using System;
using System.Threading;

namespace Threading
{
    static class SimpleThread_01
    {
        internal static void Execute()
        {
            Thread thread = new Thread(() => WriteUsingNewThread(1000));
            //worker thread
            thread.Name = "Worker-1"; //mostly for debugging
            thread.Start();

            //main thread (UI thread in WPF and the likes)
            for (int i = 0; i < 1000; i++)
            {
                Console.Write($" M-{i} ");
            }
        }

        private static void WriteUsingNewThread(int times)
        {
            if (times < 0)
                times = 1;
            for (int i = 0; i < times; i++)
            {
                Console.Write($" W-{i} ");
            }
        }
    }
}
