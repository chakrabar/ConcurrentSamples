using System;
using System.Threading;

namespace Threading
{
    static class ThreadLocking_02
    {
        private static bool isCompleted = false;
        static readonly object lockCompleted = new object();

        internal static void Execute()
        {
            Thread thread = new Thread(Write);
            //worker thread
            thread.Start();

            Write();
        }

        private static void Write()
        {
            lock (lockCompleted)
            {
                if (!isCompleted)
                {
                    isCompleted = true;
                    Console.WriteLine("Notice for ONCE");
                }
            }
        }
    }
}
