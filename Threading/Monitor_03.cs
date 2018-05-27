using System;
using System.Threading;

namespace Threading
{
    static class Monitor_03
    {
        private static bool isCompleted = false;
        static readonly object syncObj = new object();

        internal static void Execute()
        {
            Thread thread = new Thread(Write);
            //worker thread
            thread.Start();

            Write();
        }

        private static void Write()
        {
            Monitor.Enter(syncObj);
            try
            {                
                if (!isCompleted)
                {
                    Console.WriteLine("This controlled thing prints ONLY ONCE");
                    isCompleted = true;
                }
            }
            finally
            {
                Monitor.Exit(syncObj);
            }            
        }
    }
}
